using System;
using Vouchers.Common.Application.Abstractions;

namespace Vouchers.Domains.Application.UseCases.VoucherValueCases;

public class DeleteVoucherValueCommand : IRequest<Unit>
{
    public Guid VoucherValueId { get; set; }
}
