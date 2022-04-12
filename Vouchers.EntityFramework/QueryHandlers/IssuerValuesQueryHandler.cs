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

namespace Vouchers.EntityFramework.Repositories
{
    public class IssuerValuesQueryHandler : IHandler<IssuerValuesQuery,IEnumerable<VoucherValueDto>>
    {
        VouchersDbContext dbContext;

        public IssuerValuesQueryHandler(VouchersDbContext dbContext)
        {
            this.dbContext = dbContext;
        }     

        public async Task<IEnumerable<VoucherValueDto>> HandleAsync(IssuerValuesQuery query, CancellationToken cancellation)
        {
            var valueDetails = await dbContext.VoucherValueDetails
                .Include(detail => detail.Value.Issuer)
                .Where(detail => detail.Value.Issuer.Id == query.IssuerId)
                .ToListAsync();

            var accounts = await dbContext.VoucherAccounts
                .Include(account => account.Holder)
                .Include(account => account.Unit).ThenInclude(unit => unit.Value)
                .Where(account => account.Holder.Id == query.IssuerId)
                .ToListAsync();

            return ConvertToIssuerVoucherValues(valueDetails, accounts);
        }

        public IEnumerable<VoucherValueDto> Handle(IssuerValuesQuery query)
        {
            var valueDetails = dbContext.VoucherValueDetails
                .Include(detail => detail.Value.Issuer)
                .Where(detail => detail.Value.Issuer.Id == query.IssuerId)
                .ToList();

            var accounts = dbContext.VoucherAccounts
                .Include(account => account.Holder)
                .Include(account => account.Unit).ThenInclude(unit => unit.Value)
                .Where(account => account.Holder.Id == query.IssuerId)
                .ToList();
           
            return ConvertToIssuerVoucherValues(valueDetails, accounts);
        }

        private IEnumerable<VoucherValueDto> ConvertToIssuerVoucherValues(IEnumerable<VoucherValueDetail> valueDetails, IEnumerable<VoucherAccount> accounts) =>

            valueDetails.Select(valueDetail =>
                new VoucherValueDto
                {
                    Id = valueDetail.Value.Id,
                    Ticker = valueDetail.Ticker,
                    Description = valueDetail.Description,
                    IssuerId = valueDetail.Value.Issuer.Id,
                    Supply = valueDetail.Value.Supply,
                    Vouchers = dbContext.Vouchers.Where(voucher => voucher.Value.Id == valueDetail.Value.Id).ToList()
                    .Select(
                        voucher => new VoucherDto
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
