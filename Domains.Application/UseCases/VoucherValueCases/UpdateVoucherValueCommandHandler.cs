using System;
using System.Threading.Tasks;
using System.Threading;
using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identity.Abstractions;
using Vouchers.Domains.Application.Errors;
using Vouchers.Domains.Domain;

namespace Vouchers.Domains.Application.UseCases.VoucherValueCases;

internal sealed class UpdateVoucherValueCommandHandler : IRequestHandler<UpdateVoucherValueCommand,Unit>
{
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;
    private readonly IRepository<VoucherValue,Guid> _voucherValueRepository;

    public UpdateVoucherValueCommandHandler(
        IIdentityIdProvider<Guid> identityIdProvider,
        IRepository<VoucherValue,Guid> voucherValueRepository)
    {
        _identityIdProvider = identityIdProvider;
        _voucherValueRepository = voucherValueRepository;
    }

    public async Task<Result<Unit>> HandleAsync(
        UpdateVoucherValueCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.GetIdentityId();

        var value = await _voucherValueRepository.GetByIdAsync(command.Id, cancellation);
        if (value is null)
            return new VoucherValueDoesNotExistError();

        if (value.IssuerIdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        var requireUpdate = false;

        if (command.Ticker is not null && value.Ticker != command.Ticker)
        {
            value.Ticker = command.Ticker;
            requireUpdate = true;
        }

        if (value.Description != command.Description)
        { 
            value.Description = command.Description;
            requireUpdate = true;
        }

        if (requireUpdate)
            await _voucherValueRepository.UpdateAsync(value, cancellation);
        
        return Unit.Value;
    }

}