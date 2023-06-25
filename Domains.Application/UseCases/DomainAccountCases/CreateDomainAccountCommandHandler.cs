using System;
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

namespace Vouchers.Domains.Application.UseCases.DomainAccountCases;

internal sealed class CreateDomainAccountCommandHandler : IRequestHandler<CreateDomainAccountCommand, Dtos.IdDto<Guid>>
{
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;
    private readonly IReadOnlyRepository<Domain.Domain,Guid> _domainRepository;
    private readonly IRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly ITimeProvider _timeProvider;
    private readonly INotificationDispatcher _notificationDispatcher;

    public CreateDomainAccountCommandHandler(
        IIdentityIdProvider<Guid> identityIdProvider, 
        IReadOnlyRepository<Domain.Domain,Guid> domainRepository, 
        IRepository<DomainAccount,Guid> domainAccountRepository, 
        IIdentifierProvider<Guid> identifierProvider, 
        ITimeProvider timeProvider, 
        INotificationDispatcher notificationDispatcher)
    {
        _identityIdProvider = identityIdProvider;
        _domainRepository = domainRepository;
        _domainAccountRepository = domainAccountRepository;
        _identifierProvider = identifierProvider;
        _timeProvider = timeProvider;
        _notificationDispatcher = notificationDispatcher;
    }

    public async Task<Result<Dtos.IdDto<Guid>>> HandleAsync(CreateDomainAccountCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.GetIdentityId();

        var domain = await _domainRepository.GetByIdAsync(command.DomainId);
        if (domain is null)
            return new DomainDoesNotExistError();

        var domainAccountId = _identifierProvider.CreateNewId();
        var domainAccountCreatedDatetime = _timeProvider.GetUtcNow();
        var domainAccount = DomainAccount.Create(domainAccountId, authIdentityId, domain, domainAccountCreatedDatetime);
            
        if (domain.IsPublic)
        {
            domainAccount.IsConfirmed = true;
            domain.IncreaseMembersCount();
        }

        await _domainAccountRepository.AddAsync(domainAccount, cancellation);

        var result = await _notificationDispatcher.DispatchAsync(new DomainAccountCreatedNotification(
            domainAccountId,
            authIdentityId,
            domain.Id,
            domainAccountCreatedDatetime,
            domainAccount.IsIssuer,
            domainAccount.IsAdmin,
            domainAccount.IsOwner,
            domainAccount.IsConfirmed
        ), cancellation);

        if (result.IsFailure)
            return result.Errors;
            
        return new Dtos.IdDto<Guid>(domainAccount.Id);
    }
}