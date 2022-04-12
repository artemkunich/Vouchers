using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vouchers.Application.Queries
{
    public class LoginsQuery : Query
    {
        public string LoginName { get; set; }

        public string IdentityName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
