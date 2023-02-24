using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Commands.DomainOfferCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;

namespace Vouchers.Application.UseCases.DomainOfferCases;

internal sealed class CreateDomainOfferCommandHandler : IHandler<CreateDomainOfferCommand, IdDto<Guid>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IRepository<DomainOffer,Guid> _domainOfferRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly ICultureInfoProvider _cultureInfoProvider;
    public CreateDomainOfferCommandHandler(IAuthIdentityProvider authIdentityProvider, IRepository<DomainOffer,Guid> domainOfferRepository, IIdentifierProvider<Guid> identifierProvider, ICultureInfoProvider cultureInfoProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _domainOfferRepository = domainOfferRepository;
        _identifierProvider = identifierProvider;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task<Result<IdDto<Guid>>> HandleAsync(CreateDomainOfferCommand command, CancellationToken cancellation)
    {
        var currency = Enum.Parse<Currency>(command.Currency);
        var currencyAmount = CurrencyAmount.Create(currency, command.Amount);
        var invoicePeriod = Enum.Parse<InvoicePeriod>(command.InvoicePeriod);

        var validTo = command.ValidTo ?? DateTime.MaxValue;

        var domainOfferId = _identifierProvider.CreateNewId();
        var domainOffer = DomainOffer.Create(domainOfferId, command.Name, command.Description, command.MaxMembersCount, 
            currencyAmount, invoicePeriod, command.ValidFrom, validTo, command.MaxContractsPerIdentity);
        await _domainOfferRepository.AddAsync(domainOffer);

        return new IdDto<Guid>(domainOffer.Id);
    }
}