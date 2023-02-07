using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Commands.DomainAccountCommands;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Domains.Domain;

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
        
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        if (authIdentityId is null)
            return Error.NotRegistered(cultureInfo);
        
        var domainAccount = await _domainAccountRepository.GetByIdAsync(command.DomainAccountId);
        if (domainAccount is null)
            return Error.DebtorAccountDoesNotExist(cultureInfo);

        var authDomainAccount = (await _domainAccountRepository.GetByExpressionAsync(acc => acc.Domain.Id == domainAccount.Domain.Id && acc.IdentityId == authIdentityId)).FirstOrDefault();
        if (authDomainAccount is null)
            return Error.IdentityDoesNotHaveAccountInDomain(cultureInfo);

        if(command.IsConfirmed is not null && domainAccount.IsConfirmed != command.IsConfirmed)
        {
            if (!(authDomainAccount.IsAdmin || authDomainAccount.IsOwner))
                return Error.OperationIsNotAllowed(cultureInfo);
            
            domainAccount.IsConfirmed = command.IsConfirmed.Value;
        }
                
        if (command.IsIssuer is not null && domainAccount.IsIssuer != command.IsIssuer)
        {
            if (!(authDomainAccount.IsAdmin || authDomainAccount.IsOwner))
                return Error.OperationIsNotAllowed(cultureInfo);

            domainAccount.IsIssuer = command.IsIssuer.Value;
        }

        if (command.IsAdmin is not null && domainAccount.IsAdmin != command.IsAdmin)
        {
            if (!authDomainAccount.IsOwner)
                return Error.OperationIsNotAllowed(cultureInfo);

            domainAccount.IsAdmin = command.IsAdmin.Value;
        }

        await _domainAccountRepository.UpdateAsync(domainAccount);

        return Result.Create();
    }
}