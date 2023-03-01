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
using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;
using Vouchers.Files.Domain;
using Vouchers.Identities.Domain;

namespace Vouchers.Persistence.QueryHandlers;

internal sealed class IdentityDetailQueryHandler : IRequestHandler<IdentityDetailQuery, IdentityDetailDto>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly VouchersDbContext _dbContext;
    
    public IdentityDetailQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext)
    {
        _authIdentityProvider = authIdentityProvider;
        _dbContext = dbContext;
    }

    Func<CropParameters, CropParametersDto> _mapCropParameters = (CropParameters cp) => cp is null ? null : new CropParametersDto
    {
        X = cp.X,
        Y = cp.Y,
        Width = cp.Width,
        Height = cp.Height,
    };

    public async Task<Result<IdentityDetailDto>> HandleAsync(IdentityDetailQuery query, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var identityId = authIdentityId;
        var accountId = query.AccountId;
        DomainAccount domainAccount = null;
        if (accountId is not null)
        {
            domainAccount = await _dbContext.Set<DomainAccount>().Include(acc => acc.Domain).ThenInclude(domain => domain.Contract).FirstOrDefaultAsync(account => account.Id == accountId);
            if (domainAccount is null)
                return null;

            identityId = domainAccount.IdentityId;
        }

        var identityWithImage = await _dbContext.Set<Identity>().Where(id => id.Id == identityId).GroupJoin(
            _dbContext.Set<CroppedImage>(),
            id => id.ImageId,
            im => im.Id,
            (id, ims) => new {Identity = id, Images = ims}
        ).SelectMany(
            result => result.Images.DefaultIfEmpty(),
            (result, image) => new { result.Identity, Image = image }
        ).FirstOrDefaultAsync(cancellation);

        return new IdentityDetailDto
        {
            IdentityId = identityWithImage.Identity.Id,
            Email = identityWithImage.Identity.Email,
            FirstName = identityWithImage.Identity.FirstName,
            LastName = identityWithImage.Identity.LastName,
            ImageId = identityWithImage.Image == null ? null : identityWithImage.Image.ImageId,
            CroppedImageId = identityWithImage.Image == null ? null : identityWithImage.Image.Id,
            CropParameters = _mapCropParameters(identityWithImage.Image?.CropParameters),
            AccountId = accountId,
            IsAdmin = domainAccount?.IsAdmin,
            IsIssuer = domainAccount?.IsIssuer,
            IsOwner = domainAccount?.IsOwner,
        };
    }
}