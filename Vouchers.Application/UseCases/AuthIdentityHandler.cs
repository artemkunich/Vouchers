using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;

namespace Vouchers.Application.UseCases
{
    public class AuthIdentityHandler<TRequest> : IHandler<TRequest>
    {
        private readonly ILoginRepository loginRepository;
        IAuthIdentityHandler<TRequest> handler;
        ILoginService loginService;

        public AuthIdentityHandler(ILoginRepository loginRepository, IAuthIdentityHandler<TRequest> handler, ILoginService loginService)
        {
            this.loginRepository = loginRepository;
            this.handler = handler;
            this.loginService = loginService;
        }

        public async Task HandleAsync(TRequest request, CancellationToken cancellation)
        {
            var loginName = loginService.CurrentLoginName;
            var identity = await loginRepository.GetByLoginNameAsync(loginName);

            if (identity?.Identity is null)
                throw new NotRegisteredException();

            await handler.HandleAsync(request, identity.Identity.Id, cancellation);
        }
    }

    public class AuthIdentityHandler<TRequest, TResponse> : IHandler<TRequest, TResponse>
    {
        private readonly ILoginRepository loginRepository;
        IAuthIdentityHandler<TRequest, TResponse> handler;
        ILoginService loginService;

        public AuthIdentityHandler(ILoginRepository loginRepository, IAuthIdentityHandler<TRequest, TResponse> handler, ILoginService loginService)
        {
            this.loginRepository = loginRepository;
            this.handler = handler;
            this.loginService = loginService;
        }

        public async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellation)
        {
            var loginName = loginService.CurrentLoginName;
            var identity = await loginRepository.GetByLoginNameAsync(loginName);

            if (identity?.Identity is null)
                throw new NotRegisteredException();

            return await handler.HandleAsync(request, identity.Identity.Id, cancellation);
        }
    }
}
