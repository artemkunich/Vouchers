using System;
using System.Globalization;
using Vouchers.Application.UseCases;

namespace Vouchers.Application;

public class Error : IEquatable<Error>
{
    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        Code = code;
        Message = code;
    }


    public bool Equals(Error? other)
    {
        if (other is null)
            return false;

        if (Code == other.Code)
            return true;

        return false;
    }

    public static bool operator ==(Error? a, Error? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Error? a, Error? b) => !(a == b);

    public static Error Create(string code, CultureInfo cultureInfo) =>
        new(code, ApplicationResources.GetString(code, cultureInfo));
        
    public static Error Create(string code, CultureInfo cultureInfo, params object[] args) =>
        new(code, ApplicationResources.GetString(code, cultureInfo, args));
        
    
    public static Error DomainDoesNotExist(CultureInfo cultureInfo) => 
        Create("DomainDoesNotExist", cultureInfo);
    
    public static Error OperationIsNotAllowed(CultureInfo cultureInfo) => 
        Create("OperationIsNotAllowed", cultureInfo);
    
    public static Error NotAuthorized(CultureInfo cultureInfo) => 
        Create("NotAuthorized", cultureInfo);
    
    public static Error NotRegistered(CultureInfo cultureInfo) => 
        Create("NotRegistered", cultureInfo);
    
    public static Error DomainAccountDoesNotExist(CultureInfo cultureInfo) => 
        Create("DomainAccountDoesNotExist", cultureInfo);
    
    public static Error MaxCountOfContractsExceeded(CultureInfo cultureInfo) => 
        Create("MaxCountOfContractsExceeded", cultureInfo);

    public static Error IdentityDoesNotHaveAccountInDomain(CultureInfo cultureInfo) => 
        Create("IdentityDoesNotHaveAccountInDomain", cultureInfo);
    
    public static Error CreditorAccountIsNotActivated(CultureInfo cultureInfo) => 
        Create("CreditorAccountIsNotActivated", cultureInfo);
    
    public static Error TransactionRequestIsNotFound(CultureInfo cultureInfo) => 
        Create("TransactionRequestIsNotFound", cultureInfo);
    
    public static Error DebtorAccountDoesNotExist(CultureInfo cultureInfo) => 
        Create("DebtorAccountDoesNotExist", cultureInfo);
    
    public static Error DebtorAccountIsNotActivated(CultureInfo cultureInfo) => 
        Create("DebtorAccountIsNotActivated", cultureInfo);
    
    public static Error VoucherValueDoesNotExist(CultureInfo cultureInfo) => 
        Create("VoucherValueDoesNotExist", cultureInfo);
    
    public static Error CreditorAccountDoesNotExist(CultureInfo cultureInfo) => 
        Create("CreditorAccountDoesNotExist", cultureInfo);
    
    public static Error UserDoesNotHaveAccountForVoucher(Guid accountId, Guid voucherId,  CultureInfo cultureInfo) => 
        Create ("UserDoesNotHaveAccountForVoucher", cultureInfo ,accountId, voucherId);
    
    public static Error ValidFromCannotBeModifiedOnActiveOffers(CultureInfo cultureInfo) => 
        Create ("ValidFromCannotBeModifiedOnActiveOffers", cultureInfo);
    
    public static Error ValidFromCannotBeSetToPast(CultureInfo cultureInfo) => 
        Create ("ValidFromCannotBeSetToPast", cultureInfo);
    
    public static Error ValidToCannotBeLessThanValidFrom(CultureInfo cultureInfo) => 
        Create ("ValidToCannotBeLessThanValidFrom", cultureInfo);
    
    public static Error ValidToCannotBeLessThanCurrentDatetime(CultureInfo cultureInfo) => 
        Create ("ValidToCannotBeLessThanCurrentDatetime", cultureInfo);
    
    
    public static Error TransactionRequestIsAlreadyPerformed(CultureInfo cultureInfo) => 
        Create ("TransactionRequestIsAlreadyPerformed", cultureInfo);
    
    public static Error IdentityDoesNotExist(CultureInfo cultureInfo) => 
        Create ("IdentityDoesNotExist", cultureInfo);
    
    public static Error UnitTypeDoesNotExist(CultureInfo cultureInfo) => 
        Create ("UnitTypeDoesNotExist", cultureInfo);
    
    public static Error IssuerAccountIsNotActivated(CultureInfo cultureInfo) => 
        Create ("IssuerAccountIsNotActivated", cultureInfo);
    
    public static Error IssuerAccountDoesNotExist(CultureInfo cultureInfo) => 
        Create ("IssuerAccountDoesNotExist", cultureInfo);
    
    public static Error VoucherDoesNotExist(CultureInfo cultureInfo) => 
        Create ("VoucherDoesNotExist", cultureInfo);
    
    public static Error IssuerOperationsAreNotAllowed(CultureInfo cultureInfo) => 
        Create ("IssuerOperationsAreNotAllowed", cultureInfo);
    
    
    
}