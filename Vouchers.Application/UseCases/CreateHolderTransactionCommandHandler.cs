using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Core;
using Vouchers.Application.Commands;
using Vouchers.Application.Infrastructure;
using Vouchers.Domains;

namespace Vouchers.Application.UseCases
{
    public class CreateHolderTransactionCommandHandler : IAuthIdentityHandler<CreateHolderTransactionCommand, Guid>
    {
        private readonly IRepository<DomainAccount> _domainAccountRepository;
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<Unit> _unitRepository;
        private readonly IRepository<UnitType> _unitTypeRepository;
        private readonly IRepository<AccountItem> _accountItemRepository;
        private readonly IRepository<HolderTransaction> _holderTransactionRepository;

        public CreateHolderTransactionCommandHandler(IRepository<DomainAccount> domainAccountRepository, IRepository<Account> accountRepository, IRepository<Unit> unitRepository, IRepository<UnitType> unitTypeRepository, IRepository<AccountItem> accountItemRepository, IRepository<HolderTransaction> holderTransactionRepository)
        {
            _domainAccountRepository = domainAccountRepository;
            _accountRepository = accountRepository;
            _unitRepository = unitRepository;
            _unitTypeRepository = unitTypeRepository;
            _accountItemRepository = accountItemRepository;
            _holderTransactionRepository = holderTransactionRepository;
        }

        public async Task<Guid> HandleAsync(CreateHolderTransactionCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var creditorDomainAccount = await _domainAccountRepository.GetByIdAsync(command.CreditorDomainAccountId);
            if (creditorDomainAccount?.IdentityId != authIdentityId)
                throw new ApplicationException("Operation is not allowed");
            if (!creditorDomainAccount.IsConfirmed)
                throw new ApplicationException("Creditor account is not activated");

            var debtorDomainAccount = await _domainAccountRepository.GetByIdAsync(command.DebtorDomainAccountId);
            if (!debtorDomainAccount.IsConfirmed)
                throw new ApplicationException("Debtor account is not activated");

            var unitType = await _unitTypeRepository.GetByIdAsync(command.VoucherValueId);
            if (unitType is null)
                throw new ApplicationException("Voucher value does not exist");

            var creditorAccount = await _accountRepository.GetByIdAsync(command.CreditorDomainAccountId);
            if(creditorAccount is null)
                throw new ApplicationException("Creditor's account does not exist");

            var debtorAccount = await _accountRepository.GetByIdAsync(command.DebtorDomainAccountId);
            if (debtorAccount is null)
                throw new ApplicationException("Debtor's account does not exist");

            HolderTransaction transaction = HolderTransaction.Create(creditorAccount, debtorAccount, unitType);

            foreach (var item in command.Items)
            {
                var creditAccountItem = (await _accountItemRepository.GetByExpressionAsync(accItem => accItem.HolderId == command.CreditorDomainAccountId && accItem.Unit.Id == item.Item1)).FirstOrDefault();
                if (creditAccountItem is null)
                {
                    throw new ApplicationException($"User {command.CreditorDomainAccountId} does not have account for voucher {item.Item2}");
                }
                var debitAccountItem = (await _accountItemRepository.GetByExpressionAsync(accItem => accItem.HolderId == command.DebtorDomainAccountId && accItem.Unit.Id == item.Item1)).FirstOrDefault();
                if (debitAccountItem is null)
                {
                    var voucher = await _unitRepository.GetByIdAsync(item.Item1);
                    debitAccountItem = AccountItem.Create(debtorAccount, 0, voucher);
                }
                transaction.AddTransactionItem(HolderTransactionItem.Create(UnitQuantity.Create(item.Item2, debitAccountItem.Unit), creditAccountItem, debitAccountItem));
            }
    
            transaction.Perform();

            await _holderTransactionRepository.AddAsync(transaction);

            return transaction.Id;
        }
    }
}