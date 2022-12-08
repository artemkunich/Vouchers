using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Application.UseCases;
using Vouchers.Files;

namespace Vouchers.EntityFramework.QueryHandlers
{
    internal sealed class VoucherValueDetailQueryHandler : IHandler<Guid,VoucherValueDetailDto>
    {
        private readonly IAuthIdentityProvider _authIdentityProvider;
        private readonly VouchersDbContext _dbContext;

        public VoucherValueDetailQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext)
        {
            _authIdentityProvider = authIdentityProvider;
            _dbContext = dbContext;
        }

        Func<CropParameters, CropParametersDto> mapCropParameters = (CropParameters cp) => cp is null ? null : new CropParametersDto
        {
            X = cp.X,
            Y = cp.Y,
            Width = cp.Width,
            Height = cp.Height,
        };

        public async Task<VoucherValueDetailDto> HandleAsync(Guid valueId, CancellationToken cancellation)
        {
            var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

            var valueWithImage = await _dbContext.VoucherValues.Where(v => v.Id == valueId)
                .Join(
                _dbContext.DomainAccounts.Where(acc => acc.IdentityId == authIdentityId),
                    v => v.DomainId,
                    acc => acc.DomainId, 
                    (v, acc) => v
                ).GroupJoin(
                    _dbContext.CroppedImages,
                    v => v.ImageId,
                    i => i.Id,
                    (v,imgs) => new {Value = v, Images = imgs}
                ).SelectMany(
                    result => result.Images.DefaultIfEmpty(),
                    (result, image) => new {result.Value, Image = image}
                ).FirstOrDefaultAsync();

            if(valueWithImage is null)
                return null;

            CropParameters cropParameters = null;
            if (valueWithImage.Image is not null)
            {
                cropParameters = valueWithImage.Image.CropParameters;
            }

            return new VoucherValueDetailDto
            {
                Ticker = valueWithImage.Value.Ticker,
                Description = valueWithImage.Value.Description,
                ImageId = valueWithImage.Image == null ? null : valueWithImage.Image.ImageId,
                CropParameters = mapCropParameters(cropParameters)
            };
        }

    }
}
