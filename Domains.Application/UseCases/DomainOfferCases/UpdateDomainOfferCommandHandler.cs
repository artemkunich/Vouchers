using System;
using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Vouchers.Domains.Application.Errors;
using Vouchers.Domains.Domain;

namespace Vouchers.Domains.Application.UseCases.DomainOfferCases;

internal sealed class UpdateDomainOfferCommandHandler : IRequestHandler<UpdateDomainOfferCommand,Unit>
{
    private readonly IRepository<DomainOffer,Guid> _domainOfferRepository;

    public UpdateDomainOfferCommandHandler(IRepository<DomainOffer,Guid> domainOfferRepository)
    {
        _domainOfferRepository = domainOfferRepository;
    }

    public async Task<Result<Unit>> HandleAsync(
        UpdateDomainOfferCommand command, CancellationToken cancellation)
    {
        var domainOffer = await _domainOfferRepository.GetByIdAsync(command.Id, cancellation);

        if (command.Terminate == true)
        {
            if(domainOffer.ValidFrom <= DateTime.Now)
                domainOffer.ValidTo = DateTime.Now;
            else
                domainOffer.ValidTo = domainOffer.ValidFrom;

            await _domainOfferRepository.UpdateAsync(domainOffer, cancellation);
            return Unit.Value;
        } 

        if(command.Description is not null && command.Description != domainOffer.Description)
        {
            domainOffer.Description = command.Description;
            await _domainOfferRepository.UpdateAsync(domainOffer, cancellation);
        }

        var requireUpdate = false;
        var result = Result.Create(Unit.Value);
        
        if (command.ValidFrom is not null && command.ValidFrom != domainOffer.ValidFrom)
            result
                .IfTrueAddError(_ => domainOffer.ValidFrom <= DateTime.Now, new ValidFromCannotBeModifiedOnActiveOffersError())
                .IfTrueAddError(_ => command.ValidFrom < DateTime.Now, new ValidFromCannotBeSetToPastError())
                .Process(() => domainOffer.ValidFrom = command.ValidFrom.Value)
                .Process(() => requireUpdate = true);

        if (command.ValidTo is not null && command.ValidTo != domainOffer.ValidTo)
            result
                .IfTrueAddError(_ => command.ValidTo < domainOffer.ValidFrom, new ValidToCannotBeLessThanValidFromError())
                .IfTrueAddError(_ => command.ValidTo < DateTime.Now, new ValidToCannotBeLessThanCurrentDatetimeError())
                .Process(() => domainOffer.ValidTo = command.ValidTo.Value)
                .Process(() => requireUpdate = true);

        if (requireUpdate)
            await _domainOfferRepository.UpdateAsync(domainOffer, cancellation);
        
        return result;
    }
}