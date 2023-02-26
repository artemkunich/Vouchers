using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Application.Commands.VoucherCommands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Errors;
using Vouchers.Values.Domain;
using Vouchers.Application.Services;
using Unit = Vouchers.Core.Domain.Unit;

namespace Vouchers.Application.UseCases.VoucherCases;

internal sealed class UpdateVoucherCommandHandler : IHandler<UpdateVoucherCommand, Abstractions.Unit>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IReadOnlyRepository<VoucherValue,Guid> _voucherValueRepository;
    private readonly IRepository<Unit,Guid> _unitRepository;

    public UpdateVoucherCommandHandler(IAuthIdentityProvider authIdentityProvider, 
        IReadOnlyRepository<VoucherValue,Guid> voucherValueRepository, IRepository<Unit,Guid> unitRepository)
    {
        _authIdentityProvider = authIdentityProvider;
        _voucherValueRepository = voucherValueRepository;
        _unitRepository = unitRepository;
    }

    public async Task<Result<Abstractions.Unit>> HandleAsync(UpdateVoucherCommand command, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var value = await _voucherValueRepository.GetByIdAsync(command.VoucherValueId);
        if (value is null)
            return new VoucherValueDoesNotExistError();

        if (value.IssuerIdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        var unit = await _unitRepository.GetByIdAsync(command.Id);
        if (unit is null)
            return new VoucherDoesNotExistError();

        if (unit.UnitType.Id != command.VoucherValueId)
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
        
        return Abstractions.Unit.Value;
    }
}