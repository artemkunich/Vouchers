using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identity.Abstractions;
using Vouchers.Core.Application.Errors;
using Vouchers.Core.Domain;
using Unit = Vouchers.Core.Domain.Unit;
using RUnit = Akunich.Application.Abstractions.Unit;

namespace Vouchers.Core.Application.UseCases.UnitCases;

internal sealed class DeleteUnitCommandHandler : IRequestHandler<DeleteUnitCommand, RUnit>
{
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;
    private readonly IReadOnlyRepository<UnitType,Guid> _unitTypeRepository;
    private readonly IRepository<Unit,Guid> _unitRepository;

    public DeleteUnitCommandHandler(
        IIdentityIdProvider<Guid> identityIdProvider, 
        IReadOnlyRepository<UnitType,Guid> unitTypeRepository, 
        IRepository<Unit,Guid> unitRepository)
    {
        _identityIdProvider = identityIdProvider;
        _unitTypeRepository = unitTypeRepository;
        _unitRepository = unitRepository;
    }

    public async Task<Result<RUnit>> HandleAsync(DeleteUnitCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.GetIdentityId();

        var unitType = await _unitTypeRepository.GetByIdAsync(command.UnitTypeId, cancellation);
        if (unitType is null)
            return new UnitTypeDoesNotExistError();

        if(unitType.IssuerAccount.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        var unit = await _unitRepository.GetByIdAsync(command.UnitId, cancellation);
        if (unit is null)
            return new UnitDoesNotExistError();

        if (unit.UnitType.Id != command.UnitTypeId)
            return new OperationIsNotAllowedError();

        if (unit.CanBeRemoved())
            await _unitRepository.RemoveAsync(unit, cancellation);
        
        return RUnit.Value;
    }
}