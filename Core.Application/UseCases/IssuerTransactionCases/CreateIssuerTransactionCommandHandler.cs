using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identifier;
using Akunich.Extensions.Identity.Abstractions;
using Akunich.Extensions.Time;
using Vouchers.Core.Application.Dtos;
using Vouchers.Core.Application.Errors;
using Vouchers.Core.Domain;
using Unit = Vouchers.Core.Domain.Unit;

namespace Vouchers.Core.Application.UseCases.IssuerTransactionCases;

internal sealed class CreateIssuerTransactionCommandHandler : IRequestHandler<CreateIssuerTransactionCommand, IdDto<Guid>>
{
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;
    private readonly IReadOnlyRepository<Account,Guid> _accountRepository;
    private readonly IReadOnlyRepository<AccountItem,Guid> _accountItemRepository;
    private readonly IReadOnlyRepository<Unit,Guid> _unitRepository;
    private readonly IRepository<IssuerTransaction,Guid> _issuerTransactionRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly ITimeProvider _timeProvider;
    
    public CreateIssuerTransactionCommandHandler(
        IIdentityIdProvider<Guid> identityIdProvider,
        IReadOnlyRepository<Account,Guid> accountRepository, 
        IReadOnlyRepository<AccountItem,Guid> accountItemRepository, 
        IReadOnlyRepository<Unit,Guid> unitRepository, 
        IRepository<IssuerTransaction,Guid> issuerTransactionRepository, 
        IIdentifierProvider<Guid> identifierProvider, 
        ITimeProvider timeProvider) 
    {
        _identityIdProvider = identityIdProvider;
        _accountRepository = accountRepository;
        _accountItemRepository = accountItemRepository;
        _unitRepository = unitRepository;
        _issuerTransactionRepository = issuerTransactionRepository;
        _identifierProvider = identifierProvider;
        _timeProvider = timeProvider;
    }

    public async Task<Result<IdDto<Guid>>> HandleAsync(CreateIssuerTransactionCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.GetIdentityId();

        var issuerAccount = await _accountRepository.GetByIdAsync(command.IssuerAccountId, cancellation);
        if (issuerAccount?.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();
        
        if (!issuerAccount.IsActive)
            return new IssuerAccountIsNotActivatedError();

        var accountItem = (await _accountItemRepository
            .GetByExpressionAsync(accItem => accItem.HolderAccountId == issuerAccount.Id && accItem.Unit.Id == command.VoucherId, cancellation)).FirstOrDefault();
        if (accountItem is null)
        {
            if (command.Quantity > 0)
            {
                var unit = await _unitRepository.GetByIdAsync(command.VoucherId, cancellation);
                var accountItemId = _identifierProvider.CreateNewId();
                accountItem = AccountItem.Create(accountItemId, issuerAccount, unit);
            }
            else
                return new IssuerDoesNotHaveAccountItemForUnitError();
        }

        var transactionId = _identifierProvider.CreateNewId();
        IssuerTransaction transaction = IssuerTransaction.Create(transactionId, _timeProvider.GetUtcNow(), accountItem, command.Quantity);
        transaction.Perform();

        await _issuerTransactionRepository.AddAsync(transaction, cancellation);

        return new IdDto<Guid>(transaction.Id);
    }
}