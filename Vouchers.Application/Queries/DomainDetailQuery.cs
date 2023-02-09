using System;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Queries;

public sealed class DomainDetailQuery
{
    [Required]
    public Guid Id { get; init; }
}