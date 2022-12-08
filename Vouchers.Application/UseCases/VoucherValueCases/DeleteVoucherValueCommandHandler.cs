using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core;
using Vouchers.Application.Commands.VoucherValueCommands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Values;
using Vouchers.Application.Services;

namespace Vouchers.Application.UseCases.VoucherValueCases
{
    internal sealed class DeleteVoucherValueCommandHandler : IHandler<DeleteVoucherValueCommand>
    {
        private readonly IAuthIdentityProvider _authIdentityProvider;
        private readonly IRepository<VoucherValue> _valueRepository;
        private readonly IRepository<UnitType> _unitTypeRepository;

        public DeleteVoucherValueCommandHandler(IAuthIdentityProvider authIdentityProvider, IRepository<VoucherValue> valueRepository, IRepository<UnitType> unitTypeRepository)
        {
            _authIdentityProvider = authIdentityProvider;
            _valueRepository = valueRepository;
            _unitTypeRepository = unitTypeRepository;
        }

        public async Task HandleAsync(DeleteVoucherValueCommand command, CancellationToken cancellation)
        {
            var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

            var value = await _valueRepository.GetByIdAsync(command.VoucherValueId);
            if(value is null)
                throw new ApplicationException(Properties.Resources.VoucherValueDoesNotExist);

            if (value.IssuerIdentityId != authIdentityId)
                throw new ApplicationException(Properties.Resources.OperationIsNotAllowed);

            var unitType = await _unitTypeRepository.GetByIdAsync(command.VoucherValueId);
            if (unitType is null)
                throw new ApplicationException(Properties.Resources.UnitTypeDoesNotExist);

            if (unitType.CanBeRemoved())
            {
                _unitTypeRepository.Remove(unitType);
                _valueRepository.Remove(value);
            }
        }
    }
}
