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
    public class DomainQueryHandler : IAuthIdentityHandler<DomainQuery, DomainDto>
    {
        private readonly VouchersDbContext dbContext;

        public DomainQueryHandler(VouchersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<DomainDto> HandleAsync(DomainQuery query, Guid authIdentityId, CancellationToken cancellation)
        {
            return await dbContext.Domains
                .Include(domain => domain.Contract)
                .Where(domain => domain.Id == query.DomainId)
                .Select(domain => new DomainDto
                {
                    Id = domain.Id,
                    Name = domain.Contract.DomainName,
                    Description = domain.Description
                })
                .FirstOrDefaultAsync();
        }
    }
}
