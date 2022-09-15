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
using Vouchers.Core;

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
                .Include(tr => tr.Creditor)
                .Include(tr => tr.Debtor)
                .Include(tr => tr.Quantity.UnitType).ThenInclude(unit => unit.Issuer)
                .Include(tr => tr.TransactionItems)
                .Join(
                    dbContext.DomainAccounts,
                    tr => tr.DebtorId,
                    debtor => debtor.Id,
                    (tr, debtor) => new 
                    { 
                        Transaction = tr,
                        DebtorDomainAccount = debtor
                    }
                )
                .Join(
                    dbContext.DomainAccounts,
                    o => o.Transaction.CreditorId,
                    creditor => creditor.Id,
                    (tr, creditor) => new
                    {
                        Transaction = tr.Transaction,
                        DebtorDomainAccount = tr.DebtorDomainAccount,
                        CreditorDomainAccount = creditor
                    }
                )
                .Where(o => o.DebtorDomainAccount.IdentityId == authIdentityId || o.CreditorDomainAccount.IdentityId == authIdentityId).Select(o => o.Transaction);

            if (query.MinAmount != null)
                holderTransactionsQuery = holderTransactionsQuery.Where(transaction => transaction.Quantity.Amount >= query.MinAmount);
            if (query.MaxAmount != null)
                holderTransactionsQuery = holderTransactionsQuery.Where(transaction => transaction.Quantity.Amount <= query.MaxAmount);

            if (query.MinTimestamp != null)
                holderTransactionsQuery = holderTransactionsQuery.Where(transaction => transaction.Timestamp >= query.MinTimestamp);
            if (query.MaxTimestamp != null)
                holderTransactionsQuery = holderTransactionsQuery.Where(transaction => transaction.Timestamp <= query.MaxTimestamp);

            var voucherValuesQuery = dbContext.VoucherValues.AsQueryable();
            if (query.Ticker != null)
                voucherValuesQuery = voucherValuesQuery.Where(value => value.Ticker.Contains(query.Ticker));

            return holderTransactionsQuery.Join(
                voucherValuesQuery,
                t => t.Quantity.UnitTypeId,
                v => v.Id,
                (t, v) => new 
                { 
                    Transaction = t,
                    Value = v
                }
            ).Join(
                dbContext.DomainAccounts.Join(dbContext.Identities, a => a.IdentityId, i => i.Id, (a, i) => new { DomainAccountId = a.Id, Identity = i }),
                t => t.Transaction.CreditorId,
                i => i.DomainAccountId,
                (t, i) => new
                {
                    Transaction = t.Transaction,
                    Value = t.Value,
                    Creditor = i.Identity
                }              
            ).Join(
                dbContext.DomainAccounts.Join(dbContext.Identities, a => a.IdentityId, i => i.Id, (a, i) => new { DomainAccountId = a.Id, Identity = i }),
                t => t.Transaction.DebtorId,
                i => i.DomainAccountId,
                (t, i) => new HolderTransactionDto
                {
                    Id = t.Transaction.Id,
                    Timestamp = t.Transaction.Timestamp,
                    CreditorId = t.Transaction.Creditor.Id,
                    CreditorName = t.Creditor.FirstName + " " + t.Creditor.LastName,
                    DebtorId = t.Transaction.Debtor.Id,
                    DebtorName = i.Identity.FirstName + " " + i.Identity.LastName,
                    UnitId = t.Value.Id,
                    UnitTicker = t.Value.Ticker,
                    UnitIssuerId = t.Value.IssuerIdentityId,
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
                    })
                }
            );
        }
    }
}
