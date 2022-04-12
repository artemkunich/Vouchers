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
    public class HolderTransactionsQueryHandler : IAuthIdentityHandler<HolderTransactionsQuery,IEnumerable<HolderTransactionDto>>
    {
        VouchersDbContext dbContext;

        public HolderTransactionsQueryHandler(VouchersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<HolderTransactionDto> Handle(HolderTransactionsQuery query, Guid authIdentityId) =>
            GetQuery(query, authIdentityId).ToList();

        public async Task<IEnumerable<HolderTransactionDto>> HandleAsync(HolderTransactionsQuery query, Guid authIdentityId, CancellationToken cancellation) =>
            await GetQuery(query, authIdentityId).ToListAsync();

        public IQueryable<HolderTransactionDto> GetQuery(HolderTransactionsQuery query, Guid authIdentityId) {

            var holderTransactionsQuery = dbContext.HolderTransactions
                .Include(tr => tr.Creditor).ThenInclude(cr => cr.Identity)
                .Include(tr => tr.Debtor).ThenInclude(dr => dr.Identity)
                .Include(tr => tr.Quantity.Unit).ThenInclude(unit => unit.Issuer).ThenInclude(issuer => issuer.Identity)
                .Include(tr => tr.TransactionItems)
                .Where(tr => tr.Quantity.Unit.Issuer.Identity.Id == authIdentityId);

            if (query.MinAmount != null)
                holderTransactionsQuery = holderTransactionsQuery.Where(transaction => transaction.Quantity.Amount >= query.MinAmount);
            if (query.MaxAmount != null)
                holderTransactionsQuery = holderTransactionsQuery.Where(transaction => transaction.Quantity.Amount <= query.MaxAmount);

            if (query.MinTimestamp != null)
                holderTransactionsQuery = holderTransactionsQuery.Where(transaction => transaction.Timestamp >= query.MinTimestamp);
            if (query.MaxTimestamp != null)
                holderTransactionsQuery = holderTransactionsQuery.Where(transaction => transaction.Timestamp <= query.MaxTimestamp);

            var voucherValueDetailsQuery = dbContext.VoucherValueDetails
                .Include(v => v.Value)
                .ThenInclude(v => v.Issuer)
                .AsQueryable();

            if (query.Ticker != null)
                voucherValueDetailsQuery = voucherValueDetailsQuery.Where(detail => detail.Ticker.Contains(query.Ticker));

            return holderTransactionsQuery.Join(
                voucherValueDetailsQuery,
                t => t.Quantity.Unit.Id,
                v => v.Value.Id,
                (t, v) => new { 
                    Transaction = t,
                    ValueDetail = v
                }
            ).Join(
                dbContext.IdentityDetails.Include(detail => detail.Identity),
                t => t.Transaction.Creditor.Identity.Id,
                d => d.Identity.Id,
                (t, d) => new
                {
                    Transaction = t.Transaction,
                    ValueDetail = t.ValueDetail,
                    Creditor = d
                }
                
            ).Join(
                dbContext.IdentityDetails.Include(detail => detail.Identity),
                t => t.Transaction.Debtor.Identity.Id,
                d => d.Identity.Id,
                (t, d) => new HolderTransactionDto
                {
                    Id = t.Transaction.Id,
                    Timestamp = t.Transaction.Timestamp,
                    CreditorId = t.Transaction.Creditor.Id,
                    CreditorName = t.Creditor.IdentityName,
                    DebtorId = t.Transaction.Debtor.Id,
                    DebtorName = d.IdentityName,
                    UnitId = t.ValueDetail.Id,
                    UnitTicker = t.ValueDetail.Ticker,
                    UnitIssuerId = t.ValueDetail.Value.Issuer.Id,
                    Amount = t.Transaction.Quantity.Amount,
                    Items = t.Transaction.TransactionItems.Select(item => new VoucherQuantityDto
                        {
                            Amount = item.Quantity.Amount,
                            Unit = new VoucherDto
                            {
                                Id = item.Quantity.Unit.Id,
                                ValidFrom = item.Quantity.Unit.ValidFrom,
                                ValidTo = item.Quantity.Unit.ValidTo,
                                CanBeExchanged = item.Quantity.Unit.CanBeExchanged,
                                Supply = item.Quantity.Unit.Supply,
                                Balance = 0
                            }
                        }
                    )
                }
            );
        }
    }
}
