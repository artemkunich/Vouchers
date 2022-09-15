using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.WPF.Model.Specifications
{
    public class UserAccountId : UserAccountSpecification
    {
        public string Id { get; }

        public UserAccountId(string id)
        {
            Id = id;
        }
    }
}
