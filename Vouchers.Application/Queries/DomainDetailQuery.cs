using System;
using System.ComponentModel.DataAnnotations;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Queries;

public sealed class DomainDetailQuery : IRequest<DomainDetailDto>
{
    [Required]
    public Guid Id { get; init; }
}