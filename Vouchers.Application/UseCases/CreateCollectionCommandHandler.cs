using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core;
using Vouchers.Application.Commands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Domains;
using System.Linq;

namespace Vouchers.Application.UseCases
{
    public class CreateCollectionCommandHandler : IAuthIdentityHandler<CreateCollectionCommand>
    {
        private readonly IRepository<DomainAccount> _domainAccountRepository;
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<Unit> _unitRepository;
        private readonly IRepository<UnitType> _unitTypeRepository;
        private readonly IRepository<CollectionRequest> _collectionRequestRepository;
        private readonly IRepository<AccountItem> _accountItemRepository;

        public CreateCollectionCommandHandler(IRepository<DomainAccount> domainAccountRepository, IRepository<Account> accountRepository, IRepository<Unit> unitRepository, IRepository<UnitType> unitTypeRepository, IRepository<CollectionRequest> collectionRequestRepository, IRepository<AccountItem> accountItemRepository)
        {
            _domainAccountRepository = domainAccountRepository;
            _accountRepository = accountRepository;
            _unitRepository = unitRepository;
            _unitTypeRepository = unitTypeRepository;
            _collectionRequestRepository = collectionRequestRepository;
            _accountItemRepository = accountItemRepository;
        }

        public async Task HandleAsync(CreateCollectionCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var createHolderTransactionCommand = command.CreateHolderTransactionCommand;
            
            var creditorDomainAccount = await _domainAccountRepository.GetByIdAsync(createHolderTransactionCommand.CreditorDomainAccountId);
            if (creditorDomainAccount?.IdentityId != authIdentityId)
                throw new ApplicationException("Operation is not allowed");
            if (!creditorDomainAccount.IsConfirmed)
                throw new ApplicationException("Creditor account is not activated");

            var debtorDomainAccount = await _domainAccountRepository.GetByIdAsync(createHolderTransactionCommand.DebtorDomainAccountId);
            if (!debtorDomainAccount.IsConfirmed)
                throw new ApplicationException("Debtor account is not activated");

            var collectionRequest = await _collectionRequestRepository.GetByIdAsync(command.CollectionRequestId);
            if (collectionRequest is null)
                throw new ApplicationException("Collection request does not exist");

            var unitType = await _unitTypeRepository.GetByIdAsync(createHolderTransactionCommand.VoucherValueId);
            if(unitType is null)
                throw new ApplicationException("Voucher value does not exist");

            var creditorAccount = await _accountRepository.GetByIdAsync(createHolderTransactionCommand.CreditorDomainAccountId);
            if (creditorAccount is null)
                throw new ApplicationException("Creditor's account does not exist");

            var debtorAccount = await _accountRepository.GetByIdAsync(createHolderTransactionCommand.DebtorDomainAccountId);
            if (debtorAccount is null)
                throw new ApplicationException("Debtor's account does not exist");

            HolderTransaction collection = HolderTransaction.Create(creditorAccount, debtorAccount, unitType);

            foreach (var item in createHolderTransactionCommand.Items)
            {
                var creditAccountItem = (await _accountItemRepository.GetByExpressionAsync(accItem => accItem.HolderId == createHolderTransactionCommand.CreditorDomainAccountId && accItem.Unit.Id == item.Item1)).FirstOrDefault(); 
                if (creditAccountItem is null)
                {
                    throw new ApplicationException($"User {createHolderTransactionCommand.CreditorDomainAccountId} does not have account for voucher {item.Item2}");
                }
                var debitAccountItem = (await _accountItemRepository.GetByExpressionAsync(accItem => accItem.HolderId == createHolderTransactionCommand.DebtorDomainAccountId && accItem.Unit.Id == item.Item1)).FirstOrDefault(); 
                if (debitAccountItem is null)
                {
                    var voucher = await _unitRepository.GetByIdAsync(item.Item1);
                    debitAccountItem = AccountItem.Create(debtorAccount, 0, voucher);
                }
                collection.AddTransactionItem(HolderTransactionItem.Create(UnitQuantity.Create(item.Item2, debitAccountItem.Unit), creditAccountItem, debitAccountItem));
            }
           
            collectionRequest.PerformCollection(collection);

            await _collectionRequestRepository.UpdateAsync(collectionRequest);
        }
    }
}
