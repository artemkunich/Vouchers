using System;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Domains.Domain;
using Unit = Vouchers.Common.Application.Abstractions.Unit;

namespace Vouchers.Domains.Application.UseCases.VoucherValueCases;

internal sealed class DeleteVoucherValueCommandHandler : IRequestHandler<DeleteVoucherValueCommand,Unit>
{
    private readonly IRepository<VoucherValue,Guid> _valueRepository;

    public DeleteVoucherValueCommandHandler(IRepository<VoucherValue,Guid> valueRepository)
    {
        _valueRepository = valueRepository;
    }

    public async Task<Result<Unit>> HandleAsync(DeleteVoucherValueCommand @event, CancellationToken cancellation)
    {
        var value = await _valueRepository.GetByIdAsync(@event.VoucherValueId);
        await _valueRepository.RemoveAsync(value);

        return Unit.Value;
    }
}