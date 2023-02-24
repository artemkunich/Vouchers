using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Commands.DomainAccountCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;

namespace Vouchers.Application.UseCases.DomainAccountCases;

internal sealed class CreateDomainAccountCommandHandler : IHandler<CreateDomainAccountCommand, IdDto<Guid>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IReadOnlyRepository<Domain,Guid> _domainRepository;
    private readonly IRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IRepository<Account,Guid> _accountRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICultureInfoProvider _cultureInfoProvider;

    public CreateDomainAccountCommandHandler(IAuthIdentityProvider authIdentityProvider, IReadOnlyRepository<Domain,Guid> domainRepository, 
        IRepository<DomainAccount,Guid> domainAccountRepository, IRepository<Account,Guid> accountRepository, IIdentifierProvider<Guid> identifierProvider, IDateTimeProvider dateTimeProvider, ICultureInfoProvider cultureInfoProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _domainRepository = domainRepository;
        _domainAccountRepository = domainAccountRepository;
        _accountRepository = accountRepository;
        _identifierProvider = identifierProvider;
        _dateTimeProvider = dateTimeProvider;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task<Result<IdDto<Guid>>> HandleAsync(CreateDomainAccountCommand command, CancellationToken cancellation)
    {
        var cultureInfo = _cultureInfoProvider.GetCultureInfo();
        
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var domain = await _domainRepository.GetByIdAsync(command.DomainId);
        if (domain is null)
            return Error.DomainDoesNotExist(cultureInfo);

        var accountId = _identifierProvider.CreateNewId();
        var account = Account.Create(accountId, _dateTimeProvider.CurrentDateTime());
        await _accountRepository.AddAsync(account);

        var domainAccount = DomainAccount.Create(account.Id, authIdentityId, domain, _dateTimeProvider.CurrentDateTime());
            
        if (domain.IsPublic)
        {
            domainAccount.IsConfirmed = true;
            domain.IncreaseMembersCount();
        }

        await _domainAccountRepository.AddAsync(domainAccount);
        return new IdDto<Guid>(domainAccount.Id);
    }
}