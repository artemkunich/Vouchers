using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vouchers.Application.Queries;

public sealed class IssuerValuesQuery : ListQuery
{
    [Required]
    public Guid IssuerAccountId { get; set; }

    public string Ticker { get; set; }
}