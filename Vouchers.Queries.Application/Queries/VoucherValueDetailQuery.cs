using System;
using System.ComponentModel.DataAnnotations;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Queries;

public sealed class VoucherValueDetailQuery : IRequest<VoucherValueDetailDto>
{
    [Required]
    public Guid Id { get; init; }
}