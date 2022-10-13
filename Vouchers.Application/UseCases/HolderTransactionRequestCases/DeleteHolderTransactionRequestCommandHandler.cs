using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Domains;
using System.Linq;
using Vouchers.Application.Commands.HolderTransactionRequestCommands;

namespace Vouchers.Application.UseCases.HolderTransactionRequestCases
{
    internal sealed class DeleteHolderTransactionRequestCommandHandler : IAuthIdentityHandler<DeleteHolderTransactionRequestCommand>
    {
        private readonly IRepository<DomainAccount> _domainAccountRepository;
        private readonly IRepository<HolderTransactionRequest> _holderTransactionRequestRepository;

        public DeleteHolderTransactionRequestCommandHandler(IRepository<DomainAccount> domainAccountRepository, IRepository<HolderTransactionRequest> holderTransactionRequestRepository)
        {
            _domainAccountRepository = domainAccountRepository;
            _holderTransactionRequestRepository = holderTransactionRequestRepository;
        }

        public async Task HandleAsync(DeleteHolderTransactionRequestCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var transactionRequest = await _holderTransactionRequestRepository.GetByIdAsync(command.HolderTransactionRequestId);
            if (transactionRequest is null)
                throw new ApplicationException("Transaction request is not found");

            if (transactionRequest.TransactionId is not null)
                throw new ApplicationException("Transaction request is already performed");

            var debtorDomainAccount = await _domainAccountRepository.GetByIdAsync(transactionRequest.DebtorAccountId);
            if(debtorDomainAccount?.IdentityId != authIdentityId)
                throw new ApplicationException("Operation is not allowed");

            await _holderTransactionRequestRepository.RemoveAsync(transactionRequest);
        }
    }
}
