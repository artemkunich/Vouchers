using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Core;
using Vouchers.Domains;

namespace Vouchers.EntityFramework.Repositories;

internal sealed class HolderTransactionRequestRepository : Repository<HolderTransactionRequest, Guid>
{
    public HolderTransactionRequestRepository(VouchersDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<HolderTransactionRequest> GetByIdAsync(Guid id) => 
        await GetByIdQueryable(id).FirstOrDefaultAsync();
        
    private IQueryable<HolderTransactionRequest> GetByIdQueryable(Guid id) =>
        DbContext.HolderTransactionRequests
            .Include(request => request.Transaction)
            .Include(request => request.DebtorAccount)
            .Include(request => request.CreditorAccount)
            .Where(request => request.Id == id);

        
    public override async Task<IEnumerable<HolderTransactionRequest>> GetByExpressionAsync(Expression<Func<HolderTransactionRequest, bool>> expression) => 
        await GetByExpressionQueryable(expression).ToListAsync();
        
    private IQueryable<HolderTransactionRequest> GetByExpressionQueryable(Expression<Func<HolderTransactionRequest, bool>> expression) =>
        DbContext.HolderTransactionRequests
            .Include(request => request.Transaction)
            .Include(request => request.DebtorAccount)
            .Include(request => request.CreditorAccount)
            .Where(expression);
}