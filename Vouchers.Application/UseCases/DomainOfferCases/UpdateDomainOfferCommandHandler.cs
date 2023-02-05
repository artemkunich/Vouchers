using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Commands.DomainOfferCommands;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;

namespace Vouchers.Application.UseCases.DomainOfferCases;

internal sealed class UpdateDomainOfferCommandHandler : IHandler<UpdateDomainOfferCommand,Result>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IRepository<DomainOffer,Guid> _domainOfferRepository;
    private readonly ICultureInfoProvider _cultureInfoProvider;
    
    public UpdateDomainOfferCommandHandler(IAuthIdentityProvider authIdentityProvider, IRepository<DomainOffer,Guid> domainOfferRepository, ICultureInfoProvider cultureInfoProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _domainOfferRepository = domainOfferRepository;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task<Result> HandleAsync(UpdateDomainOfferCommand command, CancellationToken cancellation)
    {
        var cultureInfo = _cultureInfoProvider.GetCultureInfo();
        
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        if (authIdentityId is null)
            return Error.NotAuthorized(cultureInfo);
        
        var domainOffer = await _domainOfferRepository.GetByIdAsync(command.Id);

        if (command.Terminate == true)
        {
            if(domainOffer.ValidFrom <= DateTime.Now)
                domainOffer.ValidTo = DateTime.Now;
            else
                domainOffer.ValidTo = domainOffer.ValidFrom;

            await _domainOfferRepository.UpdateAsync(domainOffer);
            return Result.Create();
        } 

        if(command.Description is not null && command.Description != domainOffer.Description)
        {
            domainOffer.Description = command.Description;
            await _domainOfferRepository.UpdateAsync(domainOffer);
        }

        var requireUpdate = false;
        var result = Result.Create();
        
        if (command.ValidFrom is not null && command.ValidFrom != domainOffer.ValidFrom)
            result
                .IfTrueAddError(domainOffer.ValidFrom <= DateTime.Now, Error.ValidFromCannotBeModifiedOnActiveOffers(cultureInfo))
                .IfTrueAddError(command.ValidFrom < DateTime.Now, Error.ValidFromCannotBeSetToPast(cultureInfo))
                .Process(() => domainOffer.ValidFrom = command.ValidFrom.Value)
                .Process(() => requireUpdate = true);

        if (command.ValidTo is not null && command.ValidTo != domainOffer.ValidTo)
            result
                .IfTrueAddError(command.ValidTo < domainOffer.ValidFrom, Error.ValidToCannotBeLessThanValidFrom(cultureInfo))
                .IfTrueAddError(command.ValidTo < DateTime.Now, Error.ValidToCannotBeLessThanCurrentDatetime(cultureInfo))
                .Process(() => domainOffer.ValidTo = command.ValidTo.Value)
                .Process(() => requireUpdate = true);

        if (requireUpdate)
            await _domainOfferRepository.UpdateAsync(domainOffer);
        
        return result;
    }
}