using System;
using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identifier;
using Vouchers.Domains.Domain;

namespace Vouchers.Domains.Application.UseCases.DomainOfferCases;

internal sealed class CreateDomainOfferCommandHandler : IRequestHandler<CreateDomainOfferCommand, Dtos.IdDto<Guid>>
{
    private readonly IRepository<DomainOffer,Guid> _domainOfferRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    
    public CreateDomainOfferCommandHandler(
        IRepository<DomainOffer,Guid> domainOfferRepository, 
        IIdentifierProvider<Guid> identifierProvider)
    {
        _domainOfferRepository = domainOfferRepository;
        _identifierProvider = identifierProvider;
    }

    public async Task<Result<Dtos.IdDto<Guid>>> HandleAsync(CreateDomainOfferCommand command, CancellationToken cancellation)
    {
        var currency = Enum.Parse<Currency>(command.Currency);
        var currencyAmount = CurrencyAmount.Create(currency, command.Amount);
        var invoicePeriod = Enum.Parse<InvoicePeriod>(command.InvoicePeriod);

        var validTo = command.ValidTo ?? DateTime.MaxValue;

        var domainOfferId = _identifierProvider.CreateNewId();
        var domainOffer = DomainOffer.Create(domainOfferId, command.Name, command.Description, command.MaxMembersCount, 
            currencyAmount, invoicePeriod, command.ValidFrom, validTo, command.MaxContractsPerIdentity);
        await _domainOfferRepository.AddAsync(domainOffer, cancellation);

        return new Dtos.IdDto<Guid>(domainOffer.Id);
    }
}