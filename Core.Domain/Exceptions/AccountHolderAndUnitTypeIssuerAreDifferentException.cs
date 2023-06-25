using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class AccountHolderAndUnitTypeIssuerAreDifferentException : InvalidOperationException
{
    public override string Message => "Account holder and unitType issuer are different";
}