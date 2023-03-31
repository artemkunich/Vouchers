using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Core.Application.Errors;
using Vouchers.Core.Domain;
using OperationIsNotAllowedError = Vouchers.Core.Application.Errors.OperationIsNotAllowedError;
using Unit = Vouchers.Common.Application.Abstractions.Unit;

namespace Vouchers.Core.Application.UseCases.HolderTransactionRequestCases;

internal sealed class DeleteHolderTransactionRequestCommandHandler : IRequestHandler<DeleteHolderTransactionRequestCommand, Unit>
{
    private readonly IIdentityIdProvider _identityIdProvider;
    private readonly IRepository<HolderTransactionRequest,Guid> _holderTransactionRequestRepository;
    private readonly IReadOnlyRepository<Account, Guid> _accountRepository;

    public DeleteHolderTransactionRequestCommandHandler(IIdentityIdProvider identityIdProvider, IRepository<HolderTransactionRequest,Guid> holderTransactionRequestRepository, IReadOnlyRepository<Account, Guid> accountRepository)
    {
        _identityIdProvider = identityIdProvider;
        _holderTransactionRequestRepository = holderTransactionRequestRepository;
        _accountRepository = accountRepository;
    }

    public async Task<Result<Unit>> HandleAsync(DeleteHolderTransactionRequestCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.CurrentIdentityId;

        var transactionRequest = await _holderTransactionRequestRepository.GetByIdAsync(command.HolderTransactionRequestId);
        if (transactionRequest is null)
            return new TransactionRequestIsNotFoundError();

        if (transactionRequest.TransactionId is not null)
            return new TransactionRequestIsAlreadyPerformedError();

        var debtorAccount = await _accountRepository.GetByIdAsync(transactionRequest.DebtorAccountId);
        if(debtorAccount?.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        await _holderTransactionRequestRepository.RemoveAsync(transactionRequest);
        
        return Unit.Value;
    }
}