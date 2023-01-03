using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vouchers.Application.Dtos;

public sealed class HolderTransactionDto
{
    public Guid Id { get; set; }

    public DateTime Timestamp { get; set; }

    public Guid CreditorAccountId { get; set; }
    public string CreditorName { get; set; }
    public string CreditorEmail { get; set; }
    public Guid? CreditorImageId { get; set; }

    public Guid DebtorAccountId { get; set; }
    public string DebtorName { get; set; }
    public string DebtorEmail { get; set; }
    public Guid? DebtorImageId { get; set; }

    public Guid UnitTypeId { get; set; }
    public string UnitTicker { get; set; }
    public Guid? UnitImageId { get; set; }

    public Guid UnitIssuerId { get; set; }
    public string UnitIssuerName { get; set; }
    public string UnitIssuerEmail { get; set; }

    public decimal Amount { get; set; }

    public string Message { get; set; }

    public IEnumerable<VoucherQuantityDto> Items { get; set; }
}