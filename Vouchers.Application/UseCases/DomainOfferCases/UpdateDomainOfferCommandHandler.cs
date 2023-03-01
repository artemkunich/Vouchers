using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Commands.DomainOfferCommands;
using Vouchers.Application.Errors;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Domains.Domain;
using Unit = Vouchers.Core.Domain.Unit;

namespace Vouchers.Application.UseCases.DomainOfferCases;

internal sealed class UpdateDomainOfferCommandHandler : IRequestHandler<UpdateDomainOfferCommand,Abstractions.Unit>
{
    private readonly IRepository<DomainOffer,Guid> _domainOfferRepository;

    public UpdateDomainOfferCommandHandler(IRepository<DomainOffer,Guid> domainOfferRepository)
    {
        _domainOfferRepository = domainOfferRepository;
    }

    public async Task<Result<Abstractions.Unit>> HandleAsync(UpdateDomainOfferCommand command, CancellationToken cancellation)
    {
        var domainOffer = await _domainOfferRepository.GetByIdAsync(command.Id);

        if (command.Terminate == true)
        {
            if(domainOffer.ValidFrom <= DateTime.Now)
                domainOffer.ValidTo = DateTime.Now;
            else
                domainOffer.ValidTo = domainOffer.ValidFrom;

            await _domainOfferRepository.UpdateAsync(domainOffer);
            return Abstractions.Unit.Value;
        } 

        if(command.Description is not null && command.Description != domainOffer.Description)
        {
            domainOffer.Description = command.Description;
            await _domainOfferRepository.UpdateAsync(domainOffer);
        }

        var requireUpdate = false;
        var result = Result.Create(Abstractions.Unit.Value);
        
        if (command.ValidFrom is not null && command.ValidFrom != domainOffer.ValidFrom)
            result
                .IfTrueAddError(domainOffer.ValidFrom <= DateTime.Now, new ValidFromCannotBeModifiedOnActiveOffersError())
                .IfTrueAddError(command.ValidFrom < DateTime.Now, new ValidFromCannotBeSetToPastError())
                .Process(() => domainOffer.ValidFrom = command.ValidFrom.Value)
                .Process(() => requireUpdate = true);

        if (command.ValidTo is not null && command.ValidTo != domainOffer.ValidTo)
            result
                .IfTrueAddError(command.ValidTo < domainOffer.ValidFrom, new ValidToCannotBeLessThanValidFromError())
                .IfTrueAddError(command.ValidTo < DateTime.Now, new ValidToCannotBeLessThanCurrentDatetimeError())
                .Process(() => domainOffer.ValidTo = command.ValidTo.Value)
                .Process(() => requireUpdate = true);

        if (requireUpdate)
            await _domainOfferRepository.UpdateAsync(domainOffer);
        
        return result;
    }
}