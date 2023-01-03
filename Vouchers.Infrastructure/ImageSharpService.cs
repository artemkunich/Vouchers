using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;

namespace Vouchers.Infrastructure;

public sealed class ImageSharpService : IImageService
{
    private const int MaxImageSide = 1024;
    private const int CroppedImageSide = 100;

    public async Task<byte[]> CropImageAsync(Stream imageStream, CropParametersDto cropParameters)
    {
        var image = await Image.LoadAsync(imageStream);
        image.Mutate(x => x.Crop(new Rectangle()
        {
            X = (int)Math.Round(cropParameters.X * image.Width / 100),
            Y = (int)Math.Round(cropParameters.Y * image.Height / 100),
            Width = (int)Math.Round(cropParameters.Width * image.Width / 100),
            Height = (int)Math.Round(cropParameters.Height * image.Height / 100),
        }));

        if (image.Width != image.Height || image.Width != CroppedImageSide)
        {
            image.Mutate(x => x.Resize(CroppedImageSide, CroppedImageSide));
        }

        using (var pngMemoryStream = new MemoryStream())
        {
            image.SaveAsPng(pngMemoryStream);
            return pngMemoryStream.ToArray();
        }
    }

    public async Task SaveImageAsync(Stream imageStream, Guid imageId)
    {
        var image = await Image.LoadAsync(imageStream);
        var maxSide = Math.Max(image.Width, image.Height);
        if (maxSide > MaxImageSide)
            image.Mutate(x => x.Resize(image.Width * MaxImageSide / maxSide, image.Height * MaxImageSide / maxSide));

        image.SaveAsPngAsync($"/app/images/{imageId}.png");
    }

    public Task RemoveImageAsync(Guid imageId) 
    {
        var fileName = $"/app/images/{imageId}.png";

        if (File.Exists(fileName))
            File.Delete(fileName);

        return Task.CompletedTask;
    }

    public async Task<byte[]> GetImageBinaryAsync(Guid imageId) {

        var fileName = $"/app/images/{imageId}.png";

        if (File.Exists(fileName))
            return await File.ReadAllBytesAsync(fileName);

        return null;
    }

    public Stream GetImageStream(Guid imageId)
    {

        var fileName = $"/app/images/{imageId}.png";

        if (File.Exists(fileName))
            return new FileStream(fileName, FileMode.Open, FileAccess.Read);

        return null;
    }
}