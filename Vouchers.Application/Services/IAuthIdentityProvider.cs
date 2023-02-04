using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Primitives;

namespace Vouchers.Application.Services;

public interface IAuthIdentityProvider
{
    Task<Result<Guid>> GetAuthIdentityIdAsync();
}