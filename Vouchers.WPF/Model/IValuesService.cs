using System;
using System.Collections.Generic;

namespace Vouchers.WPF.Model
{
    public interface IValuesService : IDisposable
    {
        VoucherValue CreateVoucherValue(string category, string name, string description, UserAccount authUser);
        VoucherValue UpdateVoucherValue(int voucherValueId, string category, string name, string description, UserAccount authUser);
        void RemoveVoucherValue(int voucherValueId, UserAccount authUser);

        Voucher AddVoucher(int voucherValueId, DateTime validFrom, DateTime validTo, bool canBeExchanged, UserAccount authUser);
        Voucher UpdateVoucher(int voucherValueId, int voucherId, DateTime validFrom, DateTime validTo, bool canBeExchanged, UserAccount authUser);
        void RemoveVoucher(int voucherValueId, int voucherId, UserAccount authUser);

        Action OnDispose { get; set; }
    }
}
