using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Core.Application.Errors;
using Vouchers.Core.Domain;
using Unit = Vouchers.Common.Application.Abstractions.Unit;

namespace Vouchers.Core.Application.UseCases.UnitTypeCases;

public class DeleteUnitTypeCommandHandler : IRequestHandler<DeleteUnitTypeCommand, Unit>
{
    private readonly IReadOnlyRepository<Account, Guid> _accountRepository;
    private readonly IRepository<UnitType, Guid> _unitTypeRepository;
    private readonly IIdentityIdProvider _identityIdProvider;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    
    public DeleteUnitTypeCommandHandler(IReadOnlyRepository<Account, Guid> accountRepository, IRepository<UnitType, Guid> unitTypeRepository, IIdentityIdProvider identityIdProvider, IDomainEventDispatcher domainEventDispatcher)
    {
        _accountRepository = accountRepository;
        _unitTypeRepository = unitTypeRepository;
        _identityIdProvider = identityIdProvider;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<Result<Unit>> HandleAsync(DeleteUnitTypeCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.CurrentIdentityId;

        var unitType = await _unitTypeRepository.GetByIdAsync(command.UnitTypeId);
        if (unitType is null)
            return new UnitTypeDoesNotExistError();

        if (unitType.IssuerAccount.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        if (unitType.CanBeRemoved())
        {
            await _unitTypeRepository.RemoveAsync(unitType);
        }

        var unitTypeDeleted = new UnitTypeDeletedDomainEvent(unitType.Id);
        var result = await _domainEventDispatcher.DispatchAsync(unitTypeDeleted);
        if (result.IsFailure)
            return result.Errors;
            
        return Unit.Value;
    }
}