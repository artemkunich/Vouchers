using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.UseCases;

namespace Vouchers.EntityFramework.QueryHandlers
{
    public class IdentityDetailQueryHandler : IAuthIdentityHandler<Guid?, IdentityDetailDto>
    {
        private readonly VouchersDbContext dbContext;

        public IdentityDetailQueryHandler(VouchersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IdentityDetailDto> HandleAsync(Guid? identityId, Guid authIdentityId, CancellationToken cancellation)
        {
            if (identityId is null)
                identityId = authIdentityId;

            var identityDetail = await dbContext.IdentityDetails.Include(login => login.Identity).Where(id => id.Identity.Id == identityId).FirstOrDefaultAsync();

            return new IdentityDetailDto
            {
                Email = identityDetail.Email,
                FirstName = identityDetail.FirstName,
                LastName = identityDetail.LastName,
                IdentityName = identityDetail.IdentityName
            };
        }
    }
}
