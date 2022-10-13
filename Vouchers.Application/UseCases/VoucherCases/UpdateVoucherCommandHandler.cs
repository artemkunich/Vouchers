using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core;
using Vouchers.Application.Commands.VoucherCommands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Values;

namespace Vouchers.Application.UseCases.VoucherCases
{
    internal sealed class UpdateVoucherCommandHandler : IAuthIdentityHandler<UpdateVoucherCommand>
    {     
        private readonly IRepository<Unit> _unitRepository;
        private readonly IRepository<VoucherValue> _voucherValueRepository;

        public UpdateVoucherCommandHandler(IRepository<Unit> unitRepository, IRepository<VoucherValue> voucherValueRepository)
        {
            _unitRepository = unitRepository;
            _voucherValueRepository = voucherValueRepository;
        }

        public async Task HandleAsync(UpdateVoucherCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var value = await _voucherValueRepository.GetByIdAsync(command.VoucherValueId);
            if (value is null)
                throw new ApplicationException("Voucher value does not exist");

            if (value.IssuerIdentityId != authIdentityId)
                throw new ApplicationException("Operation is not allowed");

            var unit = await _unitRepository.GetByIdAsync(command.Voucher.Id);
            if (unit is null)
                throw new ApplicationException("Voucher does not exist");

            if (unit.UnitType.Id != command.VoucherValueId)
                throw new ApplicationException("Operation is not allowed");

            unit.SetValidFrom(command.Voucher.ValidFrom);
            unit.SetValidTo(command.Voucher.ValidTo ?? DateTime.MaxValue);
            unit.SetCanBeExchanged(command.Voucher.CanBeExchanged);

            _unitRepository.Update(unit);
        }
    }
}
