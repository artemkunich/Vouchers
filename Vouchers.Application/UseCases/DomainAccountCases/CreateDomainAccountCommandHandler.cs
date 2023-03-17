using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Commands.DomainAccountCommands;
using Vouchers.Application.DomainEvents;
using Vouchers.Application.Dtos;
using Vouchers.Application.Errors;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;

namespace Vouchers.Application.UseCases.DomainAccountCases;

internal sealed class CreateDomainAccountCommandHandler : IRequestHandler<CreateDomainAccountCommand, IdDto<Guid>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IReadOnlyRepository<Domain,Guid> _domainRepository;
    private readonly IRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IEventDispatcher _eventDispatcher;

    public CreateDomainAccountCommandHandler(
        IAuthIdentityProvider authIdentityProvider, 
        IReadOnlyRepository<Domain,Guid> domainRepository, 
        IRepository<DomainAccount,Guid> domainAccountRepository, 
        IIdentifierProvider<Guid> identifierProvider, 
        IDateTimeProvider dateTimeProvider, 
        IEventDispatcher eventDispatcher)
    {
        _authIdentityProvider = authIdentityProvider;
        _domainRepository = domainRepository;
        _domainAccountRepository = domainAccountRepository;
        _identifierProvider = identifierProvider;
        _dateTimeProvider = dateTimeProvider;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<Result<IdDto<Guid>>> HandleAsync(CreateDomainAccountCommand command, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

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

        var result = await _eventDispatcher.DispatchAsync(new DomainAccountCreatedEvent(
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
            
        return new IdDto<Guid>(domainAccount.Id);
    }
}