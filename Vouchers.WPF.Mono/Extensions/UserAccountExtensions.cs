using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.WPF.Model;

namespace Vouchers.WPF.Mono.Extensions
{
    public static class UserAccountExtensions
    {
        public static UserAccount GetUserAccount(this Auth.UserCredentials userAccount)
        {
            var permissions = new UserPermissions(userAccount.Permissions.IssuerOperations, userAccount.Permissions.HolderOperations, userAccount.Permissions.UserAdministration);
            return new UserAccount(userAccount.Id, userAccount.Email, userAccount.PassHash, userAccount.LastPassUpdate, permissions, userAccount.Supply);
        }

        public static Auth.UserCredentials GetUserAccount(this UserAccount userAccount)
        {
            var permissions = new Auth.UserPermissions(userAccount.Permissions.IssuerOperations, userAccount.Permissions.HolderOperations, userAccount.Permissions.UserAdministration);
            return new Auth.UserAccount(userAccount.Id, userAccount.Email, userAccount.PassHash, userAccount.LastPassUpdate, permissions, userAccount.Supply);
        }

        public static UserAccount GetUserAccount(this Vouchers.Views.UserAccount userAccount)
        {
            var permissions = new UserPermissions(userAccount.IssuerOperations, userAccount.HolderOperations, userAccount.UserAdministration);
            return new UserAccount(userAccount.Id, userAccount.Email, null, userAccount.LastPassUpdate, permissions, 0);
        }
    }
}
