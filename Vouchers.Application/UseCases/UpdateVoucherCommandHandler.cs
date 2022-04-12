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
    public class UpdateVoucherCommandHandler : IAuthIdentityHandler<UpdateVoucherCommand>
    {     
        private readonly IVoucherRepository voucherRepository;

        public UpdateVoucherCommandHandler(IVoucherRepository voucherRepository)
        {
            this.voucherRepository = voucherRepository;
        }

        public async Task HandleAsync(UpdateVoucherCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var voucher = await voucherRepository.GetByIdAsync(command.VoucherId);
            if(voucher is null)
                throw new ApplicationException("Voucher does not exist");

            var issuerDomainAccount = voucher.Value.Issuer;

            if (issuerDomainAccount.Identity.Id != authIdentityId)
                throw new ApplicationException("Operation is not allowed");

            voucher.SetValidFrom(command.VoucherDto.ValidFrom);
            voucher.SetValidTo(command.VoucherDto.ValidTo);
            voucher.SetCanBeExchanged(command.VoucherDto.CanBeExchanged);

            voucherRepository.Update(voucher);
            await voucherRepository.SaveAsync();
        }
    }
}
