using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Files;

namespace Vouchers.Application.Services
{
    internal interface IAppImageService
    {
        Task<CroppedImage> CreateCroppedImage(Stream imageStream, CropParametersDto cropParametersDto);
        Task<CroppedImage> CreateCroppedImage(Guid croppedImageId, CropParametersDto cropParametersDto);
    }
}
