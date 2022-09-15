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
    public class CreateDomainAccountCommandHandler : IAuthIdentityHandler<CreateDomainAccountCommand, Guid>
    {

        private readonly IRepository<Domain> _domainRepository;
        private readonly IRepository<DomainAccount> _domainAccountRepository;
        private readonly IRepository<Account> _accountRepository;

        public CreateDomainAccountCommandHandler(IRepository<Domain> domainRepository, IRepository<DomainAccount> domainAccountRepository, IRepository<Account> accountRepository)
        {
            _domainRepository = domainRepository;
            _domainAccountRepository = domainAccountRepository;
            _accountRepository = accountRepository;
        }

        public async Task<Guid> HandleAsync(CreateDomainAccountCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var domain = _domainRepository.GetById(command.DomainId);
            if (domain is null)
                throw new ApplicationException("Domain does not exist");

            var account = Account.Create();
            await _accountRepository.AddAsync(account);

            var domainAccount = DomainAccount.Create(account.Id, authIdentityId, domain);
            
            if (domain.IsPublic)
            {
                domainAccount.IsConfirmed = true;
                domain.IncreaseMembersCount();
            }

            await _domainAccountRepository.AddAsync(domainAccount);
            return domainAccount.Id;
        }
    }
}
