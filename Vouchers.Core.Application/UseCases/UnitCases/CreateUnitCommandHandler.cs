using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Core.Application.Dtos;
using Vouchers.Core.Application.Errors;
using Vouchers.Core.Domain;
using OperationIsNotAllowedError = Vouchers.Core.Application.Errors.OperationIsNotAllowedError;
using Unit = Vouchers.Core.Domain.Unit;

namespace Vouchers.Core.Application.UseCases.UnitCases;

internal sealed class CreateUnitCommandHandler : IRequestHandler<CreateUnitCommand, IdDto<Guid>>
{
    private readonly IIdentityIdProvider _identityIdProvider;
    private readonly IReadOnlyRepository<UnitType,Guid> _unitTypeRepository;
    private readonly IRepository<Unit,Guid> _unitRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public CreateUnitCommandHandler(IIdentityIdProvider identityIdProvider,
        IReadOnlyRepository<UnitType,Guid> unitTypeRepository, IRepository<Unit,Guid> unitRepository, IIdentifierProvider<Guid> identifierProvider, IDateTimeProvider dateTimeProvider)
    {
        _identityIdProvider = identityIdProvider;
        _unitTypeRepository = unitTypeRepository;
        _unitRepository = unitRepository;
        _identifierProvider = identifierProvider;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<IdDto<Guid>>> HandleAsync(CreateUnitCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.CurrentIdentityId;

        var unitType = await _unitTypeRepository.GetByIdAsync(command.UnitTypeId);

        if (unitType is null)
            return new UnitTypeDoesNotExistError();

        if (unitType.IssuerAccount.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        var currentDateTime = _dateTimeProvider.CurrentDateTime();
        var unitId = _identifierProvider.CreateNewId();
        var unit = Unit.Create(unitId, command.ValidFrom, command.ValidTo ?? DateTime.MaxValue, currentDateTime, command.CanBeExchanged, unitType);

        await _unitRepository.AddAsync(unit);

        return new IdDto<Guid>(unit.Id);
    }
}