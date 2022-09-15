using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Core;

namespace Vouchers.MediatR.Requests
{
    public class IdentityDetailMQuery : AuthorizedRequest<IdentityDetailDto> 
    {
    }
}
