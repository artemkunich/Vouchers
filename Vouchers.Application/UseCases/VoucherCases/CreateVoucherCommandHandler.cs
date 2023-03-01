using System;
using Vouchers.Core.Domain;
using Vouchers.Application.Commands.VoucherCommands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Application.Abstractions;
using Vouchers.Values.Domain;
using Vouchers.Application.Dtos;
using Vouchers.Application.Errors;
using Vouchers.Application.Services;
using Unit = Vouchers.Core.Domain.Unit;

namespace Vouchers.Application.UseCases.VoucherCases;

internal sealed class CreateVoucherCommandHandler : IRequestHandler<CreateVoucherCommand, IdDto<Guid>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IReadOnlyRepository<VoucherValue,Guid> _voucherValueRepository;
    private readonly IReadOnlyRepository<UnitType,Guid> _unitTypeRepository;
    private readonly IRepository<Unit,Guid> _unitRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public CreateVoucherCommandHandler(IAuthIdentityProvider authIdentityProvider, IReadOnlyRepository<VoucherValue,Guid> voucherValueRepository, 
        IReadOnlyRepository<UnitType,Guid> unitTypeRepository, IRepository<Unit,Guid> unitRepository, IIdentifierProvider<Guid> identifierProvider, IDateTimeProvider dateTimeProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _voucherValueRepository = voucherValueRepository;
        _unitTypeRepository = unitTypeRepository;
        _unitRepository = unitRepository;
        _identifierProvider = identifierProvider;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<IdDto<Guid>>> HandleAsync(CreateVoucherCommand command, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var value = await _voucherValueRepository.GetByIdAsync(command.VoucherValueId);

        if (value is null)
            return new UnitTypeDoesNotExistError();

        if (value.IssuerIdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        var unitType = await _unitTypeRepository.GetByIdAsync(command.VoucherValueId);

        var currentDateTime = _dateTimeProvider.CurrentDateTime();
        var unitId = _identifierProvider.CreateNewId();
        var unit = Unit.Create(unitId, command.ValidFrom, command.ValidTo ?? DateTime.MaxValue, currentDateTime, command.CanBeExchanged, unitType);

        await _unitRepository.AddAsync(unit);

        return new IdDto<Guid>(unit.Id);
    }
}