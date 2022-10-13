using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Entities;

namespace Vouchers.EntityFramework.Repositories
{
    public sealed class GenericRepository<TEntity> : Repository<TEntity> where TEntity : Entity
    {
        public GenericRepository(VouchersDbContext context) : base(context)
        {
        }
    }
}
