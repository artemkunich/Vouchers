using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model
{
    public class UserAccount
    {
        public string Id { get; }

        public string Email { get; set; }

        public string PassHash { get; set; }

        public DateTime LastPassUpdate { get; set; }

        public UserPermissions Permissions { get; set; }

        public decimal Supply { get; set; }

        public UserAccount(string id, string email, string passHash, DateTime lastPassUpdate, UserPermissions permissions, decimal supply)
        {
            Id = id;
            PassHash = passHash;
            LastPassUpdate = lastPassUpdate;
            Permissions = permissions;
            Supply = supply;
            Email = email;
        }
    }
}
