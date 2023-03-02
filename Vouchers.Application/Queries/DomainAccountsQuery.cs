using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Queries;

public sealed class DomainAccountsQuery : ListQuery, IRequest<IReadOnlyList<DomainAccountDto>>
{
    [Required]
    public Guid DomainId { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    public bool IncludeConfirmed { get; set; } = true;

    public bool IncludeNotConfirmed { get; set; } = false;
}