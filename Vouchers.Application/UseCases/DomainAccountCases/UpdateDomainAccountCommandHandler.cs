using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Commands.DomainAccountCommands;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Domains;

namespace Vouchers.Application.UseCases.DomainAccountCases
{
    internal sealed class UpdateDomainAccountCommandHandler : IHandler<UpdateDomainAccountCommand>
    {
        private readonly IAuthIdentityProvider _authIdentityProvider;
        private readonly IRepository<DomainAccount,Guid> _domainAccountRepository;

        public UpdateDomainAccountCommandHandler(IAuthIdentityProvider authIdentityProvider, IRepository<DomainAccount,Guid> domainAccountRepository)
        {
            _domainAccountRepository = domainAccountRepository;
            _authIdentityProvider = authIdentityProvider;
        }

        public async Task HandleAsync(UpdateDomainAccountCommand command, CancellationToken cancellation)
        {
            var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

            var domainAccount = await _domainAccountRepository.GetByIdAsync(command.DomainAccountId);
            if (domainAccount is null)
                throw new ApplicationException(Properties.Resources.DomainAccountDoesNotExist);

            var authDomainAccount = (await _domainAccountRepository.GetByExpressionAsync(acc => acc.Domain.Id == domainAccount.Domain.Id && acc.IdentityId == authIdentityId)).FirstOrDefault();
            if (authDomainAccount is null)
                throw new InvalidOperationException(Properties.Resources.IdentityDoesNotHaveAccountInDomain);    

            if(command.IsConfirmed is not null && domainAccount.IsConfirmed != command.IsConfirmed)
            {
                if(!(authDomainAccount.IsAdmin || authDomainAccount.IsOwner))
                    throw new InvalidOperationException(Properties.Resources.OperationIsNotAllowed);

                domainAccount.IsConfirmed = command.IsConfirmed.Value;
            }
                
            if (command.IsIssuer is not null && domainAccount.IsIssuer != command.IsIssuer)
            {
                if (!(authDomainAccount.IsAdmin || authDomainAccount.IsOwner))
                    throw new InvalidOperationException(Properties.Resources.OperationIsNotAllowed);

                domainAccount.IsIssuer = command.IsIssuer.Value;
            }

            if (command.IsAdmin is not null && domainAccount.IsAdmin != command.IsAdmin)
            {
                if (!authDomainAccount.IsOwner)
                    throw new InvalidOperationException(Properties.Resources.OperationIsNotAllowed);

                domainAccount.IsAdmin = command.IsAdmin.Value;
            }

            await _domainAccountRepository.UpdateAsync(domainAccount);
        }
    }
}
