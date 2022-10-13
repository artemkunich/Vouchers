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
    internal sealed class HolderTransactionRequestsQueryHandler : IAuthIdentityHandler<HolderTransactionRequestsQuery,IEnumerable<HolderTransactionRequestDto>>
    {
        readonly VouchersDbContext _dbContext;

        public HolderTransactionRequestsQueryHandler(VouchersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<HolderTransactionRequestDto> Handle(HolderTransactionRequestsQuery query, Guid authIdentityId) =>
            GetQuery(query, authIdentityId).ToList();

        public async Task<IEnumerable<HolderTransactionRequestDto>> HandleAsync(HolderTransactionRequestsQuery query, Guid authIdentityId, CancellationToken cancellation) =>
            await GetQuery(query, authIdentityId).ToListAsync();

        public IQueryable<HolderTransactionRequestDto> GetQuery(HolderTransactionRequestsQuery query, Guid authIdentityId) {

            var holderTransactionRequestsWithDomainAccountsQuery = _dbContext.HolderTransactionRequests
                .Include(req => req.CreditorAccount)
                .Include(req => req.DebtorAccount)
                .Include(req => req.Quantity.UnitType).ThenInclude(unit => unit.IssuerAccount)
                .Include(req => req.Transaction)
                .Join(
                    _dbContext.DomainAccounts.Join(_dbContext.Identities, a => a.IdentityId, i => i.Id, (a, i) => new { a.Id, DomainAccount = a, Identity = i }),
                    req => req.DebtorAccountId,
                    debtor => debtor.Id,
                    (req, debtor) => new
                    {
                        TransactionRequest = req,
                        DebtorDomainAccount = debtor.DomainAccount,
                        DebtorIdentity = debtor.Identity,
                    }
                ).GroupJoin(
                    _dbContext.DomainAccounts.Join(_dbContext.Identities, a => a.IdentityId, i => i.Id, (a, i) => new { a.Id, DomainAccount = a, Identity = i }),
                    o => o.TransactionRequest.CreditorAccountId,
                    creditor => creditor.Id,
                    (req, creditors) => new
                    {
                        req.TransactionRequest,
                        req.DebtorDomainAccount,
                        req.DebtorIdentity,
                        CreditorDomainAccounts = creditors
                    }
                ).SelectMany(
                    req => req.CreditorDomainAccounts.DefaultIfEmpty(),
                    (req, o) => new { req.TransactionRequest, req.DebtorDomainAccount, req.DebtorIdentity, CreditorDomainAccount = o.DomainAccount, CreditorIdentity = o.Identity }
                ).GroupJoin(
                    _dbContext.HolderTransactions.Join(_dbContext.DomainAccounts, tr => tr.CreditorAccountId, acc => acc.Id, (tr, acc) => new { tr.Id, CreditorDomainAccount = acc }),
                    req => req.TransactionRequest.TransactionId,
                    t => t.Id,
                    (req, ts) => new { req.TransactionRequest, req.DebtorDomainAccount, req.DebtorIdentity, req.CreditorDomainAccount, req.CreditorIdentity, Transactions = ts }
                ).SelectMany(
                    req => req.Transactions.DefaultIfEmpty(),
                    (req, t) => new { req.TransactionRequest, req.DebtorDomainAccount, req.DebtorIdentity, req.CreditorDomainAccount, req.CreditorIdentity, Transaction = t }
                );

            holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(
                o => o.DebtorDomainAccount.IdentityId == authIdentityId 
                || o.CreditorDomainAccount != null && o.CreditorDomainAccount.IdentityId == authIdentityId 
                || o.Transaction.CreditorDomainAccount.IdentityId == authIdentityId
            );

            if (!query.IncludeIncoming)
                holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(
                    o => o.DebtorDomainAccount.IdentityId == authIdentityId
                );
            
            if (!query.IncludeOutgoing)
                holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(
                    o => o.CreditorDomainAccount != null && o.CreditorDomainAccount.IdentityId == authIdentityId
                    || o.Transaction.CreditorDomainAccount.IdentityId == authIdentityId
                );

            if (!query.IncludePaid)
                holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(
                    o => o.TransactionRequest.TransactionId == null
                );

            if (!query.IncludeUnpaid)
                holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(
                    o => o.TransactionRequest.TransactionId != null
                );

            //var holderTransactionRequestsQuery = holderTransactionRequestsWithDomainAccountsQuery.Select(o => o.TransactionRequest);

            if (query.MinAmount != null)
                holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(req => req.TransactionRequest.Quantity.Amount >= query.MinAmount);
            if (query.MaxAmount != null)
                holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(req => req.TransactionRequest.Quantity.Amount <= query.MaxAmount);

            if (query.MinDueDate != null)
                holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(req => req.TransactionRequest.DueDate >= query.MinDueDate);
            if (query.MaxDueDate != null)
                holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(req => req.TransactionRequest.DueDate <= query.MaxDueDate);

            var voucherValuesQuery = _dbContext.VoucherValues.AsQueryable();
            if (query.Ticker != null)
                voucherValuesQuery = voucherValuesQuery.Where(value => value.Ticker.Contains(query.Ticker));          

            var holderTransactionRequestsWithDomainAccountsAndUnitQuery = holderTransactionRequestsWithDomainAccountsQuery.Join(
                voucherValuesQuery,
                req => req.TransactionRequest.Quantity.UnitTypeId,
                v => v.Id,
                (req, v) => new
                {
                    req.TransactionRequest,
                    req.CreditorDomainAccount,                   
                    req.CreditorIdentity,                   
                    req.DebtorDomainAccount,
                    req.DebtorIdentity,
                    Value = v
                }
            ).Join(
                _dbContext.UnitTypes,
                req => req.TransactionRequest.Quantity.UnitTypeId,
                u => u.Id,
                (req, u) => new
                {
                    req.TransactionRequest,
                    req.CreditorDomainAccount,
                    req.CreditorIdentity,
                    req.DebtorDomainAccount,
                    req.DebtorIdentity,
                    req.Value,
                    UnitType = u
                }
            ); ;

            var holderTransactionRequestsWithDomainAccountsAndUnitAndUnitImageQuery = holderTransactionRequestsWithDomainAccountsAndUnitQuery.Join(_dbContext.Identities,
                req => req.Value.IssuerIdentityId,
                issuer => issuer.Id,
                (req, issuer) => new
                {
                    req.TransactionRequest,
                    req.CreditorDomainAccount,
                    req.CreditorIdentity,
                    req.DebtorDomainAccount,
                    req.DebtorIdentity,
                    req.Value,
                    req.UnitType,
                    UnitIssuer = issuer
                }
            );

            if (query.IssuerName is not null)
                holderTransactionRequestsWithDomainAccountsAndUnitAndUnitImageQuery = holderTransactionRequestsWithDomainAccountsAndUnitAndUnitImageQuery.Where(o => (o.UnitIssuer.FirstName + o.UnitIssuer.LastName).Contains(query.IssuerName));

            return holderTransactionRequestsWithDomainAccountsAndUnitAndUnitImageQuery.Select(req =>
                new HolderTransactionRequestDto
                {
                    Id = req.TransactionRequest.Id,
                    DueDate = req.TransactionRequest.DueDate,
                    CreditorAccountId = req.TransactionRequest.CreditorAccountId,
                    DebtorAccountId = req.TransactionRequest.DebtorAccountId,
                    CounterpartyName = req.DebtorIdentity.Id == authIdentityId ? (req.CreditorIdentity == null ? null : req.CreditorIdentity.FirstName + " " + req.CreditorIdentity.LastName) : req.DebtorIdentity.FirstName + " " + req.DebtorIdentity.LastName,
                    CounterpartyEmail = req.DebtorIdentity.Id == authIdentityId ? (req.CreditorIdentity == null ? null : req.CreditorIdentity.Email) : req.DebtorIdentity.Email,
                    CounterpartyImageId = req.DebtorIdentity.Id == authIdentityId ? (req.CreditorIdentity == null ? null : req.CreditorIdentity.ImageId) : req.DebtorIdentity.ImageId,
                    UnitTypeId = req.Value.Id,
                    UnitImageId = req.Value.ImageId,
                    UnitTicker = req.Value.Ticker,
                    UnitIssuerAccountId = req.UnitType.IssuerAccountId,
                    UnitIssuerEmail = req.UnitIssuer.Email,
                    UnitIssuerName = req.UnitIssuer.FirstName + " " + req.UnitIssuer.LastName,
                    Amount = req.TransactionRequest.Quantity.Amount,
                    MaxDaysBeforeValidityStart = req.TransactionRequest.MaxDurationBeforeValidityStart == null ? 0 : req.TransactionRequest.MinDurationBeforeValidityEnd.Value.Days,
                    MinDaysBeforeValidityEnd = req.TransactionRequest.MinDurationBeforeValidityEnd == null ? 0 : req.TransactionRequest.MinDurationBeforeValidityEnd.Value.Days,
                    MustBeExchangeable = req.TransactionRequest.MustBeExchangeable,
                    Message = req.TransactionRequest.Message,
                    TransactionId = req.TransactionRequest.TransactionId,
                }
            );
        }
    }
}
