﻿using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core.Domain;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Domains.Domain;
using System.Linq;
using Vouchers.Application.Commands.HolderTransactionRequestCommands;
using Vouchers.Application.Services;

namespace Vouchers.Application.UseCases.HolderTransactionRequestCases;

internal sealed class DeleteHolderTransactionRequestCommandHandler : IHandler<DeleteHolderTransactionRequestCommand>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IRepository<HolderTransactionRequest,Guid> _holderTransactionRequestRepository;

    public DeleteHolderTransactionRequestCommandHandler(IAuthIdentityProvider authIdentityProvider, 
        IRepository<DomainAccount,Guid> domainAccountRepository, IRepository<HolderTransactionRequest,Guid> holderTransactionRequestRepository)
    {
        _authIdentityProvider = authIdentityProvider;
        _domainAccountRepository = domainAccountRepository;
        _holderTransactionRequestRepository = holderTransactionRequestRepository;
    }

    public async Task HandleAsync(DeleteHolderTransactionRequestCommand command, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var transactionRequest = await _holderTransactionRequestRepository.GetByIdAsync(command.HolderTransactionRequestId);
        if (transactionRequest is null)
            throw new ApplicationException(Properties.Resources.TransactionRequestIsNotFound);

        if (transactionRequest.TransactionId is not null)
            throw new ApplicationException(Properties.Resources.TransactionRequestIsAlreadyPerformed);

        var debtorDomainAccount = await _domainAccountRepository.GetByIdAsync(transactionRequest.DebtorAccountId);
        if(debtorDomainAccount?.IdentityId != authIdentityId)
            throw new ApplicationException(Properties.Resources.OperationIsNotAllowed);

        await _holderTransactionRequestRepository.RemoveAsync(transactionRequest);
    }
}