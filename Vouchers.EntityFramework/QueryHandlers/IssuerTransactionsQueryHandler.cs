﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using System.Threading;
using Vouchers.Application.Services;

namespace Vouchers.EntityFramework.QueryHandlers
{
    internal sealed class IssuerTransactionsQueryHandler : IHandler<IssuerTransactionsQuery,IEnumerable<IssuerTransactionDto>>
    {
        private readonly IAuthIdentityProvider _authIdentityProvider;
        private readonly VouchersDbContext _dbContext;

        public IssuerTransactionsQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext)
        {
            _authIdentityProvider = authIdentityProvider;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<IssuerTransactionDto>> HandleAsync(IssuerTransactionsQuery query, CancellationToken cancellation)
        {
            var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
            return await GetQuery(query, authIdentityId).ToListAsync();
        }
            
        IQueryable<IssuerTransactionDto> GetQuery(IssuerTransactionsQuery query, Guid authIdentityId) 
        {

            var issuerTransactionsQuery = _dbContext.IssuerTransactions
                .Include(tr => tr.Quantity.Unit).ThenInclude(unit => unit.UnitType).ThenInclude(value => value.IssuerAccount)
                .Join(_dbContext.VoucherValues, 
                    t => t.Quantity.Unit.UnitTypeId,
                    v => v.Id,
                    (t,v) => new { Transaction = t, Value = v }
                )
                .Where(o => o.Value.IssuerIdentityId == authIdentityId).Select(o => o.Transaction);

            if (query.MinAmount != null)
                issuerTransactionsQuery.Where(tr => tr.Quantity.Amount >= query.MinAmount);
            if (query.MaxAmount != null)
                issuerTransactionsQuery.Where(tr => tr.Quantity.Amount <= query.MaxAmount);

            if (query.MinTimestamp != null)
                issuerTransactionsQuery.Where(tr => tr.Timestamp >= query.MinTimestamp);
            if (query.MaxTimestamp != null)
                issuerTransactionsQuery.Where(tr => tr.Timestamp <= query.MaxTimestamp);

            var voucherValuesQuery = _dbContext.VoucherValues
                .Where(value => value.IssuerIdentityId == authIdentityId);

            return issuerTransactionsQuery.Join(
                voucherValuesQuery,
                t => t.Quantity.Unit.UnitTypeId,
                v => v.Id,
                (t, v) => new IssuerTransactionDto {
                    Id = t.Id,
                    Timestamp = t.Timestamp,
                    UnitTicker = v.Ticker,
                    Unit = new VoucherDto
                    {
                        Id = v.Id,
                        ValidFrom = t.Quantity.Unit.ValidFrom,
                        ValidTo = t.Quantity.Unit.ValidTo,
                        CanBeExchanged = t.Quantity.Unit.CanBeExchanged,
                    },                 
                    Amount = t.Quantity.Amount
                }
            ).GetListPageQuery(query);
        }
    }
}