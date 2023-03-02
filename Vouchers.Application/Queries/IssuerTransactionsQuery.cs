using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Queries;

public sealed class IssuerTransactionsQuery : ListQuery, IRequest<IReadOnlyList<IssuerTransactionDto>>
{
    public string Ticker { get; set; }

    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }

    public DateTime? MinTimestamp { get; set; }
    public DateTime? MaxTimestamp { get; set; }
}