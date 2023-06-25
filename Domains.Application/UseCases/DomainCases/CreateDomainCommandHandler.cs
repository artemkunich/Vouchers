using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identifier;
using Akunich.Extensions.Identity.Abstractions;
using Akunich.Extensions.Time;
using Vouchers.Domains.Application.DomainEvents;
using Vouchers.Domains.Application.Errors;
using Vouchers.Domains.Domain;

namespace Vouchers.Domains.Application.UseCases.DomainCases;

internal sealed class CreateDomainCommandHandler : IRequestHandler<CreateDomainCommand, Dtos.IdDto<Guid>>
{
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;
    private readonly IReadOnlyRepository<DomainOffer,Guid> _domainOfferRepository;
    private readonly IReadOnlyRepository<DomainOffersPerIdentityCounter,Guid> _domainOffersPerIdentityCounterRepository;
    private readonly IRepository<DomainContract,Guid> _domainContractRepository;
    private readonly IRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly ITimeProvider _timeProvider;
    private readonly INotificationDispatcher _notificationDispatcher;
    
    public CreateDomainCommandHandler(
        IIdentityIdProvider<Guid> identityIdProvider, 
        IReadOnlyRepository<DomainOffer,Guid> domainOfferRepository, 
        IReadOnlyRepository<DomainOffersPerIdentityCounter,Guid> domainOffersPerIdentityCounterRepository, 
        IRepository<DomainContract,Guid> domainContractRepository, 
        IRepository<DomainAccount,Guid> domainAccountRepository, 
        IIdentifierProvider<Guid> identifierProvider,
        ITimeProvider timeProvider,
        INotificationDispatcher notificationDispatcher)
    {
        _identityIdProvider = identityIdProvider;
        _domainOfferRepository = domainOfferRepository;
        _domainOffersPerIdentityCounterRepository = domainOffersPerIdentityCounterRepository;
        _domainContractRepository = domainContractRepository;
        _domainAccountRepository = domainAccountRepository;
        _identifierProvider = identifierProvider;
        _timeProvider = timeProvider;
        _notificationDispatcher = notificationDispatcher;
    }

    public async Task<Result<Dtos.IdDto<Guid>>> HandleAsync(CreateDomainCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.GetIdentityId();

        var domainOffer = await _domainOfferRepository.GetByIdAsync(command.OfferId);

        DomainOffersPerIdentityCounter domainOffersPerIdentityCounter = null;
        if (domainOffer.MaxContractsPerIdentity is not null)
        {
            domainOffersPerIdentityCounter = (await _domainOffersPerIdentityCounterRepository.GetByExpressionAsync(counter => counter.OfferId == domainOffer.Id && counter.IdentityId == authIdentityId)).FirstOrDefault();
            if (domainOffersPerIdentityCounter is null)
            {
                var domainOffersPerIdentityCounterId = _identifierProvider.CreateNewId();
                domainOffersPerIdentityCounter = DomainOffersPerIdentityCounter.Create(domainOffersPerIdentityCounterId, domainOffer, authIdentityId, 0);
                domainOffersPerIdentityCounter.AddContract();
            }
            else if (domainOffersPerIdentityCounter.Counter > domainOffer.MaxContractsPerIdentity)
            {
                return new MaxCountOfContractsExceededError();
            }
            else
            {
                domainOffersPerIdentityCounter.AddContract();
            }
        }

        var domainContract = DomainContract.Create(_identifierProvider.CreateNewId(), domainOffer, domainOffersPerIdentityCounter, authIdentityId, command.DomainName, _timeProvider.GetUtcNow());
            
        if (domainOffer.Amount.IsZero)
        {
            
            var domainId = _identifierProvider.CreateNewId();
            var domain = Domain.Domain.Create(domainId, domainContract, 0, 0, _timeProvider.GetUtcNow());
            domain.IncreaseMembersCount();

            var domainAccountId = _identifierProvider.CreateNewId();
            var domainAccountCreatedDatetime = _timeProvider.GetUtcNow();
            var domainAccount = DomainAccount.Create(domainAccountId, authIdentityId, domain, domainAccountCreatedDatetime);
            domainAccount.IsIssuer = true;
            domainAccount.IsConfirmed = true;
                
            await _domainAccountRepository.AddAsync(domainAccount, cancellation);
            var domainAcountCreatedEvent = new DomainAccountCreatedNotification(
                domainAccountId,
                authIdentityId,
                domain.Id,
                domainAccountCreatedDatetime,
                domainAccount.IsIssuer,
                domainAccount.IsAdmin,
                domainAccount.IsOwner,
                domainAccount.IsConfirmed
            );
            
            var result = await _notificationDispatcher.DispatchAsync(domainAcountCreatedEvent, cancellation);
            if (result.IsFailure)
                return result.Errors;
            
            return new Dtos.IdDto<Guid>(domainContract.Id);

        }

        await _domainContractRepository.AddAsync(domainContract, cancellation);
        return new Dtos.IdDto<Guid>(domainContract.Id);
       
    }
}