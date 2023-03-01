using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using Vouchers.Application.Services;
using System.Threading;
using Vouchers.Application;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Infrastructure;
using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;
using Vouchers.Identities.Domain;
using Vouchers.Values.Domain;

namespace Vouchers.Persistence.QueryHandlers;

internal sealed class HolderTransactionRequestsQueryHandler : IRequestHandler<HolderTransactionRequestsQuery,IReadOnlyList<HolderTransactionRequestDto>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly VouchersDbContext _dbContext;

    public HolderTransactionRequestsQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext)
    {
        _authIdentityProvider = authIdentityProvider;
        _dbContext = dbContext;
    }

    public async Task<Result<IReadOnlyList<HolderTransactionRequestDto>>> HandleAsync(HolderTransactionRequestsQuery query, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        return await GetQuery(query, authIdentityId).ToListAsync(cancellation);
    }

    private IQueryable<HolderTransactionRequestDto> GetQuery(HolderTransactionRequestsQuery query, Guid authIdentityId) {

        var holderTransactionRequestsWithDomainAccountsQuery = _dbContext.Set<HolderTransactionRequest>()
            .Include(req => req.CreditorAccount)
            .Include(req => req.DebtorAccount)
            .Include(req => req.Quantity.UnitType).ThenInclude(unit => unit.IssuerAccount)
            .Include(req => req.Transaction)
            .Join(
                _dbContext.Set<DomainAccount>().Join(_dbContext.Set<Identity>(), a => a.IdentityId, i => i.Id, (a, i) => new { a.Id, DomainAccount = a, Identity = i }),
                req => req.Transaction == null ? req.DebtorAccountId : req.Transaction.DebtorAccountId,
                debtor => debtor.Id,
                (req, debtor) => new
                {
                    TransactionRequest = req,
                    DebtorDomainAccount = debtor.DomainAccount,
                    DebtorIdentity = debtor.Identity,
                }
            ).GroupJoin(
                _dbContext.Set<DomainAccount>().Join(_dbContext.Set<Identity>(), a => a.IdentityId, i => i.Id, (a, i) => new { a.Id, DomainAccount = a, Identity = i }),
                o => o.TransactionRequest.Transaction == null ? o.TransactionRequest.CreditorAccountId : o.TransactionRequest.Transaction.CreditorAccountId,
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
            );

        holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(
            o => o.DebtorDomainAccount.IdentityId == authIdentityId 
                 || o.CreditorDomainAccount != null && o.CreditorDomainAccount.IdentityId == authIdentityId 
        );

        if (!query.IncludeIncoming)
            holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(
                o => o.DebtorDomainAccount.IdentityId == authIdentityId
            );
            
        if (!query.IncludeOutgoing)
            holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(
                o => o.CreditorDomainAccount != null && o.CreditorDomainAccount.IdentityId == authIdentityId
            );

        if (!query.IncludePerformed)
            holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(
                o => o.TransactionRequest.TransactionId == null
            );

        if (!query.IncludeNotPerformed)
            holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(
                o => o.TransactionRequest.TransactionId != null
            );

        if (query.MinAmount != null)
            holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(req => req.TransactionRequest.Quantity.Amount >= query.MinAmount);
        if (query.MaxAmount != null)
            holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(req => req.TransactionRequest.Quantity.Amount <= query.MaxAmount);

        if (query.MinDueDate != null)
            holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(req => req.TransactionRequest.DueDate >= query.MinDueDate);
        if (query.MaxDueDate != null)
            holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(req => req.TransactionRequest.DueDate <= query.MaxDueDate);

        var voucherValuesQuery = _dbContext.Set<VoucherValue>().AsQueryable();
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
            _dbContext.Set<UnitType>(),
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

        var holderTransactionRequestsWithDomainAccountsAndUnitAndUnitImageQuery = holderTransactionRequestsWithDomainAccountsAndUnitQuery.Join(_dbContext.Set<Identity>(),
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

        var resultQuery = holderTransactionRequestsWithDomainAccountsAndUnitAndUnitImageQuery.Select(req =>
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

        if (query.CounterpartyName is not null)
            resultQuery = resultQuery.Where(o => o.CounterpartyName.Contains(query.CounterpartyName) || o.CounterpartyEmail.Contains(query.CounterpartyName));

        return resultQuery.OrderByDescending(request => request.DueDate).GetListPageQuery(query);
    }
}