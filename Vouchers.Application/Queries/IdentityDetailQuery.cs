using System;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Queries;

[Permission]
public sealed class IdentityDetailQuery : IRequest<IdentityDetailDto>
{
    public Guid? AccountId { get; init; }
}