using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Core;

namespace Vouchers.MediatR.Requests
{
    public class LoginsMQuery : AuthorizedRequest<IPaginatedEnumerable<LoginDto>>
    {
        public LoginsQuery AppQuery { get; set; }
    }
}
