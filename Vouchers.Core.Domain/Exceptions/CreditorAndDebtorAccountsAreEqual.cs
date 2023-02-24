using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class CreditorAndDebtorAccountsAreEqualException : InvalidOperationException
{
    public override string Message => "Creditor and debtor accounts are equal";
}