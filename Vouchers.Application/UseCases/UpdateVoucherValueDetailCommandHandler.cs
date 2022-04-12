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
    public class UpdateVoucherValueDetailCommandHandler : IAuthIdentityHandler<UpdateVoucherValueDetailCommand>
    {
        private readonly IVoucherValueDetailRepository voucherValueDetailRepository;

        public UpdateVoucherValueDetailCommandHandler(IVoucherValueDetailRepository voucherValueDetailRepository)
        {
            this.voucherValueDetailRepository = voucherValueDetailRepository;
        }

        public async Task HandleAsync(UpdateVoucherValueDetailCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var valueDetail = await voucherValueDetailRepository.GetByValueIdAsync(command.VoucherValueId);
            if (valueDetail is null)
                throw new ApplicationException("Voucher's detail does not exist");

            var issuerDomainAccount = valueDetail.Value.Issuer;

            if (issuerDomainAccount.Identity.Id != authIdentityId)
                throw new ApplicationException("Operation is not allowed");

            valueDetail.Description = command.VoucherValueDetailDto.Description;

            voucherValueDetailRepository.Update(valueDetail);
            await voucherValueDetailRepository.SaveAsync();         
        }
    }
}
