using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Core.Application.Dtos;
using Vouchers.Core.Domain;
using OperationIsNotAllowedError = Vouchers.Core.Application.Errors.OperationIsNotAllowedError;

namespace Vouchers.Core.Application.UseCases.HolderTransactionRequestCases;

internal sealed class CreateHolderTransactionRequestCommandHandler : IRequestHandler<CreateHolderTransactionRequestCommand, IdDto<Guid>>
{
    private readonly IIdentityIdProvider _identityIdProvider;
    private readonly IReadOnlyRepository<Account,Guid> _accountRepository;
    private readonly IReadOnlyRepository<UnitType,Guid> _unitTypeRepository;
    private readonly IRepository<HolderTransactionRequest,Guid> _holderTransactionRequestRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    public CreateHolderTransactionRequestCommandHandler(IIdentityIdProvider identityIdProvider, 
        IReadOnlyRepository<Account,Guid> accountRepository, IReadOnlyRepository<UnitType,Guid> unitTypeRepository, 
        IRepository<HolderTransactionRequest,Guid> holderTransactionRequestRepository, IIdentifierProvider<Guid> identifierProvider)
    {
        _identityIdProvider = identityIdProvider;
        _accountRepository = accountRepository;
        _unitTypeRepository = unitTypeRepository;
        _holderTransactionRequestRepository = holderTransactionRequestRepository;
        _identifierProvider = identifierProvider;
    }

    public async Task<Result<IdDto<Guid>>> HandleAsync(CreateHolderTransactionRequestCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.CurrentIdentityId;
        
        var debtorAccount = await _accountRepository.GetByIdAsync(command.DebtorAccountId);
        if (debtorAccount?.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        Account creditorAccount = null;
        if (command.CreditorAccountId != null)
            creditorAccount = await _accountRepository.GetByIdAsync(command.CreditorAccountId.Value);

        var unitType = await _unitTypeRepository.GetByIdAsync(command.UnitTypeId);

        var quantity = UnitTypeQuantity.Create(command.Amount, unitType);

        var maxDurationBeforeValidityStart = command.MaxDaysBeforeValidityStart is null ? TimeSpan.Zero : TimeSpan.FromDays(command.MaxDaysBeforeValidityStart.Value);
        var minDaysBeforeValidityEnd = command.MinDaysBeforeValidityEnd is null ? TimeSpan.Zero : TimeSpan.FromDays(command.MinDaysBeforeValidityEnd.Value);

        var requestId = _identifierProvider.CreateNewId();
        var request = HolderTransactionRequest.Create(requestId, command.DueDate, creditorAccount, debtorAccount, quantity, maxDurationBeforeValidityStart, minDaysBeforeValidityEnd, command.MustBeExchangeable, command.Message);

        await _holderTransactionRequestRepository.AddAsync(request);

        return new IdDto<Guid>(request.Id);
    }
}