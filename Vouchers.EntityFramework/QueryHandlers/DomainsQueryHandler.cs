using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using Vouchers.Core;

namespace Vouchers.EntityFramework.QueryHandlers
{
    public class DomainsQueryHandler : IAuthIdentityHandler<DomainsQuery, IEnumerable<DomainDto>>
    {
        private readonly VouchersDbContext dbContext;

        public DomainsQueryHandler(VouchersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<DomainDto>> HandleAsync(DomainsQuery query, Guid authIdentityId, CancellationToken cancellation)
        {
             var domainsQuery = dbContext.Domains
                .Include(domain => domain.Contract)
                .Where(domain => domain.Contract.DomainName
                .Contains(query.Name))
                .Select(domain => new DomainDto
                {
                    Id = domain.Id,
                    Name = domain.Contract.DomainName,
                    Description = domain.Description,
                    IsPublic = domain.IsPublic,
                    MembersCount = domain.MembersCount
                });

            domainsQuery = domainsQuery.GroupJoin(
                dbContext.DomainAccounts.Where(domain => domain.IdentityId == authIdentityId),
                domain => domain.Id,
                account => account.Domain.Id,
                (domain, accounts) => new { Domain = domain, Accounts = accounts }
            ).SelectMany(
                result => result.Accounts.DefaultIfEmpty(),
                (result, account) => new { result.Domain, account }
            ).Where(domain => domain.account == null).Select(domain => domain.Domain);
            
            return await domainsQuery.ToListAsync();
        }
    }
}
