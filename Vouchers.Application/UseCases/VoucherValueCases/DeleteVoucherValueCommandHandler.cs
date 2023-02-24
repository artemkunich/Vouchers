using System;
using Vouchers.Core.Domain;
using Vouchers.Application.Commands.VoucherValueCommands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Application.Abstractions;
using Vouchers.Values.Domain;
using Vouchers.Application.Services;
using Unit = Vouchers.Application.Abstractions.Unit;

namespace Vouchers.Application.UseCases.VoucherValueCases;

internal sealed class DeleteVoucherValueCommandHandler : IHandler<DeleteVoucherValueCommand,Unit>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IRepository<VoucherValue,Guid> _valueRepository;
    private readonly IRepository<UnitType,Guid> _unitTypeRepository;
    private readonly ICultureInfoProvider _cultureInfoProvider;
    
    public DeleteVoucherValueCommandHandler(IAuthIdentityProvider authIdentityProvider, 
        IRepository<VoucherValue,Guid> valueRepository, IRepository<UnitType,Guid> unitTypeRepository, ICultureInfoProvider cultureInfoProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _valueRepository = valueRepository;
        _unitTypeRepository = unitTypeRepository;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task<Result<Unit>> HandleAsync(DeleteVoucherValueCommand command, CancellationToken cancellation)
    {
        var cultureInfo = _cultureInfoProvider.GetCultureInfo();
        
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var value = await _valueRepository.GetByIdAsync(command.VoucherValueId);
        if (value is null)
            return Error.VoucherValueDoesNotExist(cultureInfo);

        if (value.IssuerIdentityId != authIdentityId)
            return Error.OperationIsNotAllowed(cultureInfo);

        var unitType = await _unitTypeRepository.GetByIdAsync(command.VoucherValueId);
        if (unitType is null)
            return Error.UnitTypeDoesNotExist(cultureInfo);

        if (unitType.CanBeRemoved())
        {
            await _unitTypeRepository.RemoveAsync(unitType);
            await _valueRepository.RemoveAsync(value);
        }
        
        return Unit.Value;
    }
}