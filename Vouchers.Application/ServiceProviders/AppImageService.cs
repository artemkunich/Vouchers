using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Files;

namespace Vouchers.Application.ServiceProviders
{
    internal sealed class AppImageService : IAppImageService
    {
        private readonly IRepository<CroppedImage> _croppedRepository;
        private readonly IImageService _imageService;

        public AppImageService(IRepository<CroppedImage> appImageRepository, IImageService imageService)
        {
            _croppedRepository = appImageRepository;
            _imageService = imageService;
        }

        public async Task<CroppedImage> CreateCroppedImage(Stream imageStream, CropParametersDto cropParametersDto)
        {
            var croppedContent = await _imageService.CropImageAsync(imageStream, cropParametersDto);
            var cropParameters = CropParameters.Create(cropParametersDto.X, cropParametersDto.Y, cropParametersDto.Width, cropParametersDto.Height);

            var croppedImageId = Guid.NewGuid();
            using (var croppedImageStream = new MemoryStream(croppedContent))
            {
                await _imageService.SaveImageAsync(croppedImageStream, croppedImageId);
            }

            var imageId = Guid.NewGuid();
            imageStream.Position = 0;
            await _imageService.SaveImageAsync(imageStream, imageId);

            var image = CroppedImage.Create(croppedImageId, imageId, cropParameters);

            if (image is null)
                return image;

            await _croppedRepository.AddAsync(image);

            return image;

        }

        public async Task<CroppedImage> CreateCroppedImage(Guid croppedImageId, CropParametersDto cropParametersDto)
        {
            var croppedImage = await _croppedRepository.GetByIdAsync(croppedImageId);
            if (croppedImage is null)
                throw new ApplicationException("Image does not exist");

            var imageStream = _imageService.GetImageStream(croppedImage.ImageId);
            var newCroppedContent = await _imageService.CropImageAsync(imageStream, cropParametersDto);
            var newCropParameters = CropParameters.Create(cropParametersDto.X, cropParametersDto.Y, cropParametersDto.Width, cropParametersDto.Height);

            var newCroppedImageId = Guid.NewGuid();
            using (var croppedImageStream = new MemoryStream(newCroppedContent))
            {
                await _imageService.SaveImageAsync(croppedImageStream, newCroppedImageId);
            }



            var newCroppedImage = CroppedImage.Create(newCroppedImageId, croppedImage.ImageId, newCropParameters);

            await _croppedRepository.AddAsync(newCroppedImage);

            return newCroppedImage;

        }
    }
}
