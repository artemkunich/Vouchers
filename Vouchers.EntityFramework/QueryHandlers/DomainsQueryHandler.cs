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
    internal sealed class DomainsQueryHandler : IAuthIdentityHandler<DomainsQuery, IEnumerable<DomainDto>>
    {
        private readonly VouchersDbContext _dbContext;

        public DomainsQueryHandler(VouchersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DomainDto>> HandleAsync(DomainsQuery query, Guid authIdentityId, CancellationToken cancellation)
        {
            var domainsQuery = _dbContext.Domains
               .Include( domain => domain.Contract)
               .Select( domain =>
               new DomainDto {
                    Id = domain.Id,
                    Name = domain.Contract.DomainName,
                    Description = domain.Description,
                    IsPublic = domain.IsPublic,
                    MembersCount = domain.MembersCount,
                    ImageId = domain.ImageId
                }
            );

            if(!string.IsNullOrEmpty(query.Name))
                domainsQuery = domainsQuery.Where(domain => domain.Name.Contains(query.Name));

            domainsQuery = domainsQuery.GroupJoin(
                _dbContext.DomainAccounts.Where(domain => domain.IdentityId == authIdentityId),
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
