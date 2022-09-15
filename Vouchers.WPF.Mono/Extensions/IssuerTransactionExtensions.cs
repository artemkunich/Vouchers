using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.WPF.Model;

namespace Vouchers.WPF.Mono.Extensions
{
    public static class IssuerTransactionExtensions
    {
        public static IssuerTransaction GetIssuerTransaction(this Vouchers.Views.IssuerTransaction transaction)
        {
            return new IssuerTransaction(
                transaction.Id,
                transaction.Timestamp,
                transaction.UnitCategory,
                transaction.UnitName,
                transaction.ValidFrom,
                transaction.ValidTo,
                transaction.CanBeExchanged,
                transaction.Amount
            );
        }
    }
}
