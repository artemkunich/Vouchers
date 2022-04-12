using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core;
using Vouchers.Application.Commands;
using Vouchers.Application.Infrastructure;
using Vouchers.Values;
using System.Threading.Tasks;
using System.Threading;

namespace Vouchers.Application.UseCases
{
    public class CreateVoucherValueCommandHandler : IAuthIdentityHandler<CreateVoucherValueCommand>
    {
        private readonly IDomainAccountRepository domainAccountRepository;       
        private readonly IVoucherValueDetailFactory voucherValueDetailFactory;
        private readonly IVoucherValueDetailRepository voucherValueDetailRepository;

        public CreateVoucherValueCommandHandler(IDomainAccountRepository domainAccountRepository, IVoucherValueDetailFactory voucherValueDetailFactory,
            IVoucherValueDetailRepository voucherValueDetailRepository)
        {
            this.domainAccountRepository = domainAccountRepository;
            this.voucherValueDetailFactory = voucherValueDetailFactory;
            this.voucherValueDetailRepository = voucherValueDetailRepository;

        }

        public async Task HandleAsync(CreateVoucherValueCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var issuerDomainAccount = await domainAccountRepository.GetByIdAsync(command.IssuerDomainAccountId);

            if (issuerDomainAccount?.Identity.Id != authIdentityId)
                throw new ApplicationException("Operation is not allowed");

            var value = VoucherValue.Create(issuerDomainAccount);
            var detail = voucherValueDetailFactory.CreateVoucherValueDetail(value, command.Ticker, command.VoucherValueDetailDto.Description);

            await voucherValueDetailRepository.AddAsync(detail);
            await voucherValueDetailRepository.SaveAsync();
        }
    }
}
