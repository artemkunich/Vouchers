using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model.Specifications
{
    public class UserAccountEmail : UserAccountSpecification
    {
        public string Email { get; }

        public UserAccountEmail(string email) {
            Email = email;
        }
    }
}
