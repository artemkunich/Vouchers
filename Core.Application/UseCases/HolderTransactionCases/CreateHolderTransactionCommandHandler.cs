using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identifier;
using Akunich.Extensions.Identity.Abstractions;
using Akunich.Extensions.Time;
using Vouchers.Core.Application.Dtos;
using Vouchers.Core.Application.Errors;
using Vouchers.Core.Domain;
using Unit = Vouchers.Core.Domain.Unit;

namespace Vouchers.Core.Application.UseCases.HolderTransactionCases;

internal sealed class CreateHolderTransactionCommandHandler : IRequestHandler<CreateHolderTransactionCommand, IdDto<Guid>>
{
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;
    private readonly IReadOnlyRepository<Account,Guid> _accountRepository;
    private readonly IReadOnlyRepository<Unit,Guid> _unitRepository;
    private readonly IReadOnlyRepository<UnitType,Guid> _unitTypeRepository;
    private readonly IReadOnlyRepository<AccountItem,Guid> _accountItemRepository;
    private readonly IRepository<HolderTransaction,Guid> _holderTransactionRepository;
    private readonly IRepository<HolderTransactionRequest,Guid> _holderTransactionRequestRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly ITimeProvider _timeProvider;

    public CreateHolderTransactionCommandHandler(
        IIdentityIdProvider<Guid> identityIdProvider, 
        IReadOnlyRepository<Account,Guid> accountRepository, 
        IReadOnlyRepository<Unit,Guid> unitRepository, 
        IReadOnlyRepository<UnitType,Guid> unitTypeRepository, 
        IReadOnlyRepository<AccountItem,Guid> accountItemRepository, 
        IRepository<HolderTransaction,Guid> holderTransactionRepository, 
        IRepository<HolderTransactionRequest,Guid> holderTransactionRequestRepository, 
        IIdentifierProvider<Guid> identifierProvider, 
        ITimeProvider timeProvider)
    {
        _identityIdProvider = identityIdProvider;
        _accountRepository = accountRepository;
        _unitRepository = unitRepository;
        _unitTypeRepository = unitTypeRepository;
        _accountItemRepository = accountItemRepository;
        _holderTransactionRepository = holderTransactionRepository;
        _holderTransactionRequestRepository = holderTransactionRequestRepository;
        _identifierProvider = identifierProvider;
        _timeProvider = timeProvider;
    }

    public async Task<Result<IdDto<Guid>>> HandleAsync(CreateHolderTransactionCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.GetIdentityId();

        var creditorAccount = await _accountRepository.GetByIdAsync(command.CreditorAccountId, cancellation);
        if (creditorAccount?.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();
        if (!creditorAccount.IsActive)
            return new CreditorAccountIsNotActivatedError();

        HolderTransactionRequest holderTransactionRequest = null;
        if (command.HolderTransactionRequestId is not null)
        {
            holderTransactionRequest = await _holderTransactionRequestRepository.GetByIdAsync(command.HolderTransactionRequestId.Value, cancellation);
            if(holderTransactionRequest is null)
                return new TransactionRequestIsNotFoundError();
        }

        var debtorAccount = await _accountRepository.GetByIdAsync(command.DebtorAccountId, cancellation);
        if(debtorAccount is null)
            return new DebtorAccountDoesNotExistError();
        if (!debtorAccount.IsActive)
            return new DebtorAccountIsNotActivatedError();

        var unitType = await _unitTypeRepository.GetByIdAsync(command.UnitTypeId, cancellation);
        if (unitType is null)
            return new UnitTypeDoesNotExistError();
        

        var transactionId = _identifierProvider.CreateNewId();
        var currentDateTime = _timeProvider.GetLocalNow();
        HolderTransaction transaction = HolderTransaction.Create(transactionId, currentDateTime, creditorAccount, debtorAccount, unitType, command.Message);

        foreach (var item in command.Items)
        {
            var creditAccountItem = (await _accountItemRepository.GetByExpressionAsync(
                accItem => accItem.HolderAccountId == command.CreditorAccountId && accItem.UnitId == item.Item1,
                cancellation)).FirstOrDefault();
            if (creditAccountItem is null)
            {
                return new IdentityDoesNotHaveAccountForUnitError();
            }
            var debitAccountItem = (await _accountItemRepository.GetByExpressionAsync(
                accItem => accItem.HolderAccountId == command.DebtorAccountId && accItem.UnitId == item.Item1,
                cancellation)).FirstOrDefault();
            if (debitAccountItem is null)
            {
                var voucherId = _identifierProvider.CreateNewId();
                var voucher = await _unitRepository.GetByIdAsync(item.Item1, cancellation);
                debitAccountItem = AccountItem.Create(voucherId, debtorAccount, voucher);
            }

            var transactionItemId = _identifierProvider.CreateNewId();
            HolderTransactionItem.Create(transactionItemId, item.Item2, creditAccountItem, debitAccountItem, transaction);
        }
    
        if (holderTransactionRequest is null)
        {
            transaction.Perform();
            await _holderTransactionRepository.AddAsync(transaction, cancellation);
            return new IdDto<Guid>(transaction.Id);
        }

        holderTransactionRequest.Perform(transaction);
        await _holderTransactionRequestRepository.UpdateAsync(holderTransactionRequest, cancellation);
        return new IdDto<Guid>(transaction.Id);
    }
}