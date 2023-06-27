using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identifier;
using Akunich.Extensions.Identity.Abstractions;
using Akunich.Extensions.Time;
using Vouchers.Core.Application.Dtos;
using Vouchers.Core.Application.Errors;
using Vouchers.Core.Domain;
using Unit = Vouchers.Core.Domain.Unit;

namespace Vouchers.Core.Application.UseCases.UnitCases;

internal sealed class CreateUnitCommandHandler : IRequestHandler<CreateUnitCommand, IdDto<Guid>>
{
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;
    private readonly IReadOnlyRepository<UnitType,Guid> _unitTypeRepository;
    private readonly IRepository<Unit,Guid> _unitRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly ITimeProvider _timeProvider;
    
    public CreateUnitCommandHandler(
        IIdentityIdProvider<Guid> identityIdProvider,
        IReadOnlyRepository<UnitType,Guid> unitTypeRepository, 
        IRepository<Unit,Guid> unitRepository, 
        IIdentifierProvider<Guid> identifierProvider, 
        ITimeProvider timeProvider)
    {
        _identityIdProvider = identityIdProvider;
        _unitTypeRepository = unitTypeRepository;
        _unitRepository = unitRepository;
        _identifierProvider = identifierProvider;
        _timeProvider = timeProvider;
    }

    public async Task<Result<IdDto<Guid>>> HandleAsync(CreateUnitCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.GetIdentityId();

        var unitType = await _unitTypeRepository.GetByIdAsync(command.UnitTypeId, cancellation);

        if (unitType is null)
            return new UnitTypeDoesNotExistError();

        if (unitType.IssuerAccount.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        var currentDateTime = _timeProvider.GetUtcNow();
        var unitId = _identifierProvider.CreateNewId();
        var unit = Unit.Create(unitId, command.ValidFrom, command.ValidTo ?? DateTime.MaxValue, currentDateTime, command.CanBeExchanged, unitType);

        await _unitRepository.AddAsync(unit, cancellation);

        return new IdDto<Guid>(unit.Id);
    }
}