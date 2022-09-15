using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Core;

namespace Vouchers.EntityFramework.Repositories
{
    public class CollectionRequestRepository : Repository<CollectionRequest>
    {
        public CollectionRequestRepository(VouchersDbContext dbContext) : base(dbContext)
        {
        }
    }
}
