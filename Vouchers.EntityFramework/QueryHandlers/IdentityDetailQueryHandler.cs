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
using Vouchers.Domains;
using Vouchers.Files;

namespace Vouchers.EntityFramework.QueryHandlers
{
    internal sealed class IdentityDetailQueryHandler : IAuthIdentityHandler<Guid?, IdentityDetailDto>
    {
        private readonly VouchersDbContext _dbContext;
        public IdentityDetailQueryHandler(VouchersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        Func<CropParameters, CropParametersDto> mapCropParameters = (CropParameters cp) => cp is null ? null : new CropParametersDto
        {
            X = cp.X,
            Y = cp.Y,
            Width = cp.Width,
            Height = cp.Height,
        };

        public async Task<IdentityDetailDto> HandleAsync(Guid? accountId, Guid authIdentityId, CancellationToken cancellation)
        {
            var identityId = authIdentityId;
            DomainAccount domainAccount = null;
            if (accountId is not null)
            {
                domainAccount = await _dbContext.DomainAccounts.Include(acc => acc.Domain).ThenInclude(domain => domain.Contract).FirstOrDefaultAsync(account => account.Id == accountId);
                if (domainAccount is null)
                    return null;

                identityId = domainAccount.IdentityId;
            }

            var identityWithImage = await _dbContext.Identities.Where(id => id.Id == identityId).GroupJoin(
                _dbContext.CroppedImages,
                id => id.ImageId,
                im => im.Id,
                (id, ims) => new {Identity = id, Images = ims}
            ).SelectMany(
                result => result.Images.DefaultIfEmpty(),
                (result, image) => new { result.Identity, Image = image }
            ).FirstOrDefaultAsync();

            return new IdentityDetailDto
            {
                IdentityId = identityWithImage.Identity.Id,
                Email = identityWithImage.Identity.Email,
                FirstName = identityWithImage.Identity.FirstName,
                LastName = identityWithImage.Identity.LastName,
                ImageId = identityWithImage.Image == null ? null : identityWithImage.Image.ImageId,
                CroppedImageId = identityWithImage.Image == null ? null : identityWithImage.Image.Id,
                CropParameters = mapCropParameters(identityWithImage.Image?.CropParameters),
                AccountId = accountId,
                IsAdmin = domainAccount?.IsAdmin,
                IsIssuer = domainAccount?.IsIssuer,
                IsOwner = domainAccount?.IsOwner,
            };
        }
    }
}
