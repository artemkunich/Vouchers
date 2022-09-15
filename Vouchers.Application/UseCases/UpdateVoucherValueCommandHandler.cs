using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Application.Commands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Vouchers.Files;
using Vouchers.Values;

namespace Vouchers.Application.UseCases
{
    public class UpdateVoucherValueCommandHandler : IAuthIdentityHandler<UpdateVoucherValueCommand>
    {
        private readonly IRepository<VoucherValue> _voucherValueRepository;
        private readonly IRepository<AppImage> _appImageRepository;
        private readonly IImageService _imageService;

        public UpdateVoucherValueCommandHandler(IRepository<VoucherValue> voucherValueRepository, IRepository<AppImage> appImageRepository, IImageService imageService)
        {
            _voucherValueRepository = voucherValueRepository;
            _appImageRepository = appImageRepository;
            _imageService = imageService;
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

            if (valueDetailDto.CropParameters is not null)
            {
                Stream imageStream = null;

                if (command.Image is not null)
                    imageStream = command.Image.OpenReadStream();

                if (imageStream is null && value.ImageId is not null)
                    imageStream = _imageService.GetImageStream(value.ImageId.Value);

                if (imageStream is not null)
                {
                    var cropParametersDto = valueDetailDto.CropParameters;
                    var croppedContent = await _imageService.CropImageAsync(imageStream, cropParametersDto);
                    var cropParameters = CropParameters.Create(cropParametersDto.X, cropParametersDto.Y, cropParametersDto.Width, cropParametersDto.Height);

                    if (value.ImageId is null)
                    {
                        var image = AppImage.Create(croppedContent, cropParameters);
                        await _appImageRepository.AddAsync(image);

                        value.ImageId = image.Id;
                    }
                    else
                    {
                        var image = await _appImageRepository.GetByIdAsync(value.ImageId.Value);
                        image.CroppedContent = croppedContent;
                        image.CropParameters = cropParameters;
                        await _appImageRepository.UpdateAsync(image);
                    }
                 
                    if (command.Image is not null)
                        await _imageService.SaveImageAsync(command.Image.OpenReadStream(), value.ImageId.Value);

                    requireUpdate = true;
                }
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
