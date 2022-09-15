using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.WPF.Model;

namespace Vouchers.WPF.Mono.Extensions
{
    public static class HolderTransactionExtensions
    {
        public static HolderTransaction GetHolderTransaction(this Vouchers.Views.HolderTransaction transaction)
        {
            return new HolderTransaction(
                transaction.Id,
                transaction.Timestamp,
                transaction.CreditorId,
                transaction.DebtorId,
                transaction.UnitCategory,
                transaction.UnitName,
                transaction.UnitIssuerId,
                transaction.Amount,
                transaction.Items.Select(item => new VoucherQuantity(new Voucher(item.Unit.Id, item.Unit.ValidFrom, item.Unit.ValidTo, item.Unit.CanBeExchanged, item.Unit.Supply, item.Unit.Balance), item.Amount))
            );
        }
    }
}
