using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Core.Application.Errors;
using Vouchers.Core.Domain;
using Unit = Vouchers.Core.Domain.Unit;
using RUnit = Vouchers.Common.Application.Abstractions.Unit;

namespace Vouchers.Core.Application.UseCases.UnitCases;

internal sealed class DeleteUnitCommandHandler : IRequestHandler<DeleteUnitCommand, RUnit>
{
    private readonly IIdentityIdProvider _identityIdProvider;
    private readonly IReadOnlyRepository<UnitType,Guid> _unitTypeRepository;
    private readonly IRepository<Unit,Guid> _unitRepository;

    public DeleteUnitCommandHandler(IIdentityIdProvider identityIdProvider, 
        IReadOnlyRepository<UnitType,Guid> unitTypeRepository, IRepository<Unit,Guid> unitRepository)
    {
        _identityIdProvider = identityIdProvider;
        _unitTypeRepository = unitTypeRepository;
        _unitRepository = unitRepository;
    }

    public async Task<Result<RUnit>> HandleAsync(DeleteUnitCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.CurrentIdentityId;

        var unitType = await _unitTypeRepository.GetByIdAsync(command.UnitTypeId);
        if (unitType is null)
            return new UnitTypeDoesNotExistError();

        if(unitType.IssuerAccount.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        var unit = await _unitRepository.GetByIdAsync(command.UnitId);
        if (unit is null)
            return new UnitDoesNotExistError();

        if (unit.UnitType.Id != command.UnitTypeId)
            return new OperationIsNotAllowedError();

        if (unit.CanBeRemoved())
            await _unitRepository.RemoveAsync(unit);
        
        return RUnit.Value;
    }
}