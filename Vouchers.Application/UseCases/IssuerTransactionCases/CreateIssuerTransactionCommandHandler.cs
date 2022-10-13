using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Core;
using Vouchers.Application.Infrastructure;
using Vouchers.Domains;
using Vouchers.Application.Commands.IssuerTransactionCommands;

namespace Vouchers.Application.UseCases.IssuerTransactionCases
{
    internal sealed class CreateIssuerTransactionCommandHandler : IAuthIdentityHandler<CreateIssuerTransactionCommand, Guid>
    {
        private readonly IRepository<DomainAccount> _domainAccountRepository;
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<AccountItem> _accountItemRepository;
        private readonly IRepository<Unit> _unitRepository;
        private readonly IRepository<IssuerTransaction> _issuerTransactionRepository;


        public CreateIssuerTransactionCommandHandler(IRepository<DomainAccount> domainAccountRepository, IRepository<Account> accountRepository, IRepository<AccountItem> accountItemRepository, IRepository<Unit> unitRepository, IRepository<IssuerTransaction> issuerTransactionRepository) 
        {
            _domainAccountRepository = domainAccountRepository;
            _accountRepository = accountRepository;
            _accountItemRepository = accountItemRepository;
            _unitRepository = unitRepository;
            _issuerTransactionRepository = issuerTransactionRepository;
        }

        public async Task<Guid> HandleAsync(CreateIssuerTransactionCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var issuerDomainAccount = await _domainAccountRepository.GetByIdAsync(command.IssuerAccountId);
            if (issuerDomainAccount?.IdentityId != authIdentityId)
                throw new ApplicationException("Operation is not allowed");
            if (!issuerDomainAccount.IsConfirmed)
                throw new ApplicationException("Issuer account is not activated");

            var issuerAccount = await _accountRepository.GetByIdAsync(command.IssuerAccountId);
            if (issuerAccount is null)
                throw new ApplicationException("Issuer's account does not exist");

            var accountItem = (await _accountItemRepository.GetByExpressionAsync(accItem => accItem.HolderAccountId == issuerAccount.Id && accItem.Unit.Id == command.VoucherId)).FirstOrDefault();
            if (accountItem is null)
            {
                if (command.Quantity > 0)
                {
                    var unit = await _unitRepository.GetByIdAsync(command.VoucherId);
                    accountItem = AccountItem.Create(issuerAccount, 0, unit);
                }
                else
                    throw new ApplicationException($"Issuer {issuerDomainAccount.Id} does not have account item for unit {command.VoucherId}");
            }


            IssuerTransaction transaction = IssuerTransaction.Create(accountItem, command.Quantity);
            transaction.Perform();

            await _issuerTransactionRepository.AddAsync(transaction);

            return transaction.Id;
        }
    }
}