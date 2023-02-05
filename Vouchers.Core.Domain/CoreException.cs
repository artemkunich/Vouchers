using System;
using System.Dynamic;
using System.Globalization;

namespace Vouchers.Core.Domain;

public class CoreException : Exception
{
    internal CoreException(string message) : base(message)
    {
    }

    internal static CoreException Create(string message) => new (message);
    internal static CoreException AmountIsNotPositive => Create("Amount is not positive");
    internal static CoreException AmountIsGreaterThanSupply => Create("Amount is greater than supply");
    internal static CoreException AmountIsGreaterThanBalance => Create("Amount is greater than balance");
    internal static CoreException CreditorAndDebtorAreTheSame => Create("Creditor and debtor are the same");
    internal static CoreException CreditorAndDebtorAccountsAreTheSame => Create("Creditor and debtor accounts are the same");
    internal static CoreException TransactionAndItemHaveDifferentUnitTypes => Create("Transaction and item have different unit types");
    internal static CoreException TransactionContainsExpiredUnits => Create("Transaction contains expired units");
    internal static CoreException CreditAccountAndItemHaveDifferentUnits => Create("Credit account and item have different units");
    internal static CoreException DebitAccountAndItemHaveDifferentUnits => Create("Debit account and item have different units");
    internal static CoreException ItemUnitCannotBeExchanged => Create("Item unit cannot be exchanged");
    internal static CoreException IssuerCannotSetMaxDurationBeforeValidityStart => Create("Issuer cannot set max duration before validity start");
    internal static CoreException IssuerCannotSetMinDurationBeforeValidityEnd => Create("Issuer cannot set min duration before validity end");
    internal static CoreException IssuerCannotRequireExchangeability => Create("Issuer cannot require exchangeability");
    internal static CoreException TransactionIsAlreadyPerformed => Create("Transaction is already performed");
    internal static CoreException RequestCreditorIsNotSatisfiedByTransaction => Create("Request creditor is not satisfied by transaction");
    internal static CoreException RequestDebtorIsNotSatisfiedByTransaction => Create("Request debtor is not satisfied by transaction");
    internal static CoreException RequestUnitIsNotSatisfiedByTransaction => Create("Request unit is not satisfied by transaction");
    internal static CoreException RequestAmountIsNotSatisfiedByTransaction => Create("Request amount is not satisfied by transaction");
    internal static CoreException RequestMaxValidFromIsNotSatisfiedByTransaction => Create("Request MaxValidFrom is not satisfied by transaction");
    internal static CoreException RequestMinValidToIsNotSatisfiedByTransaction => Create("Request MinValidTo is not satisfied by transaction");
    internal static CoreException RequestMustBeExchangeableIsNotSatisfiedByTransaction => Create("Request MustBeExchangeable is not satisfied by transaction");
    internal static CoreException UnitIsExpired => Create("Unit is expired");
    internal static CoreException AccountHolderAndUnitTypeIssuerAreDifferent => Create("Account holder and unitType issuer are different");
    internal static CoreException AccountItemUnitAndTransactionUnitAreDifferent => Create("AccountItem unit and transaction unit are different");
    internal static CoreException ValidToIsLessThanToday => Create("ValidTo is less than today");
    internal static CoreException ValidFromIsGreaterThanValidTo => Create("ValidFrom is greater than ValidTo");

    internal static CoreException NewValidFromIsGreaterThanCurrentValidFrom => Create("New ValidFrom is greater than current ValidFrom");
    internal static CoreException NewValidFromIsGreaterThanCurrentValidTo => Create("New ValidFrom is greater than current ValidTo");
    internal static CoreException NewValidToIsLessThanCurrentValidFrom => Create("New ValidTo is less than current ValidFrom");
    internal static CoreException CurrentValidFromIsGreaterThanNewValidTo => Create("Current ValidFrom is greater than new ValidTo");
    internal static CoreException CannotDisableExchangeability => Create("Cannot disable exchangeability");
    internal static CoreException CannotOperateWithDifferentUnitTypes => Create("Cannot operate with different unit types");
    
}