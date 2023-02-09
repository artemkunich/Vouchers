using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core.Domain;
using Vouchers.Application.Commands.VoucherCommands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Values.Domain;
using Vouchers.Application.Services;

namespace Vouchers.Application.UseCases.VoucherCases;

internal sealed class UpdateVoucherCommandHandler : IHandler<UpdateVoucherCommand>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly ICultureInfoProvider _cultureInfoProvider;
    private readonly IReadOnlyRepository<VoucherValue,Guid> _voucherValueRepository;
    private readonly IRepository<Unit,Guid> _unitRepository;

    public UpdateVoucherCommandHandler(IAuthIdentityProvider authIdentityProvider, ICultureInfoProvider cultureInfoProvider, 
        IReadOnlyRepository<VoucherValue,Guid> voucherValueRepository, IRepository<Unit,Guid> unitRepository)
    {
        _authIdentityProvider = authIdentityProvider;
        _cultureInfoProvider = cultureInfoProvider;
        _voucherValueRepository = voucherValueRepository;
        _unitRepository = unitRepository;
    }

    public async Task<Result> HandleAsync(UpdateVoucherCommand command, CancellationToken cancellation)
    {
        var cultureInfo = _cultureInfoProvider.GetCultureInfo();
        
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        if (authIdentityId is null)
            return Error.NotRegistered(cultureInfo);

        var value = await _voucherValueRepository.GetByIdAsync(command.VoucherValueId);
        if (value is null)
            return Error.VoucherValueDoesNotExist(cultureInfo);

        if (value.IssuerIdentityId != authIdentityId)
            return Error.OperationIsNotAllowed(cultureInfo);

        var unit = await _unitRepository.GetByIdAsync(command.Id);
        if (unit is null)
            return Error.VoucherDoesNotExist(cultureInfo);

        if (unit.UnitType.Id != command.VoucherValueId)
            return Error.OperationIsNotAllowed(cultureInfo);

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
            unit.SetCanBeExchanged(command.CanBeExchanged.Value, cultureInfo);
            requireUpdate = true;
        }

        if(requireUpdate)
            await _unitRepository.UpdateAsync(unit);
        
        return Result.Create();
    }
}