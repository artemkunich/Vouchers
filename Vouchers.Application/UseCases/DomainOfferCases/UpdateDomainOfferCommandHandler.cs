using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Commands.DomainOfferCommands;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Core;
using Vouchers.Domains;

namespace Vouchers.Application.UseCases.DomainOfferCases
{
    internal sealed class UpdateDomainOfferCommandHandler : IHandler<UpdateDomainOfferCommand>
    {
        private readonly IAuthIdentityProvider _authIdentityProvider;
        private readonly IRepository<DomainOffer,Guid> _domainOfferRepository;

        public UpdateDomainOfferCommandHandler(IAuthIdentityProvider authIdentityProvider, IRepository<DomainOffer,Guid> domainOfferRepository)
        {
            _authIdentityProvider = authIdentityProvider;
            _domainOfferRepository = domainOfferRepository;
        }

        public async Task HandleAsync(UpdateDomainOfferCommand command, CancellationToken cancellation)
        {
            var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

            var domainOffer = await _domainOfferRepository.GetByIdAsync(command.Id);

            if (command.Terminate == true)
            {
                if(domainOffer.ValidFrom <= DateTime.Now)
                    domainOffer.ValidTo = DateTime.Now;
                else
                    domainOffer.ValidTo = domainOffer.ValidFrom;

                await _domainOfferRepository.UpdateAsync(domainOffer);
                return;
            } 

            if(command.Description is not null && command.Description != domainOffer.Description)
            {
                domainOffer.Description = command.Description;
                await _domainOfferRepository.UpdateAsync(domainOffer);
            }

            if (command.ValidFrom is not null && command.ValidFrom != domainOffer.ValidFrom)
            {
                if (domainOffer.ValidFrom <= DateTime.Now)
                    throw new ApplicationException(Properties.Resources.ValidFromCannotBeModifiedOnActiveOffers);

                if (command.ValidFrom < DateTime.Now)
                    throw new ApplicationException(Properties.Resources.ValidFromCannotBeSetToPast);

                domainOffer.ValidFrom = command.ValidFrom.Value;
            }

            if (command.ValidTo is not null && command.ValidTo != domainOffer.ValidTo)
            {
                if (command.ValidTo < domainOffer.ValidFrom)
                    throw new ApplicationException(Properties.Resources.ValidToCannotBeLessThanValidFrom);

                if (command.ValidTo < DateTime.Now)
                    throw new ApplicationException(Properties.Resources.ValidToCannotBeLessThanCurrentDatetime);

                domainOffer.ValidTo = command.ValidTo.Value;

                await _domainOfferRepository.UpdateAsync(domainOffer);
            }
        }
    }
}
