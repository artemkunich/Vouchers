using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Core.Application.Dtos;
using Vouchers.Core.Application.Errors;
using Vouchers.Core.Domain;
using OperationIsNotAllowedError = Vouchers.Core.Application.Errors.OperationIsNotAllowedError;
using Unit = Vouchers.Core.Domain.Unit;

namespace Vouchers.Core.Application.UseCases.HolderTransactionCases;

internal sealed class CreateHolderTransactionCommandHandler : IRequestHandler<CreateHolderTransactionCommand, IdDto<Guid>>
{
    private readonly IIdentityIdProvider _identityIdProvider;
    private readonly IReadOnlyRepository<Account,Guid> _accountRepository;
    private readonly IReadOnlyRepository<Unit,Guid> _unitRepository;
    private readonly IReadOnlyRepository<UnitType,Guid> _unitTypeRepository;
    private readonly IReadOnlyRepository<AccountItem,Guid> _accountItemRepository;
    private readonly IRepository<HolderTransaction,Guid> _holderTransactionRepository;
    private readonly IRepository<HolderTransactionRequest,Guid> _holderTransactionRequestRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateHolderTransactionCommandHandler(IIdentityIdProvider identityIdProvider, 
        IReadOnlyRepository<Account,Guid> accountRepository, IReadOnlyRepository<Unit,Guid> unitRepository, 
        IReadOnlyRepository<UnitType,Guid> unitTypeRepository, IReadOnlyRepository<AccountItem,Guid> accountItemRepository, 
        IRepository<HolderTransaction,Guid> holderTransactionRepository, IRepository<HolderTransactionRequest,Guid> holderTransactionRequestRepository, IIdentifierProvider<Guid> identifierProvider, IDateTimeProvider dateTimeProvider)
    {
        _identityIdProvider = identityIdProvider;
        _accountRepository = accountRepository;
        _unitRepository = unitRepository;
        _unitTypeRepository = unitTypeRepository;
        _accountItemRepository = accountItemRepository;
        _holderTransactionRepository = holderTransactionRepository;
        _holderTransactionRequestRepository = holderTransactionRequestRepository;
        _identifierProvider = identifierProvider;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<IdDto<Guid>>> HandleAsync(CreateHolderTransactionCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.CurrentIdentityId;

        var creditorAccount = await _accountRepository.GetByIdAsync(command.CreditorAccountId);
        if (creditorAccount?.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();
        if (!creditorAccount.IsActive)
            return new CreditorAccountIsNotActivatedError();

        HolderTransactionRequest holderTransactionRequest = null;
        if (command.HolderTransactionRequestId is not null)
        {
            holderTransactionRequest = await _holderTransactionRequestRepository.GetByIdAsync(command.HolderTransactionRequestId.Value);
            if(holderTransactionRequest is null)
                return new TransactionRequestIsNotFoundError();
        }

        var debtorAccount = await _accountRepository.GetByIdAsync(command.DebtorAccountId);
        if(debtorAccount is null)
            return new DebtorAccountDoesNotExistError();
        if (!debtorAccount.IsActive)
            return new DebtorAccountIsNotActivatedError();

        var unitType = await _unitTypeRepository.GetByIdAsync(command.UnitTypeId);
        if (unitType is null)
            return new UnitTypeDoesNotExistError();
        

        var transactionId = _identifierProvider.CreateNewId();
        var currentDateTime = _dateTimeProvider.CurrentDateTime();
        HolderTransaction transaction = HolderTransaction.Create(transactionId, currentDateTime, creditorAccount, debtorAccount, unitType, command.Message);

        foreach (var item in command.Items)
        {
            var creditAccountItem = (await _accountItemRepository.GetByExpressionAsync(accItem => accItem.HolderAccountId == command.CreditorAccountId && accItem.UnitId == item.Item1)).FirstOrDefault();
            if (creditAccountItem is null)
            {
                return new IdentityDoesNotHaveAccountForUnitError();
            }
            var debitAccountItem = (await _accountItemRepository.GetByExpressionAsync(accItem => accItem.HolderAccountId == command.DebtorAccountId && accItem.UnitId == item.Item1)).FirstOrDefault();
            if (debitAccountItem is null)
            {
                var voucherId = _identifierProvider.CreateNewId();
                var voucher = await _unitRepository.GetByIdAsync(item.Item1);
                debitAccountItem = AccountItem.Create(voucherId, debtorAccount, voucher);
            }

            var transactionItemId = _identifierProvider.CreateNewId();
            HolderTransactionItem.Create(transactionItemId, item.Item2, creditAccountItem, debitAccountItem, transaction);
        }
    
        if (holderTransactionRequest is null)
        {
            transaction.Perform();
            await _holderTransactionRepository.AddAsync(transaction);
            return new IdDto<Guid>(transaction.Id);
        }

        holderTransactionRequest.Perform(transaction);
        await _holderTransactionRequestRepository.UpdateAsync(holderTransactionRequest);
        return new IdDto<Guid>(transaction.Id);
    }
}