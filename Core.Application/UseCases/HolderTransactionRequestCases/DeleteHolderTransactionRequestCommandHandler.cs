using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identity.Abstractions;
using Vouchers.Core.Application.Errors;
using Vouchers.Core.Domain;
using Unit = Akunich.Application.Abstractions.Unit;

namespace Vouchers.Core.Application.UseCases.HolderTransactionRequestCases;

internal sealed class DeleteHolderTransactionRequestCommandHandler : IRequestHandler<DeleteHolderTransactionRequestCommand, Unit>
{
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;
    private readonly IRepository<HolderTransactionRequest,Guid> _holderTransactionRequestRepository;
    private readonly IReadOnlyRepository<Account, Guid> _accountRepository;

    public DeleteHolderTransactionRequestCommandHandler(
        IIdentityIdProvider<Guid> identityIdProvider, 
        IRepository<HolderTransactionRequest,Guid> holderTransactionRequestRepository, 
        IReadOnlyRepository<Account, Guid> accountRepository)
    {
        _identityIdProvider = identityIdProvider;
        _holderTransactionRequestRepository = holderTransactionRequestRepository;
        _accountRepository = accountRepository;
    }

    public async Task<Result<Unit>> HandleAsync(DeleteHolderTransactionRequestCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.GetIdentityId();

        var transactionRequest = await _holderTransactionRequestRepository
            .GetByIdAsync(command.HolderTransactionRequestId, cancellation);
        if (transactionRequest is null)
            return new TransactionRequestIsNotFoundError();

        if (transactionRequest.TransactionId is not null)
            return new TransactionRequestIsAlreadyPerformedError();

        var debtorAccount = await _accountRepository.GetByIdAsync(transactionRequest.DebtorAccountId);
        if(debtorAccount?.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        await _holderTransactionRequestRepository.RemoveAsync(transactionRequest, cancellation);
        
        return Unit.Value;
    }
}