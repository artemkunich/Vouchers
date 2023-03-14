using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Persistence.InterCommunication;
using Vouchers.Primitives;

namespace Vouchers.Persistence.Repositories;

internal sealed class GenericRepository<TAggregateRoot, TKey> : Repository<TAggregateRoot, TKey> where TAggregateRoot : AggregateRoot<TKey>
{
    public GenericRepository(VouchersDbContext dbContext) : base(dbContext)
    {
    }
}