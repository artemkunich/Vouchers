using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class HolderValuesQueryHandler : IHandler<HolderValuesQuery,IEnumerable<VoucherValueDto>>
    {
        VouchersDbContext dbContext;

        public HolderValuesQueryHandler(VouchersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<VoucherValueDto>> HandleAsync(HolderValuesQuery query, CancellationToken cancellation)
        { 

            var values = await dbContext.VoucherValueDetails
                .Include(detail => detail.Value.Issuer)
                .Where(detail =>
                   dbContext.Vouchers.Where(
                        voucher => dbContext.VoucherAccounts
                        .Include(acc => acc.Holder)
                        .Include(acc => acc.Unit)
                        .Where(acc => acc.Holder.Id == query.HolderId && acc.Balance > 0).Select(acc => acc.Unit.Id)
                        .Contains(voucher.Id) && voucher.ValidTo >= DateTime.Today && voucher.Value.Id == detail.Value.Id
                    ).ToList().Any()
                ).ToListAsync();

            var accounts = await dbContext.VoucherAccounts
                .Include(account => account.Holder)
                .Include(account => account.Unit).ThenInclude(unit => unit.Value)
                .Where(account => account.Holder.Id == query.HolderId)
                .ToListAsync();

            return ConvertToHolderVoucherValues(query.HolderId, values, accounts);
        }

        public IEnumerable<VoucherValueDto> Handle(HolderValuesQuery query)
        {
            var values = dbContext.VoucherValueDetails
                .Include(detail => detail.Value.Issuer)
                .Where(detail =>
                   dbContext.Vouchers.Where(
                        voucher => dbContext.VoucherAccounts
                        .Include(acc => acc.Holder)
                        .Include(acc => acc.Unit)
                        .Where(acc => acc.Holder.Id == query.HolderId && acc.Balance > 0).Select(acc => acc.Unit.Id)
                        .Contains(voucher.Id) && voucher.ValidTo >= DateTime.Today && voucher.Value.Id == detail.Value.Id
                    ).ToList().Any()
                ).ToList();

            var accounts = dbContext.VoucherAccounts
                .Include(account => account.Holder)
                .Include(account => account.Unit).ThenInclude(unit => unit.Value)
                .Where(account => account.Holder.Id == query.HolderId)
                .ToList();

            return ConvertToHolderVoucherValues(query.HolderId, values, accounts);
        }

        private IEnumerable<VoucherValueDto> ConvertToHolderVoucherValues(Guid holderId, IEnumerable<VoucherValueDetail> valueDetails, IEnumerable<VoucherAccount> accounts) =>
            valueDetails.Select(valueDetail =>
                new VoucherValueDto
                {
                    Id = valueDetail.Value.Id,
                    Ticker = valueDetail.Ticker,
                    Description = valueDetail.Description,
                    IssuerId = valueDetail.Value.Issuer.Id,
                    Supply = valueDetail.Value.Supply,
                    Vouchers = dbContext.Vouchers.Where(
                        voucher => dbContext.VoucherAccounts
                        .Include(acc => acc.Holder)
                        .Include(acc => acc.Unit)
                        .Where(acc => acc.Holder.Id == holderId && acc.Balance > 0).Select(acc => acc.Unit.Id)
                        .Contains(voucher.Id) && voucher.ValidTo >= DateTime.Today && voucher.Value.Id == valueDetail.Value.Id
                    ).ToList()
                    .Select(voucher =>
                        new VoucherDto
                        {
                            Id = voucher.Id,
                            ValidFrom = voucher.ValidFrom,
                            ValidTo = voucher.ValidTo,
                            CanBeExchanged = voucher.CanBeExchanged,
                            Supply = voucher.Supply,
                            Balance = accounts.FirstOrDefault(acc => acc.Unit.Id == voucher.Id)?.Balance ?? 0.0m
                        }
                    ).ToList()
                }
            );
    }
}
