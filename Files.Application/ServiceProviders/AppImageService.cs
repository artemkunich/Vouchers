using Vouchers.Files.Application.Dtos;
using Vouchers.Files.Application.Infrastructure;
using Vouchers.Files.Application.Services;
using Vouchers.Files.Domain;

namespace Vouchers.Files.Application.ServiceProviders;

public sealed class AppImageService : IAppImageService
{
    private readonly IImageService _imageService;

    public AppImageService(IImageService imageService)
    {
        _imageService = imageService;
    }

    public async Task<Image> CreateCroppedImageAsync(Stream imageStream, CropParametersDto cropParametersDto, EntityWithImage entity)
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

        return Image.Create(imageId, croppedImageId, cropParameters, entity);
    }

    public async Task UpdateCropParametersAsync(Image image, CropParametersDto cropParametersDto)
    {
        var imageStream = _imageService.GetImageStream(image.Id);
        var newCroppedContent = await _imageService.CropImageAsync(imageStream, cropParametersDto);
        var newCropParameters = CropParameters.Create(cropParametersDto.X, cropParametersDto.Y, cropParametersDto.Width, cropParametersDto.Height);

        var newCroppedImageId = Guid.NewGuid();
        using (var croppedImageStream = new MemoryStream(newCroppedContent))
        {
            await _imageService.SaveImageAsync(croppedImageStream, newCroppedImageId);
        }
        
        image.SetCrop(newCroppedImageId, newCropParameters);
    }
}