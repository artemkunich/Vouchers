using System;

namespace Vouchers.Core.Domain.Exceptions;

public sealed class CreditAccountAndDebitAccountHaveDifferentUnitsException : InvalidOperationException
{
    public override string Message => "Credit account and debit account have different units";
}