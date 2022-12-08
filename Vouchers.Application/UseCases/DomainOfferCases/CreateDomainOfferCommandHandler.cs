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
    internal sealed class CreateDomainOfferCommandHandler : IHandler<CreateDomainOfferCommand, Guid>
    {
        private readonly IAuthIdentityProvider _authIdentityProvider;
        private readonly IRepository<DomainOffer> _domainOfferRepository;

        public CreateDomainOfferCommandHandler(IAuthIdentityProvider authIdentityProvider, IRepository<DomainOffer> domainOfferRepository)
        {
            _authIdentityProvider = authIdentityProvider;
            _domainOfferRepository = domainOfferRepository;
        }

        public async Task<Guid> HandleAsync(CreateDomainOfferCommand command, CancellationToken cancellation)
        {
            var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

            var currency = Enum.Parse<Currency>(command.Currency);
            var currencyAmount = CurrencyAmount.Create(currency, command.Amount);
            var invoicePeriod = Enum.Parse<InvoicePeriod>(command.InvoicePeriod);

            var validTo = command.ValidTo ?? DateTime.MaxValue;

            var domainOffer = DomainOffer.Create(command.Name, command.Description, command.MaxMembersCount, currencyAmount, invoicePeriod, command.ValidFrom, validTo, command.MaxContractsPerIdentity);
            await _domainOfferRepository.AddAsync(domainOffer);

            return domainOffer.Id;
        }
    }
}
