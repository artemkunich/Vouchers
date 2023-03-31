using System;
using System.IO;
using System.Threading.Tasks;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Infrastructure;

public interface IImageService
{
    public Task<byte[]> CropImageAsync(Stream imageStream, CropParametersDto cropParameters);
    public Task<byte[]> GetImageBinaryAsync(Guid imageId);
    public Stream GetImageStream(Guid imageId);
    public Task SaveImageAsync(Stream imageStream, Guid imageId);
    public Task RemoveImageAsync(Guid imageId);
}