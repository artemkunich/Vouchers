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
    public class CreateVoucherCommandHandler : IAuthIdentityHandler<CreateVoucherCommand>
    {
        private readonly IVoucherValueRepository voucherValueRepository;
        private readonly IVoucherRepository voucherRepository;

        public CreateVoucherCommandHandler(IVoucherValueRepository voucherValueRepository, IVoucherRepository voucherRepository)
        {
            this.voucherValueRepository = voucherValueRepository;
            this.voucherRepository = voucherRepository;
        }

        public async Task HandleAsync(CreateVoucherCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var value = await voucherValueRepository.GetByIdAsync(command.VoucherValueId);

            if (value is null)
                throw new ApplicationException("Voucher's value does not exist");

            if (value.Issuer.Identity.Id != authIdentityId)
                throw new ApplicationException("Operation is not allowed");

            var voucherDto = command.VoucherDto;
            var voucher = Voucher.Create(voucherDto.ValidFrom, voucherDto.ValidTo, voucherDto.CanBeExchanged, value);

            await voucherRepository.AddAsync(voucher);
            await voucherRepository.SaveAsync();
        }
    }
}
