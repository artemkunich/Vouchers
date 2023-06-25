using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vouchers.Domains.Application.Dtos;

public sealed class HolderTransactionRequestDto
{
    public Guid Id { get; set; }

    public DateTime DueDate { get; set; }

    public Guid? CreditorAccountId { get; set; }
    public Guid DebtorAccountId { get; set; }

    public string CounterpartyName { get; set; }
    public string CounterpartyEmail { get; set; }
    public Guid? CounterpartyImageId { get; set; }

    public decimal Amount { get; set; }

    public Guid UnitTypeId { get; set; }
    public string UnitTicker { get; set; }
    public Guid? UnitImageId { get; set; }

    public Guid UnitIssuerAccountId { get; set; }
    public string UnitIssuerEmail { get; set; }
    public string UnitIssuerName { get; set; }

    public int MaxDaysBeforeValidityStart { get; set; }
    public int MinDaysBeforeValidityEnd { get; set; }

    public bool MustBeExchangeable { get; set; }

    public string Message { get; set; }
    public Guid? TransactionId { get; set; }

}