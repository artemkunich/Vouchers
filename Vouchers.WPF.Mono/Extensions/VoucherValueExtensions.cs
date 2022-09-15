using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.WPF.Model;

namespace Vouchers.WPF.Mono.Extensions
{
    public static class VoucherValueExtensions
    {
        public static VoucherValue GetVoucherValue(this Values.VoucherValueDetail value)
        {
            return new VoucherValue(
                value.Id,
                value.Category,
                value.Name,
                value.Description,
                value.Issuer.Id, value.Supply,
                0,
                value.Vouchers.Select(v => new Voucher(v.Id, v.ValidFrom, v.ValidTo, v.CanBeExchanged, v.Supply, 0))
            );
        }

        public static VoucherValue GetVoucherValue(this Vouchers.Views.VoucherValue value)
        {
            return new VoucherValue(
                value.Id,
                value.Category,
                value.Name,
                value.Description,
                value.IssuerId,
                value.Supply,
                value.Balance,
                value.Vouchers.Select(v => new Voucher(v.Id, v.ValidFrom, v.ValidTo, v.CanBeExchanged, v.Supply, v.Balance))
            );
        }
    }
}
