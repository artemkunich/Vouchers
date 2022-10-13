using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core;
using Vouchers.Application.Commands.VoucherCommands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Values;
using System.Linq;

namespace Vouchers.Application.UseCases.VoucherCases
{
    internal sealed class CreateVoucherCommandHandler : IAuthIdentityHandler<CreateVoucherCommand, Guid>
    {
        private readonly IRepository<VoucherValue> _voucherValueRepository;
        private readonly IRepository<UnitType> _unitTypeRepository;
        private readonly IRepository<Unit> _unitRepository;

        public CreateVoucherCommandHandler(IRepository<VoucherValue> voucherValueRepository, IRepository<UnitType> unitTypeRepository, IRepository<Unit> unitRepository)
        {
            _voucherValueRepository = voucherValueRepository;
            _unitTypeRepository = unitTypeRepository;
            _unitRepository = unitRepository;
        }

        public async Task<Guid> HandleAsync(CreateVoucherCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var value = await _voucherValueRepository.GetByIdAsync(command.VoucherValueId);

            if (value is null)
                throw new ApplicationException("Unit type does not exist");

            if (value.IssuerIdentityId != authIdentityId)
                throw new ApplicationException("Operation is not allowed");

            var unitType = await _unitTypeRepository.GetByIdAsync(command.VoucherValueId);

            var unitDto = command.Voucher;
            var unit = Unit.Create(unitDto.ValidFrom, unitDto.ValidTo ?? DateTime.MaxValue, unitDto.CanBeExchanged, unitType);

            await _unitRepository.AddAsync(unit);

            return unit.Id;
        }
    }
}
