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
    public abstract class CreateIssuerTransactionCommandHandler : IAuthIdentityHandler<CreateIssuerTransactionCommand>
    {
        private readonly IDomainAccountRepository domainAccountRepository;
        private readonly IVoucherRepository voucherRepository;
        private readonly IIssuerTransactionRepository issuerTransactionRepository;
        private readonly IVoucherAccountRepository voucherAccountRepository;

        public CreateIssuerTransactionCommandHandler(IDomainAccountRepository domainAccountRepository, IVoucherRepository voucherRepository, IIssuerTransactionRepository issuerTransactionRepository, IVoucherAccountRepository voucherAccountRepository) 
        {
            this.domainAccountRepository = domainAccountRepository;
            this.voucherRepository = voucherRepository;
            this.issuerTransactionRepository = issuerTransactionRepository;
            this.voucherAccountRepository = voucherAccountRepository;
        }

        public async Task HandleAsync(CreateIssuerTransactionCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var issuerDomainAccount = await domainAccountRepository.GetByIdAsync(command.IssuerDomainAccountId);

            if (issuerDomainAccount?.Identity.Id != authIdentityId)
                    throw new ApplicationException("Operation is not allowed");

            var voucherAccount = voucherAccountRepository.Get(command.IssuerDomainAccountId, command.VoucherId);
            if (voucherAccount is null)
            {
                if (command.Quantity > 0)
                {
                    var voucher = await voucherRepository.GetByIdAsync(command.VoucherId);
                    voucherAccount = VoucherAccount.Create(issuerDomainAccount, 0, voucher);
                }
                else
                    throw new ApplicationException($"Issuer {issuerDomainAccount.Id} does not have account for voucher {command.VoucherId}");
            }


            IssuerTransaction transaction = IssuerTransaction.Create(voucherAccount, command.Quantity);
            transaction.Perform();

            await issuerTransactionRepository.AddAsync(transaction);
            await issuerTransactionRepository.SaveAsync();
        }
    }
}