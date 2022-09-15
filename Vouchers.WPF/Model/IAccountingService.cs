using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model
{
    public interface IAccountingService : IDisposable
    {
        void CreateHolderTransaction(string creditorId, string debtorId, decimal amount, int unitId, IEnumerable<VoucherQuantity> items, UserAccount authUser);
        void CreateIssuerTransaction(decimal amount, int voucherId, UserAccount authUser);
        Action OnDispose { get; set; }
    }
}
