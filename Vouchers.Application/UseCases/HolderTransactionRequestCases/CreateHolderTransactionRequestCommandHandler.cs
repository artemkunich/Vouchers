using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core;
using Vouchers.Application.Commands.HolderTransactionRequestCommands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Domains;
using System.Linq;

namespace Vouchers.Application.UseCases.HolderTransactionRequestCases
{
    internal sealed class CreateHolderTransactionRequestCommandHandler : IAuthIdentityHandler<CreateHolderTransactionRequestCommand, Guid>
    {
        private readonly IRepository<DomainAccount> _domainAccountRepository;
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<UnitType> _unitTypeRepository;
        private readonly IRepository<HolderTransactionRequest> _holderTransactionRequestRepository;

        public CreateHolderTransactionRequestCommandHandler(IRepository<DomainAccount> domainAccountRepository, IRepository<Account> accountRepository, IRepository<UnitType> unitTypeRepository, IRepository<HolderTransactionRequest> holderTransactionRequestRepository)
        {
            _domainAccountRepository = domainAccountRepository;
            _accountRepository = accountRepository;
            _unitTypeRepository = unitTypeRepository;
            _holderTransactionRequestRepository = holderTransactionRequestRepository;
        }

        public async Task<Guid> HandleAsync(CreateHolderTransactionRequestCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var debtorDomainAccount = await _domainAccountRepository.GetByIdAsync(command.DebtorAccountId);
            if (debtorDomainAccount?.IdentityId != authIdentityId)
                throw new ApplicationException("Operation is not allowed");

            var debtorAccount = await _accountRepository.GetByIdAsync(command.DebtorAccountId);

            Account creditorAccount = null;
            if (command.CreditorAccountId != null)
                creditorAccount = await _accountRepository.GetByIdAsync(command.CreditorAccountId.Value);

            var unitType = await _unitTypeRepository.GetByIdAsync(command.UnitTypeId);

            var quantity = UnitTypeQuantity.Create(command.Amount, unitType);

            var maxDurationBeforeValidityStart = command.MaxDaysBeforeValidityStart is null ? TimeSpan.Zero : TimeSpan.FromDays(command.MaxDaysBeforeValidityStart.Value);
            var minDaysBeforeValidityEnd = command.MinDaysBeforeValidityEnd is null ? TimeSpan.Zero : TimeSpan.FromDays(command.MinDaysBeforeValidityEnd.Value);

            var transaction = HolderTransactionRequest.Create(command.DueDate, creditorAccount, debtorAccount, quantity, maxDurationBeforeValidityStart, minDaysBeforeValidityEnd, command.MustBeExchangeable, command.Message);

            await _holderTransactionRequestRepository.AddAsync(transaction);

            return transaction.Id;
        }
    }
}
