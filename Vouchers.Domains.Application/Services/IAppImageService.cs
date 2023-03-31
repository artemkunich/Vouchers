using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Domains.Application.Infrastructure;
using Vouchers.Domains.Application.Dtos;
using Vouchers.Files.Domain;

namespace Vouchers.Domains.Application.Services;

public interface IAppImageService
{
    Task<CroppedImage> CreateCroppedImageAsync(Stream imageStream, CropParametersDto cropParametersDto);
    Task<CroppedImage> CreateCroppedImageAsync(CroppedImage croppedImage, CropParametersDto cropParametersDto);
}