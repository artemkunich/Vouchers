using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Core.Application.Dtos;
using Vouchers.Core.Application.Errors;
using Vouchers.Core.Domain;
using OperationIsNotAllowedError = Vouchers.Core.Application.Errors.OperationIsNotAllowedError;
using Unit = Vouchers.Core.Domain.Unit;

namespace Vouchers.Core.Application.UseCases.IssuerTransactionCases;

internal sealed class CreateIssuerTransactionCommandHandler : IRequestHandler<CreateIssuerTransactionCommand, IdDto<Guid>>
{
    private readonly IIdentityIdProvider _identityIdProvider;
    private readonly IReadOnlyRepository<Account,Guid> _accountRepository;
    private readonly IReadOnlyRepository<AccountItem,Guid> _accountItemRepository;
    private readonly IReadOnlyRepository<Unit,Guid> _unitRepository;
    private readonly IRepository<IssuerTransaction,Guid> _issuerTransactionRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    public CreateIssuerTransactionCommandHandler(IIdentityIdProvider identityIdProvider,
        IReadOnlyRepository<Account,Guid> accountRepository, 
        IReadOnlyRepository<AccountItem,Guid> accountItemRepository, IReadOnlyRepository<Unit,Guid> unitRepository, 
        IRepository<IssuerTransaction,Guid> issuerTransactionRepository, IIdentifierProvider<Guid> identifierProvider, IDateTimeProvider dateTimeProvider) 
    {
        _identityIdProvider = identityIdProvider;
        _accountRepository = accountRepository;
        _accountItemRepository = accountItemRepository;
        _unitRepository = unitRepository;
        _issuerTransactionRepository = issuerTransactionRepository;
        _identifierProvider = identifierProvider;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<IdDto<Guid>>> HandleAsync(CreateIssuerTransactionCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.CurrentIdentityId;

        var issuerAccount = await _accountRepository.GetByIdAsync(command.IssuerAccountId);
        if (issuerAccount?.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();
        
        if (!issuerAccount.IsActive)
            return new IssuerAccountIsNotActivatedError();

        var accountItem = (await _accountItemRepository.GetByExpressionAsync(accItem => accItem.HolderAccountId == issuerAccount.Id && accItem.Unit.Id == command.VoucherId)).FirstOrDefault();
        if (accountItem is null)
        {
            if (command.Quantity > 0)
            {
                var unit = await _unitRepository.GetByIdAsync(command.VoucherId);
                var accountItemId = _identifierProvider.CreateNewId();
                accountItem = AccountItem.Create(accountItemId, issuerAccount, unit);
            }
            else
                return new IssuerDoesNotHaveAccountItemForUnitError();
        }

        var transactionId = _identifierProvider.CreateNewId();
        IssuerTransaction transaction = IssuerTransaction.Create(transactionId, _dateTimeProvider.CurrentDateTime(), accountItem, command.Quantity);
        transaction.Perform();

        await _issuerTransactionRepository.AddAsync(transaction);

        return new IdDto<Guid>(transaction.Id);
    }
}