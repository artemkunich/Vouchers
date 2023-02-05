﻿using System;
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

internal sealed class DeleteVoucherCommandHandler : IHandler<DeleteVoucherCommand,Result>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IReadOnlyRepository<VoucherValue,Guid> _voucherValueRepository;
    private readonly IRepository<Unit,Guid> _unitRepository;
    private readonly ICultureInfoProvider _cultureInfoProvider;

    public DeleteVoucherCommandHandler(IAuthIdentityProvider authIdentityProvider, 
        IReadOnlyRepository<VoucherValue,Guid> voucherValueRepository, IRepository<Unit,Guid> unitRepository, ICultureInfoProvider cultureInfoProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _voucherValueRepository = voucherValueRepository;
        _unitRepository = unitRepository;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task<Result> HandleAsync(DeleteVoucherCommand command, CancellationToken cancellation)
    {
        var cultureInfo = _cultureInfoProvider.GetCultureInfo();
        
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        if (authIdentityId is null)
            return Error.NotAuthorized(cultureInfo);

        var value = await _voucherValueRepository.GetByIdAsync(command.VoucherValueId);
        if (value is null)
            return Error.VoucherValueDoesNotExist(cultureInfo);

        if(value.IssuerIdentityId != authIdentityId)
            return Error.OperationIsNotAllowed(cultureInfo);

        var unit = await _unitRepository.GetByIdAsync(command.VoucherId);
        if(unit is null)
            return Error.VoucherDoesNotExist(cultureInfo);

        if (unit.UnitType.Id != command.VoucherValueId)
            return Error.OperationIsNotAllowed(cultureInfo);

        if (unit.CanBeRemoved())
            await _unitRepository.RemoveAsync(unit);
        
        return Result.Create();
    }
}