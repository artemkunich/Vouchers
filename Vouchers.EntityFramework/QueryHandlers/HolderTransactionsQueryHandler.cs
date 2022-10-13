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
    internal sealed class HolderTransactionsQueryHandler : IAuthIdentityHandler<HolderTransactionsQuery,IEnumerable<HolderTransactionDto>>
    {
        VouchersDbContext _dbContext;

        public HolderTransactionsQueryHandler(VouchersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<HolderTransactionDto> Handle(HolderTransactionsQuery query, Guid authIdentityId) =>
            GetQuery(query, authIdentityId).ToList();

        public async Task<IEnumerable<HolderTransactionDto>> HandleAsync(HolderTransactionsQuery query, Guid authIdentityId, CancellationToken cancellation) =>
            await GetQuery(query, authIdentityId).ToListAsync();

        public IQueryable<HolderTransactionDto> GetQuery(HolderTransactionsQuery query, Guid authIdentityId) {

            var holderTransactionsQuery = _dbContext.HolderTransactions
                .Include(tr => tr.CreditorAccount)
                .Include(tr => tr.DebtorAccount)
                .Include(tr => tr.Quantity.UnitType).ThenInclude(unit => unit.IssuerAccount)
                .Include(tr => tr.TransactionItems)
                .Join(
                    _dbContext.DomainAccounts,
                    tr => tr.DebtorAccountId,
                    debtor => debtor.Id,
                    (tr, debtor) => new 
                    { 
                        Transaction = tr,
                        DebtorDomainAccount = debtor
                    }
                )
                .Join(
                    _dbContext.DomainAccounts,
                    o => o.Transaction.CreditorAccountId,
                    creditor => creditor.Id,
                    (tr, creditor) => new
                    {
                        Transaction = tr.Transaction,
                        DebtorDomainAccount = tr.DebtorDomainAccount,
                        CreditorDomainAccount = creditor
                    }
                )
                .Where(o => o.DebtorDomainAccount.IdentityId == authIdentityId && o.DebtorDomainAccount.Id == query.AccountId || o.CreditorDomainAccount.IdentityId == authIdentityId && o.CreditorDomainAccount.Id == query.AccountId).Select(o => o.Transaction);     

            if (query.MinAmount != null)
                holderTransactionsQuery = holderTransactionsQuery.Where(transaction => transaction.Quantity.Amount >= query.MinAmount);
            if (query.MaxAmount != null)
                holderTransactionsQuery = holderTransactionsQuery.Where(transaction => transaction.Quantity.Amount <= query.MaxAmount);

            if (query.MinTimestamp != null)
                holderTransactionsQuery = holderTransactionsQuery.Where(transaction => transaction.Timestamp >= query.MinTimestamp);
            if (query.MaxTimestamp != null)
                holderTransactionsQuery = holderTransactionsQuery.Where(transaction => transaction.Timestamp <= query.MaxTimestamp);

            var voucherValuesQuery = _dbContext.VoucherValues.AsQueryable();
            if (query.Ticker != null)
                voucherValuesQuery = voucherValuesQuery.Where(value => value.Ticker.Contains(query.Ticker));

            var holderTransactionsQueryWithIdentities = holderTransactionsQuery.Join(
                voucherValuesQuery,
                t => t.Quantity.UnitTypeId,
                v => v.Id,
                (t, v) => new
                {
                    Transaction = t,
                    Value = v
                }
            ).Join(
                _dbContext.DomainAccounts.Join(_dbContext.Identities, a => a.IdentityId, i => i.Id, (a, i) => new { DomainAccountId = a.Id, Identity = i }),
                t => t.Transaction.CreditorAccountId,
                i => i.DomainAccountId,
                (t, i) => new
                {
                    Transaction = t.Transaction,
                    Value = t.Value,
                    Creditor = i.Identity
                }
            ).Join(
                _dbContext.Identities,
                res => res.Value.IssuerIdentityId,
                identity => identity.Id,
                (res, identity) => new {
                    res.Transaction,
                    res.Value,
                    res.Creditor,
                    UnitIssuer = identity
                }
            ).Join(
                _dbContext.DomainAccounts.Join(_dbContext.Identities, a => a.IdentityId, i => i.Id, (a, i) => new { DomainAccountId = a.Id, Identity = i }),
                t => t.Transaction.DebtorAccountId,
                i => i.DomainAccountId,
                (res, i) => new {
                    res.Transaction,
                    res.Value,
                    res.Creditor,
                    res.UnitIssuer,
                    Debtor = i.Identity
                }
            );

            if (query.CounterpartyName != null)
                holderTransactionsQueryWithIdentities = holderTransactionsQueryWithIdentities.Where(t =>
                    t.Debtor.Id == authIdentityId && ((t.Creditor.FirstName + " " + t.Creditor.LastName).Contains(query.CounterpartyName) || t.Creditor.Email.Contains(query.CounterpartyName)) ||
                    t.Creditor.Id == authIdentityId && ((t.Debtor.FirstName + " " + t.Debtor.LastName).Contains(query.CounterpartyName) || t.Debtor.Email.Contains(query.CounterpartyName)));

            return holderTransactionsQueryWithIdentities.Select(t => new HolderTransactionDto
            {
                Id = t.Transaction.Id,
                Timestamp = t.Transaction.Timestamp,
                CreditorAccountId = t.Transaction.CreditorAccount.Id,
                CreditorName = t.Creditor.FirstName + " " + t.Creditor.LastName,
                CreditorEmail = t.Creditor.Email,
                CreditorImageId = t.Creditor.ImageId,

                DebtorAccountId = t.Transaction.DebtorAccount.Id,
                DebtorName = t.Debtor.FirstName + " " + t.Debtor.LastName,
                DebtorEmail = t.Debtor.Email,
                DebtorImageId = t.Debtor.ImageId,

                UnitTypeId = t.Value.Id,
                UnitTicker = t.Value.Ticker,
                UnitImageId = t.Value.ImageId,

                UnitIssuerId = t.Value.IssuerIdentityId,
                UnitIssuerName = t.UnitIssuer.FirstName + " " + t.UnitIssuer.LastName,
                UnitIssuerEmail = t.UnitIssuer.Email,
                Amount = t.Transaction.Quantity.Amount,
                Message = t.Transaction.Message,
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
            ).OrderByDescending(value => value.Timestamp);

            

            
        }
    }
}
