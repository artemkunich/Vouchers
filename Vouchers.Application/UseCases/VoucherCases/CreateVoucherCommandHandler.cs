﻿using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core.Domain;
using Vouchers.Application.Commands.VoucherCommands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Values.Domain;
using System.Linq;
using Vouchers.Application.Services;

namespace Vouchers.Application.UseCases.VoucherCases;

internal sealed class CreateVoucherCommandHandler : IHandler<CreateVoucherCommand, Guid>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IRepository<VoucherValue,Guid> _voucherValueRepository;
    private readonly IRepository<UnitType,Guid> _unitTypeRepository;
    private readonly IRepository<Unit,Guid> _unitRepository;

    public CreateVoucherCommandHandler(IAuthIdentityProvider authIdentityProvider, IRepository<VoucherValue,Guid> voucherValueRepository, 
        IRepository<UnitType,Guid> unitTypeRepository, IRepository<Unit,Guid> unitRepository)
    {
        _authIdentityProvider = authIdentityProvider;
        _voucherValueRepository = voucherValueRepository;
        _unitTypeRepository = unitTypeRepository;
        _unitRepository = unitRepository;
    }

    public async Task<Guid> HandleAsync(CreateVoucherCommand command, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var value = await _voucherValueRepository.GetByIdAsync(command.VoucherValueId);

        if (value is null)
            throw new ApplicationException(Properties.Resources.UnitTypeDoesNotExist);

        if (value.IssuerIdentityId != authIdentityId)
            throw new ApplicationException(Properties.Resources.OperationIsNotAllowed);

        var unitType = await _unitTypeRepository.GetByIdAsync(command.VoucherValueId);

        var unit = Unit.Create(command.ValidFrom, command.ValidTo ?? DateTime.MaxValue, command.CanBeExchanged, unitType);

        await _unitRepository.AddAsync(unit);

        return unit.Id;
    }
}