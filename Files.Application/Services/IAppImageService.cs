using Vouchers.Files.Application.Dtos;
using Vouchers.Files.Domain;

namespace Vouchers.Files.Application.Services;

public interface IAppImageService
{
    Task<Image> CreateCroppedImageAsync(Stream imageStream, CropParametersDto cropParametersDto, EntityWithImage entity);
    Task UpdateCropParametersAsync(Image image, CropParametersDto cropParametersDto);
}