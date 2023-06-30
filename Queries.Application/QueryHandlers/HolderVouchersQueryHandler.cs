using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

internal sealed class HolderVouchersQueryHandler : IRequestHandler<HolderVouchersQuery,IReadOnlyList<VoucherDto>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly VouchersDbContext _dbContext;

    public HolderVouchersQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext)
    {
        _authIdentityProvider = authIdentityProvider;
        _dbContext = dbContext;
    }

    public async Task<Result<IReadOnlyList<VoucherDto>>> HandleAsync(HolderVouchersQuery query, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var issuerDomainAccount = await _dbContext.Set<DomainAccount>().Where(x => x.IdentityId == authIdentityId).FirstOrDefaultAsync();
        if (issuerDomainAccount is null)
            return new List<VoucherDto>();

        var authDomainAccounts = await _dbContext.Set<DomainAccount>().Where(a => a.IdentityId == authIdentityId && a.DomainId == issuerDomainAccount.DomainId).ToListAsync();
        if (!authDomainAccounts.Any())
            return new List<VoucherDto>();

        var authDomainAccount = authDomainAccounts.FirstOrDefault();

        var accountsQuery = _dbContext.Set<AccountItem>()
            .Include(account => account.HolderAccount)
            .Include(account => account.Unit).ThenInclude(unit => unit.UnitType)
            .Where(account => account.HolderAccount.Id == authDomainAccount.Id);

        var vouchersQuery = _dbContext.Set<Unit>().Where(
            voucher => _dbContext.Set<AccountItem>()
                .Include(acc => acc.HolderAccount)
                .Include(acc => acc.Unit)
                .Where(acc => acc.HolderAccount.Id == authDomainAccount.Id && acc.Balance > 0).Select(acc => acc.Unit.Id)
                .Contains(voucher.Id) && voucher.ValidTo >= DateTime.Today && voucher.UnitType.Id == query.ValueId
        );

        return await accountsQuery.Join(
            vouchersQuery,
            account => account.Unit.Id,
            voucher => voucher.Id,
            (account, voucher) => new VoucherDto
            {
                Id = voucher.Id,
                ValidFrom = voucher.ValidFrom,
                ValidTo = voucher.ValidTo,
                CanBeExchanged = voucher.CanBeExchanged,
                Supply = voucher.Supply,
                Balance = account.Balance
            }
        ).GetListPageQuery(query).ToListAsync(cancellation);
    }
}