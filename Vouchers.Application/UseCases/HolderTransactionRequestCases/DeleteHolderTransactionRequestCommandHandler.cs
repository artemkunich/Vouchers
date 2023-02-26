using System;
using Vouchers.Core.Domain;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Application.Abstractions;
using Vouchers.Domains.Domain;
using Vouchers.Application.Commands.HolderTransactionRequestCommands;
using Vouchers.Application.Errors;
using Vouchers.Application.Services;

namespace Vouchers.Application.UseCases.HolderTransactionRequestCases;

internal sealed class DeleteHolderTransactionRequestCommandHandler : IHandler<DeleteHolderTransactionRequestCommand, Abstractions.Unit>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IReadOnlyRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IRepository<HolderTransactionRequest,Guid> _holderTransactionRequestRepository;

    public DeleteHolderTransactionRequestCommandHandler(IAuthIdentityProvider authIdentityProvider, 
        IReadOnlyRepository<DomainAccount,Guid> domainAccountRepository, IRepository<HolderTransactionRequest,Guid> holderTransactionRequestRepository)
    {
        _authIdentityProvider = authIdentityProvider;
        _domainAccountRepository = domainAccountRepository;
        _holderTransactionRequestRepository = holderTransactionRequestRepository;
    }

    public async Task<Result<Abstractions.Unit>> HandleAsync(DeleteHolderTransactionRequestCommand command, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var transactionRequest = await _holderTransactionRequestRepository.GetByIdAsync(command.HolderTransactionRequestId);
        if (transactionRequest is null)
            return new TransactionRequestIsNotFoundError();

        if (transactionRequest.TransactionId is not null)
            return new TransactionRequestIsAlreadyPerformedError();

        var debtorDomainAccount = await _domainAccountRepository.GetByIdAsync(transactionRequest.DebtorAccountId);
        if(debtorDomainAccount?.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        await _holderTransactionRequestRepository.RemoveAsync(transactionRequest);
        
        return Abstractions.Unit.Value;
    }
}