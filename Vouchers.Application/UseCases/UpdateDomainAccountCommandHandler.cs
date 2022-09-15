using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Commands;
using Vouchers.Application.Infrastructure;
using Vouchers.Core;
using Vouchers.Domains;

namespace Vouchers.Application.UseCases
{
    public class UpdateDomainAccountCommandHandler : IAuthIdentityHandler<UpdateDomainAccountCommand>
    {
        private readonly IRepository<DomainAccount> _domainAccountRepository;

        public UpdateDomainAccountCommandHandler(IRepository<DomainAccount> domainAccountRepository)
        {
            _domainAccountRepository = domainAccountRepository;
        }

        public async Task HandleAsync(UpdateDomainAccountCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var domainAccount = await _domainAccountRepository.GetByIdAsync(command.DomainAccountId);
            if (domainAccount is null)
                throw new ApplicationException("Domain account does not exist");

            var authDomainAccount = (await _domainAccountRepository.GetByExpressionAsync(acc => acc.Domain.Id == domainAccount.Domain.Id && acc.IdentityId == authIdentityId)).FirstOrDefault();
            if (authDomainAccount is null)
                throw new InvalidOperationException("Identity does not have account in domain");

            if (!authDomainAccount.IsAdmin)
            {
                if(authDomainAccount.Domain.Contract.PartyId != authDomainAccount.Id)
                    throw new InvalidOperationException("Operation is not allowed");
            }       

            if(domainAccount.IsConfirmed != command.IsConfirmed)
                domainAccount.IsConfirmed = command.IsConfirmed;

            await _domainAccountRepository.UpdateAsync(domainAccount);
        }
    }
}
