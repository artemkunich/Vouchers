using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core;
using Vouchers.Application.Commands.VoucherCommands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Values;
using Vouchers.Application.Services;

namespace Vouchers.Application.UseCases.VoucherCases
{
    internal sealed class DeleteVoucherCommandHandler : IHandler<DeleteVoucherCommand>
    {
        private readonly IAuthIdentityProvider _authIdentityProvider;
        private readonly IRepository<Unit> _unitRepository;
        private readonly IRepository<VoucherValue> _voucherValueRepository;

        public DeleteVoucherCommandHandler(IAuthIdentityProvider authIdentityProvider, IRepository<Unit> unitRepository, IRepository<VoucherValue> voucherValueRepository)
        {
            _authIdentityProvider = authIdentityProvider;
            _unitRepository = unitRepository;
            _voucherValueRepository = voucherValueRepository;
        }

        public async Task HandleAsync(DeleteVoucherCommand command, CancellationToken cancellation)
        {
            var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

            var value = await _voucherValueRepository.GetByIdAsync(command.VoucherValueId);
            if (value is null)
                throw new ApplicationException(Properties.Resources.VoucherValueDoesNotExist);

            if(value.IssuerIdentityId != authIdentityId)
                throw new ApplicationException(Properties.Resources.OperationIsNotAllowed);

            var unit = await _unitRepository.GetByIdAsync(command.VoucherId);
            if(unit is null)
                throw new ApplicationException(Properties.Resources.VoucherDoesNotExist);

            if (unit.UnitType.Id != command.VoucherValueId)
                throw new ApplicationException(Properties.Resources.OperationIsNotAllowed);

            if (unit.CanBeRemoved())
                _unitRepository.Remove(unit);
        }
    }
}
