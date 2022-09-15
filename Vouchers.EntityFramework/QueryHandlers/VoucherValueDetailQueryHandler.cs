using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using Vouchers.Core;
using Vouchers.Files;
using Vouchers.Values;

namespace Vouchers.EntityFramework.QueryHandlers
{
    public class VoucherValueDetailQueryHandler : IAuthIdentityHandler<Guid,VoucherValueDetailDto>
    {
        private readonly VouchersDbContext dbContext;
        private readonly IImageService imageService;

        public VoucherValueDetailQueryHandler(VouchersDbContext dbContext, IImageService imageService)
        {
            this.dbContext = dbContext;
            this.imageService = imageService;
        }

        Func<CropParameters, CropParametersDto> mapCropParameters = (CropParameters cp) => cp is null ? null : new CropParametersDto
        {
            X = cp.X,
            Y = cp.Y,
            Width = cp.Width,
            Height = cp.Height,
        };

        public async Task<VoucherValueDetailDto> HandleAsync(Guid valueId, Guid authIdentityId, CancellationToken cancellation)
        {
            var valueWithImage = await dbContext.VoucherValues.Where(v => v.Id == valueId && v.IssuerIdentityId == authIdentityId)
                .GroupJoin(
                    dbContext.Images,
                    v => v.ImageId,
                    i => i.Id,
                    (v,imgs) => new {Value = v, Images = imgs}
                ).SelectMany(
                    result => result.Images.DefaultIfEmpty(),
                    (result, image) => new {result.Value, Image = image}
                ).FirstOrDefaultAsync();

            if(valueWithImage is null)
                return null;

            string imageBase64 = null;
            CropParameters cropParameters = null;
            if (valueWithImage.Image is not null)
            {
                cropParameters = valueWithImage.Image.CropParameters;

                var imageBinary = await imageService.GetImageBinaryAsync(valueWithImage.Image.Id);
                if (imageBinary is not null)
                    imageBase64 = Convert.ToBase64String(imageBinary);
            }

            return new VoucherValueDetailDto
            {
                Ticker = valueWithImage.Value.Ticker,
                Description = valueWithImage.Value.Description,
                ImageBase64 = imageBase64,
                CropParameters = mapCropParameters(cropParameters)
            };
        }

    }
}
