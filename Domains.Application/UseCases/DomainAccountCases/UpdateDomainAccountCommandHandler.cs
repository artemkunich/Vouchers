using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identity.Abstractions;
using Vouchers.Domains.Application.Errors;
using Vouchers.Domains.Domain;

namespace Vouchers.Domains.Application.UseCases.DomainAccountCases;

internal sealed class UpdateDomainAccountCommandHandler : IRequestHandler<UpdateDomainAccountCommand, Unit>
{
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;
    private readonly IRepository<DomainAccount,Guid> _domainAccountRepository;

    public UpdateDomainAccountCommandHandler(IIdentityIdProvider<Guid> identityIdIdProvider, IRepository<DomainAccount,Guid> domainAccountRepository)
    {
        _domainAccountRepository = domainAccountRepository;
        _identityIdProvider = identityIdIdProvider;
    }

    public async Task<Result<Unit>> HandleAsync(UpdateDomainAccountCommand command, CancellationToken cancellation)
    {

        var authIdentityId = _identityIdProvider.GetIdentityId();

        var domainAccount = await _domainAccountRepository.GetByIdAsync(command.DomainAccountId, cancellation);
        if (domainAccount is null)
            return new DomainAccountDoesNotExistError();

        var authDomainAccount = (await _domainAccountRepository.GetByExpressionAsync(acc => acc.Domain.Id == domainAccount.Domain.Id && acc.IdentityId == authIdentityId, cancellation)).FirstOrDefault();
        if (authDomainAccount is null)
            return new IdentityDoesNotHaveAccountInDomainError();

        if(command.IsConfirmed is not null && domainAccount.IsConfirmed != command.IsConfirmed)
        {
            if (!(authDomainAccount.IsAdmin || authDomainAccount.IsOwner))
                return new OperationIsNotAllowedError();
            
            domainAccount.IsConfirmed = command.IsConfirmed.Value;
        }
                
        if (command.IsIssuer is not null && domainAccount.IsIssuer != command.IsIssuer)
        {
            if (!(authDomainAccount.IsAdmin || authDomainAccount.IsOwner))
                return new OperationIsNotAllowedError();

            domainAccount.IsIssuer = command.IsIssuer.Value;
        }

        if (command.IsAdmin is not null && domainAccount.IsAdmin != command.IsAdmin)
        {
            if (!authDomainAccount.IsOwner)
                return new OperationIsNotAllowedError();

            domainAccount.IsAdmin = command.IsAdmin.Value;
        }

        await _domainAccountRepository.UpdateAsync(domainAccount, cancellation);

        return Unit.Value;
    }
}