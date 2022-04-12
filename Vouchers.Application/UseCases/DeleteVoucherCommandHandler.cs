using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core;
using Vouchers.Application.Commands;
using Vouchers.Application.Infrastructure;
using Vouchers.Details;
using System.Threading.Tasks;
using System.Threading;

namespace Vouchers.Application.UseCases
{
    public class DeleteVoucherCommandHandler : IAuthIdentityHandler<DeleteVoucherCommand>
    {     
        private readonly IVoucherRepository voucherRepository;

        public DeleteVoucherCommandHandler(IVoucherRepository voucherRepository)
        {
            this.voucherRepository = voucherRepository;
        }


        public async Task HandleAsync(DeleteVoucherCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var voucher = await voucherRepository.GetByIdAsync(command.VoucherId);
            if(voucher is null)
                throw new ApplicationException("Voucher does not exist");

            var issuerDomainAccount = voucher.Value.Issuer;

            if (issuerDomainAccount.Identity.Id != authIdentityId)
                throw new ApplicationException("Operation is not allowed");

            if (voucher.CanBeRemoved())
            {
                voucherRepository.Remove(voucher);
                await voucherRepository.SaveAsync();
            }
        }
    }
}
