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
    public class UpdateDomainOfferCommandHandler : IAuthIdentityHandler<UpdateDomainOfferCommand>
    {
        private readonly IRepository<DomainOffer> _domainOfferRepository;

        public UpdateDomainOfferCommandHandler(IRepository<DomainOffer> domainOfferRepository)
        {
            _domainOfferRepository = domainOfferRepository;
        }

        public async Task HandleAsync(UpdateDomainOfferCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var domainOffer = await _domainOfferRepository.GetByIdAsync(command.Id);

            if (command.Terminate)
            {
                if(domainOffer.ValidFrom <= DateTime.Now)
                    domainOffer.ValidTo = DateTime.Now;
                else
                    domainOffer.ValidTo = domainOffer.ValidFrom;

                await _domainOfferRepository.UpdateAsync(domainOffer);
                return;
            } 

            if(command.Description != domainOffer.Description)
            {
                domainOffer.Description = command.Description;
                await _domainOfferRepository.UpdateAsync(domainOffer);
            }

            if (command.ValidFrom is not null && command.ValidFrom != domainOffer.ValidFrom)
            {
                if (domainOffer.ValidFrom <= DateTime.Now)
                    throw new ApplicationException("ValidFrom cannot be modified on active offers");

                if (command.ValidFrom < DateTime.Now)
                    throw new ApplicationException("Cannot set validFrom to past");

                domainOffer.ValidFrom = command.ValidFrom.Value;
            }

            if (command.ValidTo is not null && command.ValidTo != domainOffer.ValidTo)
            {
                if (command.ValidTo < domainOffer.ValidFrom)
                    throw new ApplicationException("ValidTo cannot be less than ValidFrom");

                if (command.ValidTo < DateTime.Now)
                    throw new ApplicationException("ValidTo cannot be less than now");

                domainOffer.ValidTo = command.ValidTo.Value;

                await _domainOfferRepository.UpdateAsync(domainOffer);
            }
        }
    }
}
