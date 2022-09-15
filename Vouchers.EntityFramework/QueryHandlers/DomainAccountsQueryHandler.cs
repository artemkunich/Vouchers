using System;
using System.Collections.Generic;
using System.Text;

using System.Linq.Expressions;
using System.Linq;
using Vouchers.Application.Infrastructure;
using Vouchers.Identities;
using Vouchers.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using System.Threading;

namespace Vouchers.EntityFramework.QueryHandlers
{
    public class DomainAccountsQueryHandler : IAuthIdentityHandler<DomainAccountsQuery, IEnumerable<DomainAccountDto>>
    {
        VouchersDbContext _dbContext;

        public DomainAccountsQueryHandler(VouchersDbContext dbContext)
        {           
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DomainAccountDto>> HandleAsync(DomainAccountsQuery query, Guid authIdentityId, CancellationToken cancellation) =>
            await GetQuery(query, authIdentityId).ToListAsync();

        public IEnumerable<DomainAccountDto> Handle(DomainAccountsQuery query, Guid authIdentityId) =>
            GetQuery(query, authIdentityId).ToList();

        private IQueryable<DomainAccountDto> GetQuery(DomainAccountsQuery query, Guid authIdentityId) {

            var authIdentityDomainAccountQuery = _dbContext.DomainAccounts
                .Include(a => a.Domain)
                .Where(a => a.Domain.Id == query.DomainId && a.IdentityId == authIdentityId);

            var domainAccountsQuery = _dbContext.DomainAccounts
                .Include(a => a.Domain).ThenInclude(d => d.Contract)
                .Where(a => a.Domain.Id == query.DomainId && a.IdentityId != authIdentityId);

            if (query.IncludeConfirmed && !query.IncludeNotConfirmed)
                domainAccountsQuery = domainAccountsQuery.Where(a => a.IsConfirmed);
            if (!query.IncludeConfirmed && query.IncludeNotConfirmed)
                domainAccountsQuery = domainAccountsQuery.Where(a => !a.IsConfirmed);

            var resultQuery = domainAccountsQuery.Join(
                authIdentityDomainAccountQuery,
                a => a.Domain.Id,
                a => a.Domain.Id,
                (a, c) => new DomainAccountDto
                {
                    Id = a.Id,
                    DomainName = a.Domain.Contract.DomainName,
                    DomainId = a.DomainId,
                    IdentityId = a.IdentityId,
                    IsAdmin = a.IsAdmin,
                    IsIssuer = a.IsIssuer,
                    IsOwner = a.IdentityId == a.Domain.Contract.PartyId,
                    IsConfirmed = a.IsConfirmed,
                }
            );

            var identitiesQuery = _dbContext.Identities.AsQueryable();

            if (query.Email is not null)
                identitiesQuery = identitiesQuery.Where(identity => identity.Email.Contains(query.Email));
            
            if (query.Name is not null)
                identitiesQuery = identitiesQuery.Where(identity => (identity.FirstName + " " + identity.LastName).Contains(query.Name));

            var identitiesWithImageQuery = identitiesQuery.GroupJoin(
                _dbContext.Images.AsQueryable(),
                identity => identity.ImageId,
                image => image.Id,
                (identity, images) => new {
                    IdentityId = identity.Id,
                    Identity = identity,
                    Images = images
                }
            ).SelectMany(
                result => result.Images.DefaultIfEmpty(),
                (result, image) => new {result.IdentityId, result.Identity, Image=image}
            );

            return resultQuery.Join(
                identitiesWithImageQuery,
                a => a.IdentityId,
                i => i.IdentityId,
                (a, i) =>
                new DomainAccountDto()
                {
                    Id = a.Id,
                    DomainId = a.DomainId,
                    DomainName = a.DomainName,
                    IdentityId = a.IdentityId,
                    Email = i.Identity.Email,
                    Name = i.Identity.FirstName + " " + i.Identity.LastName,
                    IsAdmin = a.IsAdmin,
                    IsIssuer = a.IsIssuer,
                    IsOwner = a.IsOwner,
                    IsConfirmed = a.IsConfirmed,
                    ImageBase64 = i.Image == null ? null : Convert.ToBase64String(i.Image.CroppedContent)
                }
            );
        }
    }
}
