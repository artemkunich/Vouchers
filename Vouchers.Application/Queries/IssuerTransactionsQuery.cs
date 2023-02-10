using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Queries;

[ApplicationRoles(ApplicationRole.User)]
public sealed class IssuerTransactionsQuery : ListQuery
{
    public string Ticker { get; set; }

    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }

    public DateTime? MinTimestamp { get; set; }
    public DateTime? MaxTimestamp { get; set; }
}