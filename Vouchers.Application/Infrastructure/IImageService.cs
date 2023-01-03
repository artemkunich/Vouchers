using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Infrastructure;

public interface IImageService
{
    public Task<byte[]> CropImageAsync(Stream imageStream, CropParametersDto cropParameters);
    public Task<byte[]> GetImageBinaryAsync(Guid imageId);
    public Stream GetImageStream(Guid imageId);
    public Task SaveImageAsync(Stream imageStream, Guid imageId);
    public Task RemoveImageAsync(Guid imageId);
}