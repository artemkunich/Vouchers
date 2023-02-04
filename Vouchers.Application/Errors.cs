using System;
using System.Globalization;
using Vouchers.Application.UseCases;
using Vouchers.Core.Domain;
using Vouchers.Primitives;

namespace Vouchers.Application;

public class Errors
{
    public static Error DomainDoesNotExist(CultureInfo cultureInfo) => 
        new ("DomainDoesNotExist", ApplicationResources.GetString("DomainDoesNotExist", cultureInfo));
    
    public static Error OperationIsNotAllowed(CultureInfo cultureInfo) => 
        new ("OperationIsNotAllowed", ApplicationResources.GetString("OperationIsNotAllowed", cultureInfo));
    public static Error NotAuthorized(CultureInfo cultureInfo) => 
        new ("NotAuthorized", ApplicationResources.GetString("NotAuthorized", cultureInfo));
    public static Error NotRegisteredException(CultureInfo cultureInfo) => 
        new ("NotRegisteredException", ApplicationResources.GetString("NotRegisteredException", cultureInfo));
    
    public static Error DomainAccountDoesNotExist(CultureInfo cultureInfo) => 
        new ("DomainAccountDoesNotExist", ApplicationResources.GetString("DomainAccountDoesNotExist", cultureInfo));
    
    public static Error IdentityDoesNotHaveAccountInDomain(CultureInfo cultureInfo) => 
        new ("IdentityDoesNotHaveAccountInDomain", ApplicationResources.GetString("IdentityDoesNotHaveAccountInDomain", cultureInfo));
    
    public static Error CreditorAccountIsNotActivated(CultureInfo cultureInfo) => 
        new ("CreditorAccountIsNotActivated", ApplicationResources.GetString("CreditorAccountIsNotActivated", cultureInfo));
    
    public static Error TransactionRequestIsNotFound(CultureInfo cultureInfo) => 
        new ("TransactionRequestIsNotFound", ApplicationResources.GetString("TransactionRequestIsNotFound", cultureInfo));
    
    public static Error DebtorAccountDoesNotExist(CultureInfo cultureInfo) => 
        new ("DebtorAccountDoesNotExist", ApplicationResources.GetString("DebtorAccountDoesNotExist", cultureInfo));
    
    public static Error DebtorAccountIsNotActivated(CultureInfo cultureInfo) => 
        new ("DebtorAccountIsNotActivated", ApplicationResources.GetString("DebtorAccountIsNotActivated", cultureInfo));
    
    public static Error VoucherValueDoesNotExist(CultureInfo cultureInfo) => 
        new ("VoucherValueDoesNotExist", ApplicationResources.GetString("VoucherValueDoesNotExist", cultureInfo));
    
    public static Error CreditorAccountDoesNotExist(CultureInfo cultureInfo) => 
        new ("CreditorAccountDoesNotExist", ApplicationResources.GetString("CreditorAccountDoesNotExist", cultureInfo));
    
    public static Error UserDoesNotHaveAccountForVoucher(Guid accountId, Guid voucherId,  CultureInfo cultureInfo) => 
        new ("UserDoesNotHaveAccountForVoucher", string.Format(ApplicationResources.GetString("UserDoesNotHaveAccountForVoucher", cultureInfo),accountId, voucherId));
    
    
    
    
}