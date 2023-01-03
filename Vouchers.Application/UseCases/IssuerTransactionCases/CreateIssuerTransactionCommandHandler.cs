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
using Vouchers.Application.Services;

namespace Vouchers.Application.UseCases.IssuerTransactionCases;

internal sealed class CreateIssuerTransactionCommandHandler : IHandler<CreateIssuerTransactionCommand, Guid>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IRepository<Account,Guid> _accountRepository;
    private readonly IRepository<AccountItem,Guid> _accountItemRepository;
    private readonly IRepository<Unit,Guid> _unitRepository;
    private readonly IRepository<IssuerTransaction,Guid> _issuerTransactionRepository;


    public CreateIssuerTransactionCommandHandler(IAuthIdentityProvider authIdentityProvider,
        IRepository<DomainAccount,Guid> domainAccountRepository, IRepository<Account,Guid> accountRepository, 
        IRepository<AccountItem,Guid> accountItemRepository, IRepository<Unit,Guid> unitRepository, 
        IRepository<IssuerTransaction,Guid> issuerTransactionRepository) 
    {
        _authIdentityProvider = authIdentityProvider;
        _domainAccountRepository = domainAccountRepository;
        _accountRepository = accountRepository;
        _accountItemRepository = accountItemRepository;
        _unitRepository = unitRepository;
        _issuerTransactionRepository = issuerTransactionRepository;
    }

    public async Task<Guid> HandleAsync(CreateIssuerTransactionCommand command, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var issuerDomainAccount = await _domainAccountRepository.GetByIdAsync(command.IssuerAccountId);
        if (issuerDomainAccount?.IdentityId != authIdentityId)
            throw new ApplicationException(Properties.Resources.OperationIsNotAllowed);
        if (!issuerDomainAccount.IsConfirmed)
            throw new ApplicationException(Properties.Resources.IssuerAccountIsNotActivated);

        var issuerAccount = await _accountRepository.GetByIdAsync(command.IssuerAccountId);
        if (issuerAccount is null)
            throw new ApplicationException(Properties.Resources.IssuerAccountDoesNotExist);

        var accountItem = (await _accountItemRepository.GetByExpressionAsync(accItem => accItem.HolderAccountId == issuerAccount.Id && accItem.Unit.Id == command.VoucherId)).FirstOrDefault();
        if (accountItem is null)
        {
            if (command.Quantity > 0)
            {
                var unit = await _unitRepository.GetByIdAsync(command.VoucherId);
                accountItem = AccountItem.Create(issuerAccount, 0, unit);
            }
            else
                throw new ApplicationException(Properties.Resources.IssuerDoesNotHaveAccountItemForUnit, issuerDomainAccount.Id, command.VoucherId);
        }


        IssuerTransaction transaction = IssuerTransaction.Create(accountItem, command.Quantity);
        transaction.Perform();

        await _issuerTransactionRepository.AddAsync(transaction);

        return transaction.Id;
    }
}