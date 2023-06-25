using System;
using System.Threading.Tasks;
using System.Threading;
using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Vouchers.Domains.Domain;
using Unit = Akunich.Application.Abstractions.Unit;

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
        var value = await _valueRepository.GetByIdAsync(@event.VoucherValueId, cancellation);
        await _valueRepository.RemoveAsync(value, cancellation);

        return Unit.Value;
    }
}