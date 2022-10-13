using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core;
using Vouchers.Application.Commands.VoucherValueCommands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Values;

namespace Vouchers.Application.UseCases.VoucherValueCases
{
    internal sealed class DeleteVoucherValueCommandHandler : IAuthIdentityHandler<DeleteVoucherValueCommand>
    {     
        private readonly IRepository<VoucherValue> _valueRepository;
        private readonly IRepository<UnitType> _unitTypeRepository;

        public DeleteVoucherValueCommandHandler(IRepository<VoucherValue> valueRepository, IRepository<UnitType> unitTypeRepository)
        {
            _valueRepository = valueRepository;
            _unitTypeRepository = unitTypeRepository;
        }

        public async Task HandleAsync(DeleteVoucherValueCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var value = await _valueRepository.GetByIdAsync(command.VoucherValueId);
            if(value is null)
                throw new ApplicationException("Voucher's value does not exist");

            if (value.IssuerIdentityId != authIdentityId)
                throw new ApplicationException("Operation is not allowed");

            var unitType = await _unitTypeRepository.GetByIdAsync(command.VoucherValueId);
            if (unitType is null)
                throw new ApplicationException("Unit type does not exist");

            if (unitType.CanBeRemoved())
            {
                _unitTypeRepository.Remove(unitType);
                _valueRepository.Remove(value);
            }
        }
    }
}
