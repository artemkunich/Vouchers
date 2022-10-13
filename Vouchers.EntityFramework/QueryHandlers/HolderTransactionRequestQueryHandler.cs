﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.UseCases;
using System.Threading;


namespace Vouchers.EntityFramework.QueryHandlers
{
    internal sealed class HolderTransactionRequestQueryHandler : IAuthIdentityHandler<Guid,HolderTransactionRequestDto>
    {
        readonly VouchersDbContext _dbContext;

        public HolderTransactionRequestQueryHandler(VouchersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public HolderTransactionRequestDto Handle(Guid transactionRequestId, Guid authIdentityId) =>
            GetQuery(transactionRequestId, authIdentityId).FirstOrDefault();

        public async Task<HolderTransactionRequestDto> HandleAsync(Guid transactionRequestId, Guid authIdentityId, CancellationToken cancellation) =>
            await GetQuery(transactionRequestId, authIdentityId).FirstOrDefaultAsync();

        public IQueryable<HolderTransactionRequestDto> GetQuery(Guid transactionRequestId, Guid authIdentityId) {

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
                    o => o.CreditorDomainAccounts.DefaultIfEmpty(),
                    (req, o) => new { req.TransactionRequest, req.DebtorDomainAccount, req.DebtorIdentity, CreditorDomainAccount = o.DomainAccount, CreditorIdentity = o.Identity }
                );


            holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(
                o => o.CreditorDomainAccount == null || o.CreditorDomainAccount.IdentityId == authIdentityId
            );

            holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(
               o => o.TransactionRequest.TransactionId == null
            );

            holderTransactionRequestsWithDomainAccountsQuery = holderTransactionRequestsWithDomainAccountsQuery.Where(
                o => o.TransactionRequest.Id == transactionRequestId
            );

            var holderTransactionRequestsWithDomainAccountsAndUnitQuery = holderTransactionRequestsWithDomainAccountsQuery.Join(
                _dbContext.VoucherValues,
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
            );

            var holderTransactionRequestsWithDomainAccountsAndUnitAndUnitIssuerQuery = holderTransactionRequestsWithDomainAccountsAndUnitQuery.Join(_dbContext.Identities,
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
                    UnitTypeIssuer = issuer
                }
            );

            return holderTransactionRequestsWithDomainAccountsAndUnitAndUnitIssuerQuery.Select(req =>
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
                    UnitIssuerEmail = req.UnitTypeIssuer.Email,
                    UnitIssuerName = req.UnitTypeIssuer.FirstName + " " + req.UnitTypeIssuer.LastName,
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
