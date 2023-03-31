using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Dtos;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Common.Application.Queries;
using Vouchers.Common.Application.Services;
using Vouchers.Common.Application.UseCases;
using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;
using Unit = Vouchers.Core.Domain.Unit;

namespace Vouchers.Persistence.QueryHandlers;

internal sealed class IssuerVouchersQueryHandler : IRequestHandler<IssuerVouchersQuery,IReadOnlyList<VoucherDto>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly VouchersDbContext _dbContext;

    public IssuerVouchersQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext)
    {
        _authIdentityProvider = authIdentityProvider;
        _dbContext = dbContext;
    }     

    public async Task<Result<IReadOnlyList<VoucherDto>>> HandleAsync(IssuerVouchersQuery query, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var issuerDomainAccount = await _dbContext.Set<UnitType>().Where(v => v.Id == query.ValueId)
            .Join(_dbContext.Set<DomainAccount>(), u => u.IssuerAccountId, a => a.Id, (u, a) => a).FirstOrDefaultAsync();
        if (issuerDomainAccount is null)
            return new List<VoucherDto>();

        var authDomainAccounts = await _dbContext.Set<DomainAccount>().Where(a => a.IdentityId == authIdentityId && a.Domain.Id == issuerDomainAccount.DomainId).ToListAsync();
        if(!authDomainAccounts.Any())
            return new List<VoucherDto>();

        var authDomainAccount = authDomainAccounts.FirstOrDefault();
        var accountsQuery = _dbContext.Set<AccountItem>().Where(account => account.HolderAccountId == authDomainAccount.Id);

        return await _dbContext.Set<Unit>().Where(voucher => voucher.UnitTypeId == query.ValueId)
            .GroupJoin(
                accountsQuery,
                v => v.Id,
                a => a.UnitId,
                (voucher, accounts) => new { Voucher = voucher, Accounts = accounts }
            ).SelectMany(
                result => result.Accounts.DefaultIfEmpty(),
                (result, account) => new VoucherDto
                {
                    Id = result.Voucher.Id,
                    ValidFrom = result.Voucher.ValidFrom,
                    ValidTo = result.Voucher.ValidTo,
                    CanBeExchanged = result.Voucher.CanBeExchanged,
                    Supply = result.Voucher.Supply,
                    Balance = account == null ? 0.0m : account.Balance
                }
            ).GetListPageQuery(query).ToListAsync(cancellation);
    }
}