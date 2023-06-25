using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Vouchers.Common.Application.Dtos;
using Vouchers.Common.Application.Queries;
using Vouchers.Common.Application.UseCases;
using System.Threading;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Common.Application.Services;
using Vouchers.Core.Domain;

namespace Vouchers.Persistence.QueryHandlers;

internal sealed class IssuerTransactionsQueryHandler : IRequestHandler<IssuerTransactionsQuery,IReadOnlyList<IssuerTransactionDto>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly VouchersDbContext _dbContext;

    public IssuerTransactionsQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext)
    {
        _authIdentityProvider = authIdentityProvider;
        _dbContext = dbContext;
    }

    public async Task<Result<IReadOnlyList<IssuerTransactionDto>>> HandleAsync(IssuerTransactionsQuery query, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        return await GetQuery(query, authIdentityId).ToListAsync(cancellation);
    }
            
    IQueryable<IssuerTransactionDto> GetQuery(IssuerTransactionsQuery query, Guid authIdentityId) 
    {
        var issuerTransactionsQuery = _dbContext.Set<IssuerTransaction>()
            .Include(tr => tr.IssuerAccountItem.Unit).ThenInclude(unit => unit.UnitType).ThenInclude(value => value.IssuerAccount)
            .Join(_dbContext.Set<VoucherValue>(), 
                t => t.IssuerAccountItem.Unit.UnitTypeId,
                v => v.Id,
                (t,v) => new { Transaction = t, Value = v }
            )
            .Where(o => o.Value.IssuerIdentityId == authIdentityId).Select(o => o.Transaction);

        if (query.MinAmount != null)
            issuerTransactionsQuery = issuerTransactionsQuery.Where(tr => tr.Amount >= query.MinAmount);
        if (query.MaxAmount != null)
            issuerTransactionsQuery = issuerTransactionsQuery.Where(tr => tr.Amount <= query.MaxAmount);

        if (query.MinTimestamp != null)
            issuerTransactionsQuery = issuerTransactionsQuery.Where(tr => tr.Timestamp >= query.MinTimestamp);
        if (query.MaxTimestamp != null)
            issuerTransactionsQuery = issuerTransactionsQuery.Where(tr => tr.Timestamp <= query.MaxTimestamp);

        var voucherValuesQuery = _dbContext.Set<VoucherValue>()
            .Where(value => value.IssuerIdentityId == authIdentityId);

        return issuerTransactionsQuery.Join(
            voucherValuesQuery,
            t => t.IssuerAccountItem.Unit.UnitTypeId,
            v => v.Id,
            (t, v) => new IssuerTransactionDto {
                Id = t.Id,
                Timestamp = t.Timestamp,
                UnitTicker = v.Ticker,
                Unit = new VoucherDto
                {
                    Id = v.Id,
                    ValidFrom = t.IssuerAccountItem.Unit.ValidFrom,
                    ValidTo = t.IssuerAccountItem.Unit.ValidTo,
                    CanBeExchanged = t.IssuerAccountItem.Unit.CanBeExchanged,
                },                 
                Amount = t.Amount
            }
        ).GetListPageQuery(query);
    }
}