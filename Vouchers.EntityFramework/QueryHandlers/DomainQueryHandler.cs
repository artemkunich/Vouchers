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
            var domainDetail = await dbContext.DomainDetails
            .Include(domainDetail => domainDetail.Contract).ThenInclude(contract => contract.Domain)
            .Where(domainDetail => domainDetail.Contract.Domain.Id == query.DomainId)
            .FirstOrDefaultAsync();

            return new DomainDto
            {
                Id = domainDetail.Contract.Domain.Id,
                Name = domainDetail.Contract.DomainName,
                Description = domainDetail.Description
            };
        }
    }
}
