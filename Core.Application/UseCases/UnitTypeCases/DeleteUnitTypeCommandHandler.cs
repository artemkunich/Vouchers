using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identity.Abstractions;
using Vouchers.Core.Application.Errors;
using Vouchers.Core.Domain;
using Unit = Akunich.Application.Abstractions.Unit;

namespace Vouchers.Core.Application.UseCases.UnitTypeCases;

public class DeleteUnitTypeCommandHandler : IRequestHandler<DeleteUnitTypeCommand, Unit>
{
    private readonly IReadOnlyRepository<Account, Guid> _accountRepository;
    private readonly IRepository<UnitType, Guid> _unitTypeRepository;
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;
    private readonly INotificationDispatcher _notificationDispatcher;
    
    public DeleteUnitTypeCommandHandler(
        IReadOnlyRepository<Account, Guid> accountRepository, 
        IRepository<UnitType, Guid> unitTypeRepository, 
        IIdentityIdProvider<Guid> identityIdProvider, 
        INotificationDispatcher notificationDispatcher)
    {
        _accountRepository = accountRepository;
        _unitTypeRepository = unitTypeRepository;
        _identityIdProvider = identityIdProvider;
        _notificationDispatcher = notificationDispatcher;
    }

    public async Task<Result<Unit>> HandleAsync(DeleteUnitTypeCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.GetIdentityId();

        var unitType = await _unitTypeRepository.GetByIdAsync(command.UnitTypeId, cancellation);
        if (unitType is null)
            return new UnitTypeDoesNotExistError();

        if (unitType.IssuerAccount.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        if (unitType.CanBeRemoved())
        {
            await _unitTypeRepository.RemoveAsync(unitType, cancellation);
        }

        var unitTypeDeleted = new UnitTypeDeletedNotification(unitType.Id);
        var result = await _notificationDispatcher.DispatchAsync(unitTypeDeleted);
        if (result.IsFailure)
            return result.Errors;
            
        return Unit.Value;
    }
}