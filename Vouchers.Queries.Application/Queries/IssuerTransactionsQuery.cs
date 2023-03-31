using System;
using System.Collections.Generic;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Queries;

public sealed class IssuerTransactionsQuery : ListQuery, IRequest<IReadOnlyList<IssuerTransactionDto>>
{
    public string Ticker { get; set; }

    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }

    public DateTime? MinTimestamp { get; set; }
    public DateTime? MaxTimestamp { get; set; }
}