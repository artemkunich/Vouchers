using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Commands.DomainAccountCommands;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;
using Vouchers.Primitives;

namespace Vouchers.Application.UseCases.DomainAccountCases;

internal sealed class CreateDomainAccountCommandHandler : IHandler<CreateDomainAccountCommand, Result<Guid>>
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

    public async Task<Result<Guid>> HandleAsync(CreateDomainAccountCommand command, CancellationToken cancellation)
    {
        var cultureInfo = _cultureInfoProvider.GetCultureInfo();
        return await (await _authIdentityProvider.GetAuthIdentityIdAsync())
            .ToResultAsync(async authIdentityId =>
            {
                var domain = await _domainRepository.GetByIdAsync(command.DomainId);
                return (await
                    (await Result.Create(domain)
                        .IfValueIsNullAddError(Errors.DomainDoesNotExist(cultureInfo))
                        .SetValue(_identifierProvider.CreateNewId())
                        .ToResult(accountId => Account.Create(accountId, _dateTimeProvider.CurrentDateTime()))
                        .ProcessAsync(account => _accountRepository.AddAsync(account)))
                    .Map(account =>
                        DomainAccount.Create(account.Id, authIdentityId, domain,
                            _dateTimeProvider.CurrentDateTime()))
                    .Process(domainAccount =>
                    {
                        if (domain.IsPublic)
                        {
                            domainAccount.IsConfirmed = true;
                            domain.IncreaseMembersCount();
                        }
                    })
                    .ProcessAsync(domainAccount => _domainAccountRepository.AddAsync(domainAccount)))
                .Map(domainAccount => domainAccount.Id);
            });
    }
}