using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Primitives;
using System.Globalization;
using System.Net;

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

    public static Result<HolderTransaction> Create(Guid id, DateTime timestamp, Account creditorAccount,
        Account debtorAccount, UnitType unitType, string message, CultureInfo cultureInfo = null) =>
        Result.Create()
            .IfTrueAddError(creditorAccount.Equals(debtorAccount), Errors.CreditorAndDebtorAreTheSame(cultureInfo))
            .SetValue(new HolderTransaction
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
            });

    public Result<HolderTransaction> AddTransactionItem(HolderTransactionItem item, CultureInfo cultureInfo = null) =>
        Result.Create(this)
            .IfTrueAddError(item.Quantity.Unit.UnitType.NotEquals(Quantity.UnitType),
                Errors.TransactionAndItemHaveDifferentUnitTypes(cultureInfo))
            .IfTrueAddError(item.Quantity.Unit.ValidTo < DateTime.Today,
                Errors.TransactionContainsExpiredUnits(cultureInfo))
            .MergeResultErrors(transaction =>
                transaction.Quantity.Add(item.Quantity)
                    .Process(quantity => transaction.Quantity = quantity)
            )
            .Process(transaction => transaction.TransactionItems.Add(item));

    public Result<HolderTransaction> Perform(CultureInfo cultureInfo = null) =>
        Result.Create(this)
            .IfTrueAddError(Quantity.Amount == 0, Errors.AmountIsNotPositive(cultureInfo))
            .ForeachWhileSuccess(transaction => transaction.TransactionItems, 
                (item, _) => Result.Create(item)
                    .MergeResultErrors(i => i.CreditAccountItem.ProcessCredit(i.Quantity.Amount))
                    .MergeResultErrors(i => i.DebitAccountItem.ProcessDebit(i.Quantity.Amount))
            )
            .Process(transaction => transaction.IsPerformed = true);
}
