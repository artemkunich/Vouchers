using System;
using Vouchers.Core.Domain;
using Vouchers.Application.Commands.VoucherValueCommands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Errors;
using Vouchers.Values.Domain;
using Vouchers.Application.Services;
using Unit = Vouchers.Application.Abstractions.Unit;

namespace Vouchers.Application.UseCases.VoucherValueCases;

internal sealed class DeleteVoucherValueCommandHandler : IHandler<DeleteVoucherValueCommand,Unit>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IRepository<VoucherValue,Guid> _valueRepository;
    private readonly IRepository<UnitType,Guid> _unitTypeRepository;

    public DeleteVoucherValueCommandHandler(IAuthIdentityProvider authIdentityProvider, 
        IRepository<VoucherValue,Guid> valueRepository, IRepository<UnitType,Guid> unitTypeRepository)
    {
        _authIdentityProvider = authIdentityProvider;
        _valueRepository = valueRepository;
        _unitTypeRepository = unitTypeRepository;
    }

    public async Task<Result<Unit>> HandleAsync(DeleteVoucherValueCommand command, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var value = await _valueRepository.GetByIdAsync(command.VoucherValueId);
        if (value is null)
            return new VoucherValueDoesNotExistError();

        if (value.IssuerIdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        var unitType = await _unitTypeRepository.GetByIdAsync(command.VoucherValueId);
        if (unitType is null)
            return new UnitTypeDoesNotExistError();

        if (unitType.CanBeRemoved())
        {
            await _unitTypeRepository.RemoveAsync(unitType);
            await _valueRepository.RemoveAsync(value);
        }
        
        return Unit.Value;
    }
}