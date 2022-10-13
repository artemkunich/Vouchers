using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Commands.DomainCommands;
using Vouchers.Application.Infrastructure;
using Vouchers.Core;
using Vouchers.Domains;

namespace Vouchers.Application.UseCases.DomainCases
{
    internal sealed class CreateDomainCommandHandler : IAuthIdentityHandler<CreateDomainCommand, Guid?>
    {       
        private readonly IRepository<DomainOffer> _domainOfferRepository;
        private readonly IRepository<DomainOffersPerIdentityCounter> _domainOffersPerIdentityCounterRepository;
        private readonly IRepository<DomainContract> _domainContractRepository;
        private readonly IRepository<DomainAccount> _domainAccountRepository;
        private readonly IRepository<Account> _accountRepository;

        public CreateDomainCommandHandler(IRepository<DomainOffer> domainOfferRepository, IRepository<DomainOffersPerIdentityCounter> domainOffersPerIdentityCounterRepository, 
            IRepository<DomainContract> domainContractRepository, IRepository<DomainAccount> domainAccountRepository, IRepository<Account> accountRepository)
        {
            _domainOfferRepository = domainOfferRepository;
            _domainOffersPerIdentityCounterRepository = domainOffersPerIdentityCounterRepository;
            _domainContractRepository = domainContractRepository;
            _domainAccountRepository = domainAccountRepository;
            _accountRepository = accountRepository;
        }

        public async Task<Guid?> HandleAsync(CreateDomainCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var domainOffer = _domainOfferRepository.GetById(command.OfferId);

            DomainOffersPerIdentityCounter domainOffersPerIdentityCounter = null;
            if (domainOffer.MaxContractsPerIdentity is not null)
            {
                domainOffersPerIdentityCounter = (await _domainOffersPerIdentityCounterRepository.GetByExpressionAsync(counter => counter.OfferId == domainOffer.Id && counter.IdentityId == authIdentityId)).FirstOrDefault();
                if (domainOffersPerIdentityCounter is null)
                {
                    domainOffersPerIdentityCounter = DomainOffersPerIdentityCounter.Create(domainOffer, authIdentityId);
                    domainOffersPerIdentityCounter.AddContract();
                }
                else if (domainOffersPerIdentityCounter.Counter > domainOffer.MaxContractsPerIdentity)
                {
                    throw new ApplicationException("Max count of contracts is exceeded");
                }
                else
                {
                    domainOffersPerIdentityCounter.AddContract();
                }
            }

            var domainContract = DomainContract.Create(domainOffer, domainOffersPerIdentityCounter, authIdentityId, command.DomainName);
            
            if (domainOffer.Amount.Amount == 0)
            {
                var account = Account.Create();
                await _accountRepository.AddAsync(account);

                var domain = Domain.Create(domainContract, 0);
                domain.IncreaseMembersCount();

                var domainAccount = DomainAccount.Create(account.Id, authIdentityId, domain);
                domainAccount.IsIssuer = true;
                domainAccount.IsConfirmed = true;
                
                await _domainAccountRepository.AddAsync(domainAccount);
                return domain.Id;

            }
            else
            {
                await _domainContractRepository.AddAsync(domainContract);
                return null;
            }         
        }
    }
}
