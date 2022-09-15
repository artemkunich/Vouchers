using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model
{
    public interface IAdminService : IDisposable
    {
        void UpdatePermissions(string userAccountId, UserPermissions permissions, UserAccount authUser);
        void RemoveUserAccount(string userAccountId, UserAccount authUser);
        void ResetPassword(string userAccountId, string newPassword, UserAccount authUser);

        Action OnDispose { get; set; }
    }
}
