using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Application.Commands.VoucherValueCommands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Vouchers.Files;
using Vouchers.Values;
using Vouchers.Application.Dtos;
using Vouchers.Application.Services;

namespace Vouchers.Application.UseCases.VoucherValueCases
{
    internal sealed class UpdateVoucherValueCommandHandler : IHandler<UpdateVoucherValueCommand>
    {
        private readonly IAuthIdentityProvider _authIdentityProvider;
        private readonly IRepository<VoucherValue> _voucherValueRepository;
        private readonly IAppImageService _appImageService;

        public UpdateVoucherValueCommandHandler(IAuthIdentityProvider authIdentityProvider, IRepository<VoucherValue> voucherValueRepository, IAppImageService appImageService)
        {
            _authIdentityProvider = authIdentityProvider;
            _voucherValueRepository = voucherValueRepository;
            _appImageService = appImageService;
        }

        public async Task HandleAsync(UpdateVoucherValueCommand command, CancellationToken cancellation)
        {
            var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

            var value = await _voucherValueRepository.GetByIdAsync(command.Id);
            if (value is null)
                throw new ApplicationException(Properties.Resources.VoucherValueDoesNotExist);

            if (value.IssuerIdentityId != authIdentityId)
                throw new ApplicationException(Properties.Resources.OperationIsNotAllowed);

            var requireUpdate = false;

            if (command.Image is not null && command.CropParameters is not null)
            {
                var imageStream = command.Image.OpenReadStream();
                var image = await _appImageService.CreateCroppedImage(imageStream, command.CropParameters);
                value.ImageId = image.Id;
                requireUpdate = true;
            }
                
            if (command.Image is null && value.ImageId is not null && command.CropParameters is not null)
            {
                var image = await _appImageService.CreateCroppedImage(value.ImageId.Value, command.CropParameters);
                value.ImageId = image.Id;
                requireUpdate = true;
            }

            if (command.Ticker is not null && value.Ticker != command.Ticker)
            {
                value.Ticker = command.Ticker;
                requireUpdate = true;
            }

            if (value.Description != command.Description)
            { 
                value.Description = command.Description;
                requireUpdate = true;
            }

            if (requireUpdate)
                _voucherValueRepository.Update(value);
        }

    }
}
