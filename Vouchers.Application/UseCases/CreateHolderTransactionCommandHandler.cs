using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Core;
using Vouchers.Application.Commands;
using Vouchers.Application.Infrastructure;

namespace Vouchers.Application.UseCases
{
    public abstract class CreateHolderTransactionCommandHandler : IAuthIdentityHandler<CreateHolderTransactionCommand>
    {
        private readonly IDomainAccountRepository domainAccountRepository;      
        private readonly IVoucherRepository voucherRepository;
        private readonly IVoucherValueRepository voucherValueRepository;
        private readonly IHolderTransactionRepository holderTransactionRepository;
        private readonly IVoucherAccountRepository voucherAccountRepository;

        public CreateHolderTransactionCommandHandler(IDomainAccountRepository domainAccountRepository, IVoucherRepository voucherRepository, IVoucherValueRepository voucherValueRepository, IHolderTransactionRepository holderTransactionRepository, IVoucherAccountRepository voucherAccountRepository) 
        {
            this.domainAccountRepository = domainAccountRepository;
            
            this.voucherRepository = voucherRepository;
            this.voucherValueRepository = voucherValueRepository;
            this.holderTransactionRepository = holderTransactionRepository;
            this.voucherAccountRepository = voucherAccountRepository;
        }

        public async Task HandleAsync(CreateHolderTransactionCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var creditorDomainAccount = await domainAccountRepository.GetByIdAsync(command.CreditorDomainAccountId);
            if (creditorDomainAccount?.Identity.Id != authIdentityId)
                throw new ApplicationException("Operation is not allowed");

            var debtorDomainAccount = await domainAccountRepository.GetByIdAsync(command.DebtorDomainAccountId);

            var voucherValue = await voucherValueRepository.GetByIdAsync(command.VoucherValueId);

            HolderTransaction transaction = HolderTransaction.Create(creditorDomainAccount, debtorDomainAccount, voucherValue);

            foreach (var item in command.TransactionItems)
            {
                var creditAccount = voucherAccountRepository.Get(command.CreditorDomainAccountId, item.Item2);
                if (creditAccount is null)
                {
                    throw new ApplicationException($"User {command.CreditorDomainAccountId} does not have account for voucher {item.Item2}");
                }
                var debitAccount = voucherAccountRepository.Get(command.DebtorDomainAccountId, item.Item2);
                if (debitAccount is null)
                {
                    var voucher = await voucherRepository.GetByIdAsync(item.Item2);
                    debitAccount = VoucherAccount.Create(debtorDomainAccount, 0, voucher);
                }
                transaction.AddTransactionItem(HolderTransactionItem.Create(VoucherQuantity.Create(item.Item1, debitAccount.Unit), creditAccount, debitAccount));
            }
    
            transaction.Perform();

            await holderTransactionRepository.AddAsync(transaction);
            await holderTransactionRepository.SaveAsync();
        }
    }
}