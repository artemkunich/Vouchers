using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Core.Domain;
using Vouchers.Application.Infrastructure;
using Vouchers.Domains.Domain;
using Vouchers.Application.Commands.IssuerTransactionCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Services;

namespace Vouchers.Application.UseCases.IssuerTransactionCases;

internal sealed class CreateIssuerTransactionCommandHandler : IHandler<CreateIssuerTransactionCommand, IdDto<Guid>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IReadOnlyRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IReadOnlyRepository<Account,Guid> _accountRepository;
    private readonly IReadOnlyRepository<AccountItem,Guid> _accountItemRepository;
    private readonly IReadOnlyRepository<Unit,Guid> _unitRepository;
    private readonly IRepository<IssuerTransaction,Guid> _issuerTransactionRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICultureInfoProvider _cultureInfoProvider;
    public CreateIssuerTransactionCommandHandler(IAuthIdentityProvider authIdentityProvider,
        IReadOnlyRepository<DomainAccount,Guid> domainAccountRepository, IReadOnlyRepository<Account,Guid> accountRepository, 
        IReadOnlyRepository<AccountItem,Guid> accountItemRepository, IReadOnlyRepository<Unit,Guid> unitRepository, 
        IRepository<IssuerTransaction,Guid> issuerTransactionRepository, IIdentifierProvider<Guid> identifierProvider, IDateTimeProvider dateTimeProvider, ICultureInfoProvider cultureInfoProvider) 
    {
        _authIdentityProvider = authIdentityProvider;
        _domainAccountRepository = domainAccountRepository;
        _accountRepository = accountRepository;
        _accountItemRepository = accountItemRepository;
        _unitRepository = unitRepository;
        _issuerTransactionRepository = issuerTransactionRepository;
        _identifierProvider = identifierProvider;
        _dateTimeProvider = dateTimeProvider;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task<Result<IdDto<Guid>>> HandleAsync(CreateIssuerTransactionCommand command, CancellationToken cancellation)
    {
        var cultureInfo = _cultureInfoProvider.GetCultureInfo();
        
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        if (authIdentityId is null)
            return Error.NotRegistered(cultureInfo);

        var issuerDomainAccount = await _domainAccountRepository.GetByIdAsync(command.IssuerAccountId);
        if (issuerDomainAccount?.IdentityId != authIdentityId)
            return Error.OperationIsNotAllowed(cultureInfo);
        
        if (!issuerDomainAccount.IsConfirmed)
            return Error.IssuerAccountIsNotActivated(cultureInfo);

        var issuerAccount = await _accountRepository.GetByIdAsync(command.IssuerAccountId);
        if (issuerAccount is null)
            return Error.IssuerAccountDoesNotExist(cultureInfo);

        var accountItem = (await _accountItemRepository.GetByExpressionAsync(accItem => accItem.HolderAccountId == issuerAccount.Id && accItem.Unit.Id == command.VoucherId)).FirstOrDefault();
        if (accountItem is null)
        {
            if (command.Quantity > 0)
            {
                var unit = await _unitRepository.GetByIdAsync(command.VoucherId);
                var accountItemId = _identifierProvider.CreateNewId();
                accountItem = AccountItem.Create(accountItemId, issuerAccount, 0, unit);
            }
            else
                throw new ApplicationException(Properties.Resources.IssuerDoesNotHaveAccountItemForUnit, issuerDomainAccount.Id, command.VoucherId);
        }

        var transactionId = _identifierProvider.CreateNewId();
        IssuerTransaction transaction = IssuerTransaction.Create(transactionId, _dateTimeProvider.CurrentDateTime(), accountItem, command.Quantity);
        transaction.Perform();

        await _issuerTransactionRepository.AddAsync(transaction);

        return new IdDto<Guid>(transaction.Id);
    }
}