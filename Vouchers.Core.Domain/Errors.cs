using System.Globalization;
using Vouchers.Primitives;

namespace Vouchers.Core.Domain;

public static class Errors
{
    public static Error AmountIsNotPositive(CultureInfo cultureInfo) => 
        new ("AmountIsNotPositive", CoreResources.GetString("AmountIsNotPositive", cultureInfo));
    
    public static Error AmountIsGreaterThanSupply(CultureInfo cultureInfo) => 
        new ("AmountIsGreaterThanSupply", CoreResources.GetString("AmountIsGreaterThanSupply", cultureInfo));
    
    public static Error AmountIsGreaterThanBalance(CultureInfo cultureInfo) => 
        new ("AmountIsGreaterThanBalance", CoreResources.GetString("AmountIsGreaterThanBalance", cultureInfo));

    public static Error ValidToIsLessThanToday(CultureInfo cultureInfo) => 
        new ("ValidToIsLessThanToday", CoreResources.GetString("ValidToIsLessThanToday", cultureInfo));

    public static Error ValidFromIsGreaterThanValidTo(CultureInfo cultureInfo) => 
        new ("ValidFromIsGreaterThanValidTo", CoreResources.GetString("ValidFromIsGreaterThanValidTo", cultureInfo));
    
    public static Error UnitIsExpired(CultureInfo cultureInfo) => 
        new ("UnitIsExpired", CoreResources.GetString("UnitIsExpired", cultureInfo));
    
    public static Error AccountHolderAndUnitTypeIssuerAreDifferent(CultureInfo cultureInfo) => 
        new ("AccountHolderAndUnitTypeIssuerAreDifferent", CoreResources.GetString("AccountHolderAndUnitTypeIssuerAreDifferent", cultureInfo));

    public static Error AccountItemUnitAndTransactionUnitAreDifferent(CultureInfo cultureInfo) => 
        new ("AccountItemUnitAndTransactionUnitAreDifferent", CoreResources.GetString("AccountItemUnitAndTransactionUnitAreDifferent", cultureInfo));
    
    public static Error NewValidFromIsGreaterThanCurrentValidFrom(CultureInfo cultureInfo) => 
        new ("NewValidFromIsGreaterThanCurrentValidFrom", CoreResources.GetString("NewValidFromIsGreaterThanCurrentValidFrom", cultureInfo));
    
    public static Error NewValidFromIsGreaterThanCurrentValidTo(CultureInfo cultureInfo) => 
        new ("NewValidFromIsGreaterThanCurrentValidTo", CoreResources.GetString("NewValidFromIsGreaterThanCurrentValidTo", cultureInfo));
    
    public static Error NewValidToIsLessThanCurrentValidFrom(CultureInfo cultureInfo) => 
        new ("NewValidToIsLessThanCurrentValidFrom", CoreResources.GetString("NewValidToIsLessThanCurrentValidFrom", cultureInfo));
    
    public static Error CurrentValidFromIsGreaterThanNewValidTo(CultureInfo cultureInfo) => 
        new ("CurrentValidFromIsGreaterThanNewValidTo", CoreResources.GetString("CurrentValidFromIsGreaterThanNewValidTo", cultureInfo));
  
    public static Error CannotDisableExchangeability(CultureInfo cultureInfo) => 
        new ("CannotDisableExchangeability", CoreResources.GetString("CannotDisableExchangeability", cultureInfo));

    public static Error CreditorAndDebtorAreTheSame(CultureInfo cultureInfo) => 
        new ("CreditorAndDebtorAreTheSame", CoreResources.GetString("CreditorAndDebtorAreTheSame", cultureInfo));
    
    public static Error TransactionAndItemHaveDifferentUnitTypes(CultureInfo cultureInfo) => 
        new ("TransactionAndItemHaveDifferentUnitTypes", CoreResources.GetString("TransactionAndItemHaveDifferentUnitTypes", cultureInfo));
    
    public static Error TransactionContainsExpiredUnits(CultureInfo cultureInfo) => 
        new ("TransactionContainsExpiredUnits", CoreResources.GetString("TransactionContainsExpiredUnits", cultureInfo));

    public static Error CreditAccountAndItemHaveDifferentUnits(CultureInfo cultureInfo) => 
        new ("CreditAccountAndItemHaveDifferentUnits", CoreResources.GetString("CreditAccountAndItemHaveDifferentUnits", cultureInfo));
    
    public static Error DebitAccountAndItemHaveDifferentUnits(CultureInfo cultureInfo) => 
        new ("DebitAccountAndItemHaveDifferentUnits", CoreResources.GetString("DebitAccountAndItemHaveDifferentUnits", cultureInfo));
    
    public static Error ItemUnitCannotBeExchanged(CultureInfo cultureInfo) => 
        new ("ItemUnitCannotBeExchanged", CoreResources.GetString("ItemUnitCannotBeExchanged", cultureInfo));

    public static Error CannotOperateWithDifferentUnitTypes(CultureInfo cultureInfo) => 
        new ("CannotOperateWithDifferentUnitTypes", CoreResources.GetString("CannotOperateWithDifferentUnitTypes", cultureInfo));
    
    public static Error IssuerCannotSetMaxDurationBeforeValidityStart(CultureInfo cultureInfo) => 
        new ("IssuerCannotSetMaxDurationBeforeValidityStart", CoreResources.GetString("IssuerCannotSetMaxDurationBeforeValidityStart", cultureInfo));
    
    public static Error IssuerCannotSetMinDurationBeforeValidityEnd(CultureInfo cultureInfo) => 
        new ("IssuerCannotSetMinDurationBeforeValidityEnd", CoreResources.GetString("IssuerCannotSetMinDurationBeforeValidityEnd", cultureInfo));
        
    public static Error IssuerCannotRequireExchangeability(CultureInfo cultureInfo) => 
        new ("IssuerCannotRequireExchangeability", CoreResources.GetString("IssuerCannotRequireExchangeability", cultureInfo));
    
    public static Error TransactionIsAlreadyPerformed(CultureInfo cultureInfo) => 
        new ("TransactionIsAlreadyPerformed", CoreResources.GetString("TransactionIsAlreadyPerformed", cultureInfo));
    
    public static Error RequestCreditorIsNotSatisfiedByTransaction(CultureInfo cultureInfo) => 
        new ("RequestCreditorIsNotSatisfiedByTransaction", CoreResources.GetString("RequestCreditorIsNotSatisfiedByTransaction", cultureInfo));
    
    public static Error RequestDebtorIsNotSatisfiedByTransaction(CultureInfo cultureInfo) => 
        new ("RequestDebtorIsNotSatisfiedByTransaction", CoreResources.GetString("RequestDebtorIsNotSatisfiedByTransaction", cultureInfo));
    
    public static Error RequestUnitIsNotSatisfiedByTransaction(CultureInfo cultureInfo) => 
        new ("RequestUnitIsNotSatisfiedByTransaction", CoreResources.GetString("RequestUnitIsNotSatisfiedByTransaction", cultureInfo));
    
    public static Error RequestAmountIsNotSatisfiedByTransaction(CultureInfo cultureInfo) => 
        new ("RequestAmountIsNotSatisfiedByTransaction", CoreResources.GetString("RequestAmountIsNotSatisfiedByTransaction", cultureInfo));
    
    public static Error RequestMaxValidFromIsNotSatisfiedByTransaction(CultureInfo cultureInfo) => 
        new ("RequestMaxValidFromIsNotSatisfiedByTransaction", CoreResources.GetString("RequestMaxValidFromIsNotSatisfiedByTransaction", cultureInfo)); 

    public static Error RequestMinValidToIsNotSatisfiedByTransaction(CultureInfo cultureInfo) => 
        new ("RequestMinValidToIsNotSatisfiedByTransaction", CoreResources.GetString("RequestMinValidToIsNotSatisfiedByTransaction", cultureInfo)); 

    public static Error RequestMustBeExchangeableIsNotSatisfiedByTransaction(CultureInfo cultureInfo) => 
        new ("RequestMustBeExchangeableIsNotSatisfiedByTransaction", CoreResources.GetString("RequestMustBeExchangeableIsNotSatisfiedByTransaction", cultureInfo));
}