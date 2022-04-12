using System;
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

namespace Vouchers.EntityFramework.QueryHandlers
{
    public class IssuerTransactionsQueryHandler : IAuthIdentityHandler<IssuerTransactionsQuery,IEnumerable<IssuerTransactionDto>>
    {
        VouchersDbContext dbContext;

        public IssuerTransactionsQueryHandler(VouchersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<IssuerTransactionDto> Handle(IssuerTransactionsQuery query, Guid authIdentityId, CancellationToken cancellation) =>
            GetQuery(query, authIdentityId).ToList();

        public async Task<IEnumerable<IssuerTransactionDto>> HandleAsync(IssuerTransactionsQuery query, Guid authIdentityId, CancellationToken cancellation) =>
            await GetQuery(query, authIdentityId).ToListAsync();

        IQueryable<IssuerTransactionDto> GetQuery(IssuerTransactionsQuery query, Guid authIdentityId) 
        {

            var issuerTransactionsQuery = dbContext.IssuerTransactions
                .Include(tr => tr.Quantity.Unit)
                    .ThenInclude(unit => unit.Value)
                        .ThenInclude(value => value.Issuer)
                            .ThenInclude(issuer => issuer.Identity)
                .Where(tr => tr.Quantity.Unit.Value.Issuer.Identity.Id == authIdentityId);

            if (query.MinAmount != null)
                issuerTransactionsQuery.Where(tr => tr.Quantity.Amount >= query.MinAmount);
            if (query.MaxAmount != null)
                issuerTransactionsQuery.Where(tr => tr.Quantity.Amount <= query.MaxAmount);

            if (query.MinTimestamp != null)
                issuerTransactionsQuery.Where(tr => tr.Timestamp >= query.MinTimestamp);
            if (query.MaxTimestamp != null)
                issuerTransactionsQuery.Where(tr => tr.Timestamp <= query.MaxTimestamp);

            var voucherValueDetailsQuery = dbContext.VoucherValueDetails
                .Include(detail => detail.Value)
                    .ThenInclude(value => value.Issuer)
                        .ThenInclude(issuer => issuer.Identity)
                .Where(valueDetail => valueDetail.Value.Issuer.Identity.Id == authIdentityId);

            return issuerTransactionsQuery.Join(
                voucherValueDetailsQuery,
                t => t.Quantity.Unit.Value.Id,
                v => v.Value.Id,
                (t, v) => new IssuerTransactionDto {
                    Id = t.Id,
                    Timestamp = t.Timestamp,
                    UnitTicker = v.Ticker,
                    ValidFrom = t.Quantity.Unit.ValidFrom,
                    ValidTo = t.Quantity.Unit.ValidTo,
                    CanBeExchanged = t.Quantity.Unit.CanBeExchanged,
                    Amount = t.Quantity.Amount
                }
            );
        }
    }
}
