using System;
using System.IO;
using System.Threading.Tasks;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Files.Domain;

namespace Vouchers.Application.ServiceProviders;

public sealed class AppImageService : IAppImageService
{
    private readonly IImageService _imageService;

    public AppImageService(IImageService imageService)
    {
        _imageService = imageService;
    }

    public async Task<CroppedImage> CreateCroppedImageAsync(Stream imageStream, CropParametersDto cropParametersDto)
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

        return CroppedImage.Create(croppedImageId, imageId, cropParameters);
    }

    public async Task<CroppedImage> CreateCroppedImageAsync(CroppedImage croppedImage, CropParametersDto cropParametersDto)
    {
        var imageStream = _imageService.GetImageStream(croppedImage.ImageId);
        var newCroppedContent = await _imageService.CropImageAsync(imageStream, cropParametersDto);
        var newCropParameters = CropParameters.Create(cropParametersDto.X, cropParametersDto.Y, cropParametersDto.Width, cropParametersDto.Height);

        var newCroppedImageId = Guid.NewGuid();
        using (var croppedImageStream = new MemoryStream(newCroppedContent))
        {
            await _imageService.SaveImageAsync(croppedImageStream, newCroppedImageId);
        }
        
        return CroppedImage.Create(newCroppedImageId, croppedImage.ImageId, newCropParameters);
    }
}