using System;
using System.ComponentModel.DataAnnotations;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Queries;

public sealed class IdentityDetailQuery : IRequest<IdentityDetailDto>
{
    public Guid? AccountId { get; init; }
}