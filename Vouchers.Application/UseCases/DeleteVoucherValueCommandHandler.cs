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
    public class DeleteVoucherValueCommandHandler : IAuthIdentityHandler<DeleteVoucherValueCommand>
    {     
        private readonly IVoucherValueRepository valueRepository;

        public DeleteVoucherValueCommandHandler(IVoucherValueRepository valueRepository)
        {
            this.valueRepository = valueRepository;
        }

        public async Task HandleAsync(DeleteVoucherValueCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var value = await valueRepository.GetByIdAsync(command.VoucherValueId);
            if(value is null)
                throw new ApplicationException("Voucher's value does not exist");

            var issuerDomainAccount = value.Issuer;

            if (issuerDomainAccount.Identity.Id != authIdentityId)
                throw new ApplicationException("Operation is not allowed");

            if (value.CanBeRemoved())
            {
                valueRepository.Remove(value);
                await valueRepository.SaveAsync();
            }
        }
    }
}
