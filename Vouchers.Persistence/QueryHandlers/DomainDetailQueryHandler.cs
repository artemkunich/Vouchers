using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Application.Services;
using Vouchers.Application.UseCases;
using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;
using Vouchers.Files.Domain;

namespace Vouchers.Persistence.QueryHandlers;

internal sealed class DomainDetailQueryHandler : IHandler<Guid, Result<DomainDetailDto>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly VouchersDbContext _dbContext;
    private readonly ICultureInfoProvider _cultureInfoProvider;

    Func<CropParameters, CropParametersDto> _mapCropParameters = (CropParameters cp) => cp is null ? null : new CropParametersDto
    {
        X = cp.X,
        Y = cp.Y,
        Width = cp.Width,
        Height = cp.Height,
    };

    public DomainDetailQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext, ICultureInfoProvider cultureInfoProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _dbContext = dbContext;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task<Result<DomainDetailDto>> HandleAsync(Guid domainId, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        if (authIdentityId is null)
            return Error.NotAuthorized(_cultureInfoProvider.GetCultureInfo());

        var domainAccountsQuery = _dbContext.Set<DomainAccount>().Where(account => account.DomainId == domainId && account.IdentityId == authIdentityId);

        var domainWithImage = await _dbContext.Set<Domain>().Join(domainAccountsQuery, domain => domain.Id, account => account.DomainId, (domain, account) => domain)
            .Include(domain => domain.Contract)
            .Where(domain => domain.Id == domainId)
            .GroupJoin(
                _dbContext.Set<CroppedImage>(),
                d => d.ImageId,
                i => i.Id,
                (d, imgs) => new { Domain = d, Images = imgs }
            ).SelectMany(
                result => result.Images.DefaultIfEmpty(),
                (result, image) => new { result.Domain, Image = image }
            ).FirstOrDefaultAsync(cancellation);

        if (domainWithImage is null)
            return null;

        CropParameters cropParameters = null;
        if (domainWithImage.Image is not null)
        {
            cropParameters = domainWithImage.Image.CropParameters;
        }

        return new DomainDetailDto
        {
            Id = domainWithImage.Domain.Id,
            Name = domainWithImage.Domain.Contract.DomainName,
            Description = domainWithImage.Domain.Description,
            IsPublic = domainWithImage.Domain.IsPublic,
            MembersCount = domainWithImage.Domain.MembersCount,
            ImageId = domainWithImage.Image == null ? null : domainWithImage.Image.ImageId,
            CropParameters = _mapCropParameters(cropParameters)
        };
    }
}