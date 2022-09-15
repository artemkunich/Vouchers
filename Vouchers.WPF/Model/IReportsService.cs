using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.WPF.Model.Specifications;

namespace Vouchers.WPF.Model
{
    public interface IReportsService : IDisposable
    {
        IEnumerable<VoucherValue> GetIssuerVoucherValues(UserAccount authUser);
        IEnumerable<VoucherValue> GetHolderVoucherValues(UserAccount authUser);

        IEnumerable<HolderTransaction> GetHolderTransactions(IEnumerable<TransactionSpecification> specifications, UserAccount authUser);
        IEnumerable<IssuerTransaction> GetIssuerTransactions(IEnumerable<TransactionSpecification> specifications, UserAccount authUser);

        IEnumerable<UserAccount> GetUserAccounts(IEnumerable<UserAccountSpecification> specifications, UserAccount authUser);

        Action OnDispose { get; set; }
    }
}
