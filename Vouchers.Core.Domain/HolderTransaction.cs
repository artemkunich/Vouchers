using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Primitives;
using System.Globalization;

namespace Vouchers.Core.Domain;

public sealed class HolderTransaction : AggregateRoot<Guid>
{
    public DateTime Timestamp { get; private set; }

    public Guid CreditorAccountId { get; init; }
    public Account CreditorAccount { get; init; }

    public Guid DebtorAccountId { get; init; }
    public Account DebtorAccount { get; init; }
    
    public UnitTypeQuantity Quantity { get; private set; }

    public ICollection<HolderTransactionItem> TransactionItems { get; init; }

    public string Message { get; init; }
    public bool IsPerformed { get; private set; }

    public static HolderTransaction Create(Guid id, DateTime timestamp, Account creditorAccount, Account debtorAccount, UnitType unitType, string message, CultureInfo cultureInfo = null)
    {
        if (creditorAccount.Equals(debtorAccount))
            throw new CoreException("CreditorAndDebtorAreTheSame", cultureInfo);

        return new()
        {
            Id = id,
            Timestamp = timestamp,
            CreditorAccountId = creditorAccount.Id,
            CreditorAccount = creditorAccount,
            DebtorAccountId = debtorAccount.Id,
            DebtorAccount = debtorAccount,
            Quantity = UnitTypeQuantity.Create(0, unitType),
            Message = message,
            TransactionItems = new List<HolderTransactionItem>()
        };
    }

    public void AddTransactionItem(HolderTransactionItem item, CultureInfo cultureInfo = null)
    {
        if (item.Quantity.Unit.UnitType.NotEquals(Quantity.UnitType))
            throw new CoreException("TransactionAndItemHaveDifferentUnitTypes", cultureInfo);

        if (item.Quantity.Unit.ValidTo < DateTime.Today)
            throw new CoreException("TransactionContainsExpiredUnits", cultureInfo);

        Quantity = Quantity.Add(item.Quantity);
        TransactionItems.Add(item);           
    }
    
    public void Perform(CultureInfo cultureInfo = null) {
        if (Quantity.Amount == 0)
            throw new CoreException("AmountIsNotPositive", cultureInfo);

        foreach (var item in TransactionItems) {
            item.CreditAccountItem.ProcessCredit(item.Quantity.Amount);
            item.DebitAccountItem.ProcessDebit(item.Quantity.Amount);
        }

        IsPerformed = true;
    }
}
