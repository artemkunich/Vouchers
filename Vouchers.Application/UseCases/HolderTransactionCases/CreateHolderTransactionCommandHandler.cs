using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Core;
using Vouchers.Application.Infrastructure;
using Vouchers.Domains;
using Vouchers.Application.Commands.HolderTransactionCommands;

namespace Vouchers.Application.UseCases.HolderTransactionCases
{
    internal sealed class CreateHolderTransactionCommandHandler : IAuthIdentityHandler<CreateHolderTransactionCommand, Guid>
    {
        private readonly IRepository<DomainAccount> _domainAccountRepository;
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<Unit> _unitRepository;
        private readonly IRepository<UnitType> _unitTypeRepository;
        private readonly IRepository<AccountItem> _accountItemRepository;
        private readonly IRepository<HolderTransaction> _holderTransactionRepository;
        private readonly IRepository<HolderTransactionRequest> _holderTransactionRequestRepository;

        public CreateHolderTransactionCommandHandler(IRepository<DomainAccount> domainAccountRepository, IRepository<Account> accountRepository, IRepository<Unit> unitRepository, IRepository<UnitType> unitTypeRepository, IRepository<AccountItem> accountItemRepository, IRepository<HolderTransaction> holderTransactionRepository, IRepository<HolderTransactionRequest> holderTransactionRequestRepository)
        {
            _domainAccountRepository = domainAccountRepository;
            _accountRepository = accountRepository;
            _unitRepository = unitRepository;
            _unitTypeRepository = unitTypeRepository;
            _accountItemRepository = accountItemRepository;
            _holderTransactionRepository = holderTransactionRepository;
            _holderTransactionRequestRepository = holderTransactionRequestRepository;
        }

        public async Task<Guid> HandleAsync(CreateHolderTransactionCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var creditorDomainAccount = await _domainAccountRepository.GetByIdAsync(command.CreditorAccountId);
            if (creditorDomainAccount?.IdentityId != authIdentityId)
                throw new ApplicationException("Operation is not allowed");
            if (!creditorDomainAccount.IsConfirmed)
                throw new ApplicationException("Creditor account is not activated");

            HolderTransactionRequest holderTransactionRequest = null;
            if (command.HolderTransactionRequestId is not null)
            {
                holderTransactionRequest = await _holderTransactionRequestRepository.GetByIdAsync(command.HolderTransactionRequestId.Value);
                if(holderTransactionRequest is null)
                    throw new ApplicationException("Transaction request is not found");
            }

            var debtorDomainAccount = await _domainAccountRepository.GetByIdAsync(command.DebtorAccountId);
            if(debtorDomainAccount is null)
                throw new ApplicationException("Debtor account does not exist");
            if (!debtorDomainAccount.IsConfirmed)
                throw new ApplicationException("Debtor account is not activated");

            var unitType = await _unitTypeRepository.GetByIdAsync(command.UnitTypeId);
            if (unitType is null)
                throw new ApplicationException("Voucher value does not exist");

            var creditorAccount = await _accountRepository.GetByIdAsync(command.CreditorAccountId);
            if(creditorAccount is null)
                throw new ApplicationException("Creditor's account does not exist");

            var debtorAccount = await _accountRepository.GetByIdAsync(command.DebtorAccountId);
            if (debtorAccount is null)
                throw new ApplicationException("Debtor's account does not exist");

            HolderTransaction transaction = HolderTransaction.Create(creditorAccount, debtorAccount, unitType, command.Message);

            foreach (var item in command.Items)
            {
                var creditAccountItem = (await _accountItemRepository.GetByExpressionAsync(accItem => accItem.HolderAccountId == command.CreditorAccountId && accItem.Unit.Id == item.Item1)).FirstOrDefault();
                if (creditAccountItem is null)
                {
                    throw new ApplicationException($"User {command.CreditorAccountId} does not have account for voucher {item.Item2}");
                }
                var debitAccountItem = (await _accountItemRepository.GetByExpressionAsync(accItem => accItem.HolderAccountId == command.DebtorAccountId && accItem.Unit.Id == item.Item1)).FirstOrDefault();
                if (debitAccountItem is null)
                {
                    var voucher = await _unitRepository.GetByIdAsync(item.Item1);
                    debitAccountItem = AccountItem.Create(debtorAccount, 0, voucher);
                }
                transaction.AddTransactionItem(HolderTransactionItem.Create(UnitQuantity.Create(item.Item2, debitAccountItem.Unit), creditAccountItem, debitAccountItem));
            }
    
            if (holderTransactionRequest is null)
            {
                transaction.Perform();
                await _holderTransactionRepository.AddAsync(transaction);
                return transaction.Id;
            }

            holderTransactionRequest.Perform(transaction);
            await _holderTransactionRequestRepository.UpdateAsync(holderTransactionRequest);
            return transaction.Id;
        }
    }
}