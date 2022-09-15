using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Identities;

namespace Vouchers.Application.UseCases
{
    public class AuthIdentityHandler<TMessage> : IHandler<TMessage>
    {
        private readonly IRepository<Login> _loginRepository;
        IAuthIdentityHandler<TMessage> _handler;
        ILoginService _loginService;

        public AuthIdentityHandler(IRepository<Login> loginRepository, IAuthIdentityHandler<TMessage> handler, ILoginService loginService)
        {
            _loginRepository = loginRepository;
            _handler = handler;
            _loginService = loginService;
        }

        public async Task HandleAsync(TMessage message, CancellationToken cancellation)
        {
            var loginName = _loginService.CurrentLoginName;
            var login = (await _loginRepository.GetByExpressionAsync(login => login.LoginName == loginName)).FirstOrDefault();

            if (login?.IdentityId is null)
                throw new NotRegisteredException();

            await _handler.HandleAsync(message, login.IdentityId, cancellation);
        }
    }

    public class AuthIdentityHandler<TRequest, TResponse> : IHandler<TRequest, TResponse>
    {
        private readonly IRepository<Login> _loginRepository;
        IAuthIdentityHandler<TRequest, TResponse> _handler;
        ILoginService _loginService;

        public AuthIdentityHandler(IRepository<Login> loginRepository, IAuthIdentityHandler<TRequest, TResponse> handler, ILoginService loginService)
        {
            _loginRepository = loginRepository;
            _handler = handler;
            _loginService = loginService;
        }

        public async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellation)
        {
            var loginName = _loginService.CurrentLoginName;
            var login = (await _loginRepository.GetByExpressionAsync(login => login.LoginName == loginName)).FirstOrDefault();

            if (login?.Identity is null)
                throw new NotRegisteredException();

            return await _handler.HandleAsync(request, login.IdentityId, cancellation);
        }
    }
}
