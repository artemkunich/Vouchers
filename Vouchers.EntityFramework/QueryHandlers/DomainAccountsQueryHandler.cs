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
    internal sealed class DomainAccountsQueryHandler : IAuthIdentityHandler<DomainAccountsQuery, IEnumerable<DomainAccountDto>>
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
                (a, c) => new
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

            return resultQuery.Join(
                identitiesQuery,
                a => a.IdentityId,
                i => i.Id,
                (account, identity) =>
                new DomainAccountDto()
                {
                    Id = account.Id,
                    DomainId = account.DomainId,
                    DomainName = account.DomainName,
                    Email = identity.Email,
                    Name = identity.FirstName + " " + identity.LastName,
                    IsAdmin = account.IsAdmin,
                    IsIssuer = account.IsIssuer,
                    IsOwner = account.IsOwner,
                    IsConfirmed = account.IsConfirmed,
                    ImageId = identity.ImageId
                }
            );
        }
    }
}
