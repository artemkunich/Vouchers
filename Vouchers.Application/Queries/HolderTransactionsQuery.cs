using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Queries;

public sealed class HolderTransactionsQuery : ListQuery, IRequest<IReadOnlyList<HolderTransactionDto>>
{
    [Required]
    public Guid AccountId { get; set; } 
    public string Ticker { get; set; }
    public string IssuerName { get; set; }

    public string CounterpartyName { get; set; }

    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }

    public DateTime? MinTimestamp { get; set; }
    public DateTime? MaxTimestamp { get; set; }
}