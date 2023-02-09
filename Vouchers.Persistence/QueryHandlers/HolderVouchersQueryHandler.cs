using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Application.Services;
using Vouchers.Application.UseCases;
using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;

namespace Vouchers.Persistence.QueryHandlers;

internal sealed class HolderVouchersQueryHandler : IHandler<HolderVouchersQuery,IEnumerable<VoucherDto>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly VouchersDbContext _dbContext;
    private readonly ICultureInfoProvider _cultureInfoProvider;

    public HolderVouchersQueryHandler(IAuthIdentityProvider authIdentityProvider, VouchersDbContext dbContext, ICultureInfoProvider cultureInfoProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _dbContext = dbContext;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task<Result<IEnumerable<VoucherDto>>> HandleAsync(HolderVouchersQuery query, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        if (authIdentityId is null)
            return Error.NotAuthorized(_cultureInfoProvider.GetCultureInfo());

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