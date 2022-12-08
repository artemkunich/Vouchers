using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vouchers.Application.Services
{
    public interface IAuthIdentityProvider
    {
        Task<Guid> GetAuthIdentityIdAsync();
    }
}
