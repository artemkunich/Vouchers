using System;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Domains.Application.DomainEvents;
using Vouchers.Domains.Application.Errors;
using Vouchers.Domains.Domain;

namespace Vouchers.Domains.Application.UseCases.DomainAccountCases;

internal sealed class CreateDomainAccountCommandHandler : IRequestHandler<CreateDomainAccountCommand, Dtos.IdDto<Guid>>
{
    private readonly IIdentityIdProvider _identityIdProvider;
    private readonly IReadOnlyRepository<Domain.Domain,Guid> _domainRepository;
    private readonly IRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public CreateDomainAccountCommandHandler(
        IIdentityIdProvider identityIdProvider, 
        IReadOnlyRepository<Domain.Domain,Guid> domainRepository, 
        IRepository<DomainAccount,Guid> domainAccountRepository, 
        IIdentifierProvider<Guid> identifierProvider, 
        IDateTimeProvider dateTimeProvider, 
        IDomainEventDispatcher domainEventDispatcher)
    {
        _identityIdProvider = identityIdProvider;
        _domainRepository = domainRepository;
        _domainAccountRepository = domainAccountRepository;
        _identifierProvider = identifierProvider;
        _dateTimeProvider = dateTimeProvider;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<Result<Dtos.IdDto<Guid>>> HandleAsync(CreateDomainAccountCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.CurrentIdentityId;

        var domain = await _domainRepository.GetByIdAsync(command.DomainId);
        if (domain is null)
            return new DomainDoesNotExistError();

        var domainAccountId = _identifierProvider.CreateNewId();
        var domainAccountCreatedDatetime = _dateTimeProvider.CurrentDateTime();
        var domainAccount = DomainAccount.Create(domainAccountId, authIdentityId, domain, domainAccountCreatedDatetime);
            
        if (domain.IsPublic)
        {
            domainAccount.IsConfirmed = true;
            domain.IncreaseMembersCount();
        }

        await _domainAccountRepository.AddAsync(domainAccount);

        var result = await _domainEventDispatcher.DispatchAsync(new DomainAccountCreatedDomainEvent(
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