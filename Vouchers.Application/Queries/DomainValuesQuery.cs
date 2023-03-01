using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Queries;

[IdentityRoles(IdentityRole.User)]
public sealed class DomainValuesQuery : ListQuery, IRequest<IReadOnlyList<VoucherValueDto>>
{
    [Required]
    public Guid DomainId { get; set; }

    public string Ticker { get; set; }

    public string IssuerName { get; set; }
}