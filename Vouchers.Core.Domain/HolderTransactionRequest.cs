using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Primitives;
using System.Globalization;

namespace Vouchers.Core.Domain;

public sealed class HolderTransactionRequest : AggregateRoot<Guid>
{
    public DateTime DueDate { get; init; }
    public Guid? CreditorAccountId { get; init; }
    public Account CreditorAccount { get; init; }
    public Guid DebtorAccountId { get; init; }
    public Account DebtorAccount { get; init; }
    public UnitTypeQuantity Quantity { get; init; }
    public TimeSpan? MaxDurationBeforeValidityStart { get; init; }
    public TimeSpan? MinDurationBeforeValidityEnd { get; init; }
    public bool MustBeExchangeable { get; init; }
    public string Message { get; init; }
    public Guid? TransactionId { get; private set; }
    public HolderTransaction Transaction { get; private set; }
    

    public static Result<HolderTransactionRequest> Create(Guid id, DateTime dueDate, Account creditorAccount, Account debtorAccount,
        UnitTypeQuantity quantity, TimeSpan maxDurationBeforeValidityStart, TimeSpan minDurationBeforeValidityEnd,
        bool mustBeExchangeable, string message, CultureInfo cultureInfo = null)
    {
        var issuerIsDebtor = quantity.UnitType.IssuerAccount.Equals(debtorAccount);

        return Result.Create()
            .IfTrueAddError(quantity.Amount <= 0, Errors.AmountIsNotPositive(cultureInfo))
            .IfTrueAddError(issuerIsDebtor && maxDurationBeforeValidityStart != TimeSpan.Zero,
                Errors.IssuerCannotSetMaxDurationBeforeValidityStart(cultureInfo))
            .IfTrueAddError(issuerIsDebtor && minDurationBeforeValidityEnd != TimeSpan.Zero,
                Errors.IssuerCannotSetMinDurationBeforeValidityEnd(cultureInfo))
            .IfTrueAddError(issuerIsDebtor && mustBeExchangeable, Errors.IssuerCannotRequireExchangeability(cultureInfo))
            .IfTrueAddError(creditorAccount is not null && debtorAccount.Equals(creditorAccount),
                Errors.CreditorAndDebtorAreTheSame(cultureInfo))
            .SetValue(new HolderTransactionRequest()
            {
                Id = id,
                DueDate = dueDate,
                DebtorAccountId = debtorAccount.Id,
                DebtorAccount = debtorAccount,
                Quantity = quantity,
                MaxDurationBeforeValidityStart = maxDurationBeforeValidityStart,
                MinDurationBeforeValidityEnd = minDurationBeforeValidityEnd,
                MustBeExchangeable = mustBeExchangeable,
                Message = message,
                CreditorAccount = creditorAccount,
                CreditorAccountId = creditorAccount?.Id
            });
    }

    public Result<HolderTransactionRequest> Perform(HolderTransaction transaction, CultureInfo cultureInfo = null) =>
        Result.Create(this)
            .IfTrueAddError(Transaction != null, Errors.TransactionIsAlreadyPerformed(cultureInfo))
            .IfTrueAddError(CreditorAccount != null && CreditorAccount.NotEquals(transaction.CreditorAccount),
                Errors.RequestCreditorIsNotSatisfiedByTransaction(cultureInfo))
            .IfTrueAddError(DebtorAccount.NotEquals(transaction.DebtorAccount),
                Errors.RequestDebtorIsNotSatisfiedByTransaction(cultureInfo))
            .IfTrueAddError(Quantity.UnitType.NotEquals(transaction.Quantity.UnitType),
                Errors.RequestUnitIsNotSatisfiedByTransaction(cultureInfo))
            .IfTrueAddError(Quantity.Amount != transaction.Quantity.Amount,
                Errors.RequestAmountIsNotSatisfiedByTransaction(cultureInfo))
            .ForeachWhileSuccess(request => request.Transaction.TransactionItems, (item, request) =>
                Result.Create(item)
                    .IfTrueAddError(
                        request.MaxDurationBeforeValidityStart is not null && item.Quantity.Unit.ValidFrom >
                        DateTime.Now.Add(request.MaxDurationBeforeValidityStart.Value),
                        Errors.RequestMaxValidFromIsNotSatisfiedByTransaction(cultureInfo))
                    .IfTrueAddError(
                        MinDurationBeforeValidityEnd is not null && item.Quantity.Unit.ValidTo <
                        DateTime.Now.Add(MinDurationBeforeValidityEnd.Value),
                        Errors.RequestMinValidToIsNotSatisfiedByTransaction(cultureInfo))
                    .IfTrueAddError(MustBeExchangeable && !item.Quantity.Unit.CanBeExchanged,
                        Errors.RequestMustBeExchangeableIsNotSatisfiedByTransaction(cultureInfo))
            )
            .MergeResultErrors(request => transaction.Perform(cultureInfo))
            .Process(request =>
            {
                request.TransactionId = transaction.Id;
                request.Transaction = transaction;
            });
}

