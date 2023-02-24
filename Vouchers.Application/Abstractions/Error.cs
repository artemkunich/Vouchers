using System;
using System.Globalization;
using Vouchers.Application.UseCases;

namespace Vouchers.Application.Abstractions;

public class Error : IEquatable<Error>
{
    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public bool Equals(object other)
    {
        if (other is null)
            return false;

        if(other is Error error)
            return Code == error.Code;

        return false;
    }
    
    public bool Equals(Error other)
    {
        if (other is null)
            return false;

        return Code == other.Code;
    }

    public static bool operator ==(Error a, Error b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Error a, Error b) => !(a == b);

    public static Error Create(string code, CultureInfo cultureInfo) =>
        new(code, ApplicationResources.GetString(code, cultureInfo));
        
    public static Error Create(string code, CultureInfo cultureInfo, params object[] args) =>
        new(code, ApplicationResources.GetString(code, cultureInfo, args));
        
    
    public static Error DomainDoesNotExist(CultureInfo cultureInfo = null) => 
        Create("DomainDoesNotExist", cultureInfo);
    
    public static Error OperationIsNotAllowed(CultureInfo cultureInfo = null) => 
        Create("OperationIsNotAllowed", cultureInfo);

    public static Error NotRegistered(CultureInfo cultureInfo = null) => 
        Create("NotRegistered", cultureInfo);
    
    public static Error DomainAccountDoesNotExist(CultureInfo cultureInfo = null) => 
        Create("DomainAccountDoesNotExist", cultureInfo);
    
    public static Error MaxCountOfContractsExceeded(CultureInfo cultureInfo = null) => 
        Create("MaxCountOfContractsExceeded", cultureInfo);

    public static Error IdentityDoesNotHaveAccountInDomain(CultureInfo cultureInfo = null) => 
        Create("IdentityDoesNotHaveAccountInDomain", cultureInfo);
    
    public static Error CreditorAccountIsNotActivated(CultureInfo cultureInfo = null) => 
        Create("CreditorAccountIsNotActivated", cultureInfo);
    
    public static Error TransactionRequestIsNotFound(CultureInfo cultureInfo = null) => 
        Create("TransactionRequestIsNotFound", cultureInfo);
    
    public static Error DebtorAccountDoesNotExist(CultureInfo cultureInfo = null) => 
        Create("DebtorAccountDoesNotExist", cultureInfo);
    
    public static Error DebtorAccountIsNotActivated(CultureInfo cultureInfo = null) => 
        Create("DebtorAccountIsNotActivated", cultureInfo);
    
    public static Error VoucherValueDoesNotExist(CultureInfo cultureInfo = null) => 
        Create("VoucherValueDoesNotExist", cultureInfo);
    
    public static Error CreditorAccountDoesNotExist(CultureInfo cultureInfo = null) => 
        Create("CreditorAccountDoesNotExist", cultureInfo);
    
    public static Error UserDoesNotHaveAccountForVoucher(Guid accountId, Guid voucherId,  CultureInfo cultureInfo = null) => 
        Create ("UserDoesNotHaveAccountForVoucher", cultureInfo ,accountId, voucherId);
    
    public static Error ValidFromCannotBeModifiedOnActiveOffers(CultureInfo cultureInfo = null) => 
        Create ("ValidFromCannotBeModifiedOnActiveOffers", cultureInfo);
    
    public static Error ValidFromCannotBeSetToPast(CultureInfo cultureInfo = null) => 
        Create ("ValidFromCannotBeSetToPast", cultureInfo);
    
    public static Error ValidToCannotBeLessThanValidFrom(CultureInfo cultureInfo = null) => 
        Create ("ValidToCannotBeLessThanValidFrom", cultureInfo);
    
    public static Error ValidToCannotBeLessThanCurrentDatetime(CultureInfo cultureInfo = null) => 
        Create ("ValidToCannotBeLessThanCurrentDatetime", cultureInfo);
    
    
    public static Error TransactionRequestIsAlreadyPerformed(CultureInfo cultureInfo = null) => 
        Create ("TransactionRequestIsAlreadyPerformed", cultureInfo);
    
    public static Error IdentityDoesNotExist(CultureInfo cultureInfo = null) => 
        Create ("IdentityDoesNotExist", cultureInfo);
    
    public static Error UnitTypeDoesNotExist(CultureInfo cultureInfo = null) => 
        Create ("UnitTypeDoesNotExist", cultureInfo);
    
    public static Error IssuerAccountIsNotActivated(CultureInfo cultureInfo = null) => 
        Create ("IssuerAccountIsNotActivated", cultureInfo);
    
    public static Error IssuerAccountDoesNotExist(CultureInfo cultureInfo = null) => 
        Create ("IssuerAccountDoesNotExist", cultureInfo);
    
    public static Error IssuerDoesNotHaveAccountItemForUnit(CultureInfo cultureInfo = null) => 
        Create ("IssuerDoesNotHaveAccountItemForUnit", cultureInfo);
    
    public static Error VoucherDoesNotExist(CultureInfo cultureInfo = null) => 
        Create ("VoucherDoesNotExist", cultureInfo);
    
    public static Error IssuerOperationsAreNotAllowed(CultureInfo cultureInfo = null) => 
        Create ("IssuerOperationsAreNotAllowed", cultureInfo);
    
    public static Error ImageDoesNotExist(CultureInfo cultureInfo = null) => 
        Create ("ImageDoesNotExist", cultureInfo);

}