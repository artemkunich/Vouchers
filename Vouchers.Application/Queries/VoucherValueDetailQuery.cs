using System;
using System.ComponentModel.DataAnnotations;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Queries;

[IdentityRoles(IdentityRole.User)]
public sealed class VoucherValueDetailQuery : IRequest<VoucherValueDetailDto>
{
    [Required]
    public Guid Id { get; init; }
}