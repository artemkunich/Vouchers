using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Identities.Domain;

namespace Vouchers.Application.ServiceProviders;

public class AuthIdentityProvider : IAuthIdentityProvider
{
    private readonly ILoginNameProvider _loginNameProvider;
    private readonly IRepository<Login, Guid> _loginRepository;

    public AuthIdentityProvider(ILoginNameProvider loginNameProvider, IRepository<Login, Guid> loginRepository)
    {
        _loginNameProvider = loginNameProvider;
        _loginRepository = loginRepository;
    }

    public async Task<Guid?> GetAuthIdentityIdAsync()
    {
        var loginName = _loginNameProvider.CurrentLoginName;
        var login = (await _loginRepository.GetByExpressionAsync(login => login.LoginName == loginName)).FirstOrDefault();
        
        return login?.IdentityId;
    }
}