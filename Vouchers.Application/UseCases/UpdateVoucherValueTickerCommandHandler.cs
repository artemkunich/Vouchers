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
    public class UpdateVoucherValueTickerCommandHandler : IAuthIdentityHandler<UpdateVoucherValueTickerCommand>
    {     
        private readonly IVoucherValueDetailRepository voucherValueDetailRepository;

        public UpdateVoucherValueTickerCommandHandler(IVoucherValueDetailRepository voucherValueDetailRepository)
        {
            this.voucherValueDetailRepository = voucherValueDetailRepository;
        }

        public async Task HandleAsync(UpdateVoucherValueTickerCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var valueDetail = await voucherValueDetailRepository.GetByValueIdAsync(command.VoucherValueId);
            if(valueDetail is null)
                throw new ApplicationException("Voucher's detail does not exist");

            var issuerDomainAccount = valueDetail.Value.Issuer;

            if (issuerDomainAccount.Identity.Id != authIdentityId)
                throw new ApplicationException("Operation is not allowed");

            valueDetail.Ticker = command.Ticker;

            voucherValueDetailRepository.Update(valueDetail);
            await voucherValueDetailRepository.SaveAsync();           
        }
    }
}
