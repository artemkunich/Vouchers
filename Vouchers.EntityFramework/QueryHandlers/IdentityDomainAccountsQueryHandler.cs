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
    public class IdentityDomainAccountsQueryHandler : IAuthIdentityHandler<IdentityDomainAccountsQuery, IEnumerable<DomainAccountDto>>
    {
        VouchersDbContext dbContext;

        public IdentityDomainAccountsQueryHandler(VouchersDbContext dbContext)
        {           
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<DomainAccountDto>> HandleAsync(IdentityDomainAccountsQuery query, Guid authIdentityId, CancellationToken cancellation) =>
            await GetQuery(query, authIdentityId).ToListAsync();

        public IEnumerable<DomainAccountDto> Handle(IdentityDomainAccountsQuery query, Guid authIdentityId) =>
            GetQuery(query, authIdentityId).ToList();

        private IQueryable<DomainAccountDto> GetQuery(IdentityDomainAccountsQuery query, Guid authIdentityId) 
        {
            var domainAccountsQuery = dbContext.DomainAccounts
                .Include(a => a.Domain).ThenInclude(d => d.Contract)
                .Where(a => a.IdentityId == authIdentityId && a.IsConfirmed);

            if (query.DomainName is not null)
                domainAccountsQuery = domainAccountsQuery.Where(d => d.Domain.Contract.DomainName.Contains(query.DomainName));

            return domainAccountsQuery.Select(a =>
                new DomainAccountDto
                {
                    Id = a.Id,
                    DomainId = a.Domain.Id,
                    DomainName = a.Domain.Contract.DomainName,
                    IsAdmin = a.IsAdmin,
                    IsIssuer = a.IsIssuer,
                    IsOwner = a.IdentityId == a.Domain.Contract.PartyId
                }
            );
        }
    }
}
