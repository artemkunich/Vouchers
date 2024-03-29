﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Files.Domain;

namespace Vouchers.Application.Services;

public interface IAppImageService
{
    Task<CroppedImage> CreateCroppedImageAsync(Stream imageStream, CropParametersDto cropParametersDto);
    Task<CroppedImage> CreateCroppedImageAsync(CroppedImage croppedImage, CropParametersDto cropParametersDto);
}