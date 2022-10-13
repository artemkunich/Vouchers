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
    internal sealed class UpdateVoucherValueCommandHandler : IAuthIdentityHandler<UpdateVoucherValueCommand>
    {
        private readonly IRepository<VoucherValue> _voucherValueRepository;
        private readonly AppImageService _appImageService;

        public UpdateVoucherValueCommandHandler(IRepository<VoucherValue> voucherValueRepository, AppImageService appImageService)
        {
            _voucherValueRepository = voucherValueRepository;
            _appImageService = appImageService;
        }

        public async Task HandleAsync(UpdateVoucherValueCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var value = await _voucherValueRepository.GetByIdAsync(command.VoucherValueId);
            if (value is null)
                throw new ApplicationException("Voucher's value does not exist");

            if (value.IssuerIdentityId != authIdentityId)
                throw new ApplicationException("Operation is not allowed");

            var requireUpdate = false;
            var valueDetailDto = command.VoucherValueDetail;

            if (command.Image is not null && valueDetailDto.CropParameters is not null)
            {
                var imageStream = command.Image.OpenReadStream();
                var image = await _appImageService.CreateCroppedImage(imageStream, valueDetailDto.CropParameters);
                value.ImageId = image.Id;
                requireUpdate = true;
            }
                
            if (command.Image is null && value.ImageId is not null && valueDetailDto.CropParameters is not null)
            {
                var image = await _appImageService.CreateCroppedImage(value.ImageId.Value, valueDetailDto.CropParameters);
                value.ImageId = image.Id;
                requireUpdate = true;
            }

            if (valueDetailDto.Ticker is not null && value.Ticker != valueDetailDto.Ticker)
            {
                value.Ticker = valueDetailDto.Ticker;
                requireUpdate = true;
            }

            if (value.Description != valueDetailDto.Description)
            { 
                value.Description = valueDetailDto.Description;
                requireUpdate = true;
            }

            if (requireUpdate)
                _voucherValueRepository.Update(value);
        }

    }
}
