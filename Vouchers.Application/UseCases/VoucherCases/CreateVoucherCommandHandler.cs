using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core.Domain;
using Vouchers.Application.Commands.VoucherCommands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Values.Domain;
using System.Linq;
using Vouchers.Application.Dtos;
using Vouchers.Application.Services;

namespace Vouchers.Application.UseCases.VoucherCases;

internal sealed class CreateVoucherCommandHandler : IHandler<CreateVoucherCommand, Result<IdDto<Guid>>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IReadOnlyRepository<VoucherValue,Guid> _voucherValueRepository;
    private readonly IReadOnlyRepository<UnitType,Guid> _unitTypeRepository;
    private readonly IRepository<Unit,Guid> _unitRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly ICultureInfoProvider _cultureInfoProvider;
    public CreateVoucherCommandHandler(IAuthIdentityProvider authIdentityProvider, IReadOnlyRepository<VoucherValue,Guid> voucherValueRepository, 
        IReadOnlyRepository<UnitType,Guid> unitTypeRepository, IRepository<Unit,Guid> unitRepository, IIdentifierProvider<Guid> identifierProvider, ICultureInfoProvider cultureInfoProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _voucherValueRepository = voucherValueRepository;
        _unitTypeRepository = unitTypeRepository;
        _unitRepository = unitRepository;
        _identifierProvider = identifierProvider;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task<Result<IdDto<Guid>>> HandleAsync(CreateVoucherCommand command, CancellationToken cancellation)
    {
        var cultureInfo = _cultureInfoProvider.GetCultureInfo();
        
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        if (authIdentityId is null)
            return Error.NotRegistered(cultureInfo);

        var value = await _voucherValueRepository.GetByIdAsync(command.VoucherValueId);

        if (value is null)
            return Error.UnitTypeDoesNotExist(cultureInfo);

        if (value.IssuerIdentityId != authIdentityId)
            return Error.OperationIsNotAllowed(cultureInfo);

        var unitType = await _unitTypeRepository.GetByIdAsync(command.VoucherValueId);

        var unitId = _identifierProvider.CreateNewId();
        var unit = Unit.Create(unitId, command.ValidFrom, command.ValidTo ?? DateTime.MaxValue, command.CanBeExchanged, unitType, 0);

        await _unitRepository.AddAsync(unit);

        return new IdDto<Guid>(unit.Id);
    }
}