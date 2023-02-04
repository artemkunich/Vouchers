using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Identities.Domain;
using Vouchers.Primitives;

namespace Vouchers.Application.ServiceProviders;

public class AuthIdentityProvider : IAuthIdentityProvider
{
    private readonly ILoginNameProvider _loginNameProvider;
    private readonly IRepository<Login, Guid> _loginRepository;
    private readonly ICultureInfoProvider _cultureInfoProvider;
    public AuthIdentityProvider(ILoginNameProvider loginNameProvider, IRepository<Login, Guid> loginRepository, ICultureInfoProvider cultureInfoProvider)
    {
        _loginNameProvider = loginNameProvider;
        _loginRepository = loginRepository;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task<Result<Guid>> GetAuthIdentityIdAsync()
    {
        var currentCulture = _cultureInfoProvider.GetCultureInfo();
        var loginName = _loginNameProvider.CurrentLoginName;
        return Result.Create()
            .IfTrueAddError(loginName is null, Errors.NotAuthorized(currentCulture))
            .SetValue((await _loginRepository.GetByExpressionAsync(login => login.LoginName == loginName))
                .FirstOrDefault())
            .IfTrueAddError(login => login?.IdentityId is null, Errors.NotRegisteredException(currentCulture))
            .Map(login => login.IdentityId);
    }
}