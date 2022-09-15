using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.UseCases;
using Vouchers.Core;
using Vouchers.Files;

namespace Vouchers.EntityFramework.QueryHandlers
{
    public class IdentityDetailQueryHandler : IAuthIdentityHandler<Guid?, IdentityDetailDto>
    {
        private readonly VouchersDbContext dbContext;
        private readonly IImageService imageService;
        public IdentityDetailQueryHandler(VouchersDbContext dbContext, IImageService imageService)
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

        public async Task<IdentityDetailDto> HandleAsync(Guid? identityId, Guid authIdentityId, CancellationToken cancellation)
        {
            if (identityId is null)
                identityId = authIdentityId;

            var identityWithImage = await dbContext.Identities.Where(id => id.Id == identityId).GroupJoin(
                dbContext.Images,
                id => id.ImageId,
                im => im.Id,
                (id, ims) => new {Identity = id, Images = ims}
            ).SelectMany(
                result => result.Images.DefaultIfEmpty(),
                (result, image) => new { result.Identity, Image = image }
            ).FirstOrDefaultAsync();

            string imageBase64 = null;
            if(identityWithImage.Image is not null)
            {
                var imageBinary = await imageService.GetImageBinaryAsync(identityWithImage.Image.Id);
                if(imageBinary is not null)
                    imageBase64 = Convert.ToBase64String(imageBinary);
            }

            string croppedImageBase64 = identityWithImage.Image?.CroppedContent is null ? null : Convert.ToBase64String(identityWithImage.Image.CroppedContent);

            return new IdentityDetailDto
            {
                IdentityId = identityWithImage.Identity.Id,
                Email = identityWithImage.Identity.Email,
                FirstName = identityWithImage.Identity.FirstName,
                LastName = identityWithImage.Identity.LastName,
                ImageBase64 = imageBase64,
                CroppedImageBase64 = croppedImageBase64,
                CropParameters = mapCropParameters(identityWithImage.Image?.CropParameters),
            };
        }
    }
}
