using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Application.Services;
using Vouchers.Application.UseCases;
using Vouchers.Domains.Domain;
using Vouchers.Files.Domain;
using Vouchers.Values.Domain;

namespace Vouchers.Persistence.QueryHandlers;

internal sealed class VoucherValueDetailQueryHandler : IHandler<VoucherValueDetailQuery,VoucherValueDetailDto>
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

    public async Task<Result<VoucherValueDetailDto>> HandleAsync(VoucherValueDetailQuery query, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var valueId = query.Id;
        
        var valueWithImage = await _dbContext.Set<VoucherValue>().Where(v => v.Id == valueId)
            .Join(
                _dbContext.Set<DomainAccount>().Where(acc => acc.IdentityId == authIdentityId),
                v => v.DomainId,
                acc => acc.DomainId, 
                (v, acc) => v
            ).GroupJoin(
                _dbContext.Set<CroppedImage>(),
                v => v.ImageId,
                i => i.Id,
                (v,imgs) => new {Value = v, Images = imgs}
            ).SelectMany(
                result => result.Images.DefaultIfEmpty(),
                (result, image) => new {result.Value, Image = image}
            ).FirstOrDefaultAsync(cancellation);

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