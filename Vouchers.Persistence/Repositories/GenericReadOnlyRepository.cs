using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Primitives;
using Vouchers.Persistence.InterCommunication;

namespace Vouchers.Persistence.Repositories;

internal class GenericReadOnlyRepository<TEntity, TKey> : ReadOnlyRepository<TEntity, TKey> where TEntity: class, IEntity<TKey>
{
    public GenericReadOnlyRepository(VouchersDbContext dbContext) : base(dbContext)
    {
    }
}