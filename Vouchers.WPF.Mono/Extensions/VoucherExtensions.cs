using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.WPF.Model;

namespace Vouchers.WPF.Mono.Extensions
{
    public static class VoucherExtensions
    {
        public static Voucher GetVoucher(this Values.Voucher voucher)
        {
            return new Voucher(voucher.Id, voucher.ValidFrom, voucher.ValidTo, voucher.CanBeExchanged, voucher.Supply, 0);
        }
    }
}
