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

}