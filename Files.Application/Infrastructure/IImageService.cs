using Vouchers.Files.Application.Dtos;

namespace Vouchers.Files.Application.Infrastructure;

public interface IImageService
{
    public Task<byte[]> CropImageAsync(Stream imageStream, CropParametersDto cropParameters);
    public Task<byte[]> GetImageBinaryAsync(Guid imageId);
    public Stream GetImageStream(Guid imageId);
    public Task SaveImageAsync(Stream imageStream, Guid imageId);
    public Task RemoveImageAsync(Guid imageId);
}