using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Entities;

namespace Vouchers.EntityFramework.Repositories;

internal sealed class GenericRepository<TEntity, TKey> : Repository<TEntity, TKey> where TEntity : Entity<TKey>
{
    public GenericRepository(VouchersDbContext context) : base(context)
    {
    }
}