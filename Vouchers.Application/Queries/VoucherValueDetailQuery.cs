using System;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Queries;

public sealed class VoucherValueDetailQuery
{
    [Required]
    public Guid Id { get; init; }
}