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
    public class CreateDomainOfferCommandHandler : IAuthIdentityHandler<CreateDomainOfferCommand, Guid>
    {
        private readonly IRepository<DomainOffer> _domainOfferRepository;

        public CreateDomainOfferCommandHandler(IRepository<DomainOffer> domainOfferRepository)
        {
            _domainOfferRepository = domainOfferRepository;
        }

        public async Task<Guid> HandleAsync(CreateDomainOfferCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var currency = Enum.Parse<Currency>(command.Currency);
            var currencyAmount = CurrencyAmount.Create(currency, command.Amount);
            var invoicePeriod = Enum.Parse<InvoicePeriod>(command.InvoicePeriod);

            var validTo = command.ValidTo ?? DateTime.MaxValue;

            var domainOffer = DomainOffer.Create(command.Name, command.Description, command.MaxSubscribersCount, currencyAmount, invoicePeriod, command.ValidFrom, validTo, command.MaxContractsPerIdentity);
            await _domainOfferRepository.AddAsync(domainOffer);

            return domainOffer.Id;
        }
    }
}
