using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vouchers.Application.Queries;

public sealed class DomainValuesQuery : ListQuery
{
    [Required]
    public Guid DomainId { get; set; }

    public string Ticker { get; set; }

    public string IssuerName { get; set; }
}