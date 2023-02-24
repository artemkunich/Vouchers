using System;
using Vouchers.Core.Domain;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Application.Abstractions;
using Vouchers.Domains.Domain;
using Vouchers.Application.Commands.HolderTransactionRequestCommands;
using Vouchers.Application.Services;

namespace Vouchers.Application.UseCases.HolderTransactionRequestCases;

internal sealed class DeleteHolderTransactionRequestCommandHandler : IHandler<DeleteHolderTransactionRequestCommand, Abstractions.Unit>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IReadOnlyRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IRepository<HolderTransactionRequest,Guid> _holderTransactionRequestRepository;
    private readonly ICultureInfoProvider _cultureInfoProvider;
    
    public DeleteHolderTransactionRequestCommandHandler(IAuthIdentityProvider authIdentityProvider, 
        IReadOnlyRepository<DomainAccount,Guid> domainAccountRepository, IRepository<HolderTransactionRequest,Guid> holderTransactionRequestRepository, ICultureInfoProvider cultureInfoProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _domainAccountRepository = domainAccountRepository;
        _holderTransactionRequestRepository = holderTransactionRequestRepository;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task<Result<Abstractions.Unit>> HandleAsync(DeleteHolderTransactionRequestCommand command, CancellationToken cancellation)
    {
        var cultureInfo = _cultureInfoProvider.GetCultureInfo();
        
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var transactionRequest = await _holderTransactionRequestRepository.GetByIdAsync(command.HolderTransactionRequestId);
        if (transactionRequest is null)
            return Error.TransactionRequestIsNotFound(cultureInfo);

        if (transactionRequest.TransactionId is not null)
            return Error.TransactionRequestIsAlreadyPerformed(cultureInfo);

        var debtorDomainAccount = await _domainAccountRepository.GetByIdAsync(transactionRequest.DebtorAccountId);
        if(debtorDomainAccount?.IdentityId != authIdentityId)
            return Error.OperationIsNotAllowed(cultureInfo);

        await _holderTransactionRequestRepository.RemoveAsync(transactionRequest);
        
        return Abstractions.Unit.Value;
    }
}