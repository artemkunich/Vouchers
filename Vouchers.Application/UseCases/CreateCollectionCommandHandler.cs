using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;
using Vouchers.Application.Commands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;

namespace Vouchers.Application.UseCases
{
    public class CreateCollectionCommandHandler : IAuthIdentityHandler<CreateCollectionCommand>
    {
        private readonly IDomainAccountRepository domainAccountRepository;
        private readonly IVoucherRepository voucherRepository;
        private readonly IVoucherValueRepository voucherValueRepository;
        private readonly ICollectionRequestRepository collectionRequestRepository;
        private readonly IVoucherAccountRepository voucherAccountRepository;

        public CreateCollectionCommandHandler(IDomainAccountRepository domainAccountRepository, IVoucherRepository voucherRepository, IVoucherValueRepository voucherValueRepository, ICollectionRequestRepository collectionRequestRepository, IVoucherAccountRepository voucherAccountRepository)
        {
            this.domainAccountRepository = domainAccountRepository;

            this.voucherRepository = voucherRepository;
            this.voucherValueRepository = voucherValueRepository;
            this.collectionRequestRepository = collectionRequestRepository;
            this.voucherAccountRepository = voucherAccountRepository;
        }

        public async Task HandleAsync(CreateCollectionCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var createHolderTransactionCommand = command.CreateHolderTransactionCommand;
            var creditorDomainAccount = await domainAccountRepository.GetByIdAsync(createHolderTransactionCommand.CreditorDomainAccountId);

            if (creditorDomainAccount?.Identity.Id != authIdentityId)
                throw new ApplicationException("Operation is not allowed");

            var debtorDomainAccount = await domainAccountRepository.GetByIdAsync(createHolderTransactionCommand.DebtorDomainAccountId);
            var collectionRequest = await collectionRequestRepository.GetByIdAsync(command.CollectionRequestId);
            if (collectionRequest is null)
                throw new ApplicationException("Collection request does not exist");

            var voucherValue = await voucherValueRepository.GetByIdAsync(createHolderTransactionCommand.VoucherValueId);
            if(voucherValue is null)
                throw new ApplicationException("Voucher value does not exist");
            HolderTransaction collection = HolderTransaction.Create(creditorDomainAccount, debtorDomainAccount, voucherValue);

            foreach (var item in createHolderTransactionCommand.TransactionItems)
            {
                var creditAccount = voucherAccountRepository.Get(createHolderTransactionCommand.CreditorDomainAccountId, item.Item2);
                if (creditAccount is null)
                {
                    throw new ApplicationException($"User {createHolderTransactionCommand.CreditorDomainAccountId} does not have account for voucher {item.Item2}");
                }
                var debitAccount = voucherAccountRepository.Get(createHolderTransactionCommand.DebtorDomainAccountId, item.Item2);
                if (debitAccount is null)
                {
                    var voucher = await voucherRepository.GetByIdAsync(item.Item2);
                    debitAccount = VoucherAccount.Create(debtorDomainAccount, 0, voucher);
                }
                collection.AddTransactionItem(HolderTransactionItem.Create(VoucherQuantity.Create(item.Item1, debitAccount.Unit), creditAccount, debitAccount));
            }
           
            collectionRequest.PerformCollection(collection);

            collectionRequestRepository.Update(collectionRequest);
            await collectionRequestRepository.SaveAsync();
        }
    }
}
