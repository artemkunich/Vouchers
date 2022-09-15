using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using Vouchers.Core;
using Vouchers.Values;

namespace Vouchers.EntityFramework.QueryHandlers
{
    public class IssuerVouchersQueryHandler : IAuthIdentityHandler<IssuerVouchersQuery,IEnumerable<VoucherDto>>
    {
        VouchersDbContext dbContext;

        public IssuerVouchersQueryHandler(VouchersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }     

        public async Task<IEnumerable<VoucherDto>> HandleAsync(IssuerVouchersQuery query, Guid authIdentityId, CancellationToken cancellation)
        {
            var issuerDomainAccount = await dbContext.UnitTypes.Where(v => v.Id == query.ValueId)
                .Join(dbContext.DomainAccounts, u => u.IssuerId, a => a.Id, (u, a) => a).FirstOrDefaultAsync();
            if (issuerDomainAccount is null)
                return new List<VoucherDto>();

            var authDomainAccounts = await dbContext.DomainAccounts.Where(a => a.IdentityId == authIdentityId && a.Domain.Id == issuerDomainAccount.Domain.Id).ToListAsync();
            if(!authDomainAccounts.Any())
                return new List<VoucherDto>();

            var authDomainAccount = authDomainAccounts.FirstOrDefault();
            var accountsQuery = dbContext.AccountItems.Where(account => account.Holder.Id == authDomainAccount.Id);

            return await dbContext.Units.Include(voucher => voucher.UnitType).Where(voucher => voucher.UnitType.Id == query.ValueId)
                .GroupJoin(
                    accountsQuery,
                    v => v.Id,
                    a => a.Unit.Id,
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
                ).ToListAsync(cancellation);
        }
    }
}
