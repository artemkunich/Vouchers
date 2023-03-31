using System;
using System.Collections.Generic;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Queries;

public sealed class HolderTransactionRequestsQuery : ListQuery, IRequest<IReadOnlyList<HolderTransactionRequestDto>>
{
    public Guid AccountId { get; set; }
    public string Ticker { get; set; }
    public string IssuerName { get; set; }
    public string CounterpartyName { get; set; }

    public bool IncludeIncoming { get; set; }
    public bool IncludeOutgoing { get; set; }

    public bool IncludePerformed { get; set; }
    public bool IncludeNotPerformed { get; set; }

    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }

    public DateTime? MinDueDate { get; set; }
    public DateTime? MaxDueDate { get; set; }
}