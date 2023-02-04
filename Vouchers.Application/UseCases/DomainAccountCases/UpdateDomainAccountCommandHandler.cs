using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Commands.DomainAccountCommands;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Domains.Domain;
using Vouchers.Primitives;

namespace Vouchers.Application.UseCases.DomainAccountCases;

internal sealed class UpdateDomainAccountCommandHandler : IHandler<UpdateDomainAccountCommand, Result>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly ICultureInfoProvider _cultureInfoProvider;
    
    public UpdateDomainAccountCommandHandler(IAuthIdentityProvider authIdentityProvider, IRepository<DomainAccount,Guid> domainAccountRepository, ICultureInfoProvider cultureInfoProvider)
    {
        _domainAccountRepository = domainAccountRepository;
        _cultureInfoProvider = cultureInfoProvider;
        _authIdentityProvider = authIdentityProvider;
    }

    public async Task<Result> HandleAsync(UpdateDomainAccountCommand command, CancellationToken cancellation)
    {
        var cultureInfo = _cultureInfoProvider.GetCultureInfo();

        var authIdentityIdResult = await _authIdentityProvider.GetAuthIdentityIdAsync();
        var domainAccountResult = await authIdentityIdResult
            .MapAsync(_ => _domainAccountRepository.GetByIdAsync(command.DomainAccountId));
        domainAccountResult
            .IfValueIsNullAddError(Errors.DomainAccountDoesNotExist(cultureInfo));

        var authDomainAccountsResult = await domainAccountResult
            .MapAsync(domainAccount => _domainAccountRepository.GetByExpressionAsync(acc => acc.DomainId == domainAccount.DomainId && acc.IdentityId == authIdentityIdResult.Value));
        
        var authDomainAccountResult = authDomainAccountsResult.Map(authDomainAccounts => authDomainAccounts.FirstOrDefault());
        authDomainAccountResult
            .IfValueIsNullAddError(Errors.IdentityDoesNotHaveAccountInDomain(cultureInfo))
            .MergeResultErrors(authDomainAccount => UpdateDomainAccount(domainAccountResult.Value, authDomainAccount, command, cultureInfo));
        
        return await authDomainAccountResult
            .ProcessAsync(_ => _domainAccountRepository.UpdateAsync(domainAccountResult.Value));
        
    }

    private Result UpdateDomainAccount(DomainAccount domainAccount, DomainAccount authDomainAccount, UpdateDomainAccountCommand command, CultureInfo cultureInfo)
    {
        var result = Result.Create();
        if (command.IsConfirmed is not null && domainAccount.IsConfirmed != command.IsConfirmed)
            result
                .IfTrueAddError(!(authDomainAccount.IsAdmin || authDomainAccount.IsOwner),
                    Errors.OperationIsNotAllowed(cultureInfo))
                .Process(() => domainAccount.IsConfirmed = command.IsConfirmed.Value);

        if (command.IsIssuer is not null && domainAccount.IsIssuer != command.IsIssuer)
            result
                .IfTrueAddError(!(authDomainAccount.IsAdmin || authDomainAccount.IsOwner),
                    Errors.OperationIsNotAllowed(cultureInfo))
                .Process(() => domainAccount.IsIssuer = command.IsIssuer.Value);
        if (command.IsAdmin is not null && domainAccount.IsAdmin != command.IsAdmin)
            result
                .IfTrueAddError(!authDomainAccount.IsOwner, Errors.OperationIsNotAllowed(cultureInfo))
                .Process(() => domainAccount.IsAdmin = command.IsAdmin.Value);

        return result;
    }
}