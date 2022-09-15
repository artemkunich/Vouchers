using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model
{
    public class UserPermissions
    {
        public bool IssuerOperations { get; }

        public bool HolderOperations { get; }

        public bool UserAdministration { get; }

        public UserPermissions(bool issuerOperations, bool holderOperations, bool userAdministration)
        {
            IssuerOperations = issuerOperations;
            HolderOperations = holderOperations;
            UserAdministration = userAdministration;
        }
    }
}
