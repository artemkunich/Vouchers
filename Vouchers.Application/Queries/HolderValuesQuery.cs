using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vouchers.Application.Queries;

public sealed class HolderValuesQuery : ListQuery
{
    [Required]
    public Guid HolderId { get; set; }

    public string Ticker { get; set; }

    public string IssuerName { get; set; }
}