using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Core.Application.Errors;
using Vouchers.Core.Domain;
using Unit = Vouchers.Core.Domain.Unit;
using RUnit = Vouchers.Common.Application.Abstractions.Unit;

namespace Vouchers.Core.Application.UseCases.UnitCases;

internal sealed class UpdateVoucherCommandHandler : IRequestHandler<UpdateUnitCommand, RUnit>
{
    private readonly IIdentityIdProvider _identityIdProvider;
    private readonly IReadOnlyRepository<UnitType,Guid> _unitTypeRepository;
    private readonly IRepository<Unit,Guid> _unitRepository;

    public UpdateVoucherCommandHandler(IIdentityIdProvider identityIdProvider, 
        IReadOnlyRepository<UnitType,Guid> unitTypeRepository, IRepository<Unit,Guid> unitRepository)
    {
        _identityIdProvider = identityIdProvider;
        _unitTypeRepository = unitTypeRepository;
        _unitRepository = unitRepository;
    }

    public async Task<Result<RUnit>> HandleAsync(UpdateUnitCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.CurrentIdentityId;

        var unitType = await _unitTypeRepository.GetByIdAsync(command.UnitTypeId);
        if (unitType is null)
            return new UnitTypeDoesNotExistError();

        if (unitType.IssuerAccount.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        var unit = await _unitRepository.GetByIdAsync(command.Id);
        if (unit is null)
            return new UnitDoesNotExistError();

        if (unit.UnitType.Id != command.UnitTypeId)
            return new OperationIsNotAllowedError();

        var requireUpdate = false;

        if (command.ValidFrom is not null && command.ValidFrom != unit.ValidFrom)
        {
            unit.SetValidFrom(command.ValidFrom.Value);
            requireUpdate = true;
        }

        if (command.ValidTo is not null && command.ValidTo != unit.ValidTo)
        {
            unit.SetValidTo(command.ValidTo.Value);
            requireUpdate = true;
        }
        if (command.CanBeExchanged is not null && command.CanBeExchanged != unit.CanBeExchanged)
        {
            unit.SetCanBeExchanged(command.CanBeExchanged.Value);
            requireUpdate = true;
        }

        if(requireUpdate)
            await _unitRepository.UpdateAsync(unit);
        
        return RUnit.Value;
    }
}