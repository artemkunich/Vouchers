using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using Vouchers.Core;
using Vouchers.Domains;
using Vouchers.EntityFramework.QueryHandlers;
using Vouchers.EntityFramework.Repositories;
using Vouchers.Files;
using Vouchers.Identities;
using Vouchers.Values;

namespace Vouchers.EntityFramework
{
    public static class IServiceCollectionExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<AccountItem>, AccountItemRepository>();
            services.AddScoped<IRepository<HolderTransactionRequest>, HolderTransactionRequestRepository>();
            services.AddScoped<IRepository<DomainAccount>, DomainAccountRepository>();
            services.AddScoped<IRepository<DomainContract>, DomainContractRepository>();
            services.AddScoped<IRepository<DomainOffersPerIdentityCounter>, DomainOffersPerIdentityCounterRepository>();
            services.AddScoped<IRepository<Domain>, DomainRepository>();
            services.AddScoped<IRepository<Login>, LoginRepository>();
            services.AddScoped<IRepository<Unit>, UnitRepository>();
            services.AddScoped<IRepository<UnitType>, UnitTypeRepository>();

            services.AddScoped<IRepository<Account>, GenericRepository<Account>>();
            services.AddScoped<IRepository<HolderTransaction>, GenericRepository<HolderTransaction>>();
            services.AddScoped<IRepository<HolderTransactionItem>, GenericRepository<HolderTransactionItem>>();
            services.AddScoped<IRepository<IssuerTransaction>, GenericRepository<IssuerTransaction>>();
            services.AddScoped<IRepository<DomainOffer>, GenericRepository<DomainOffer>>();
            services.AddScoped<IRepository<VoucherValue>, GenericRepository<VoucherValue>>();
            services.AddScoped<IRepository<Identity>, GenericRepository<Identity>>();
            services.AddScoped<IRepository<CroppedImage>, GenericRepository<CroppedImage>>();
        }

        public static void AddQueryHandlers(this IServiceCollection services)
        {
            services.AddIdentityHandlers();
            services.AddDomainOfferHandlers();
            services.AddDomainHandlers();
            services.AddDomainAccountHandlers();
            services.AddIssuerValueHandlers();
            services.AddIssuerVoucherHandlers();
            services.AddIssuerTransactionHandlers();
            services.AddHolderValueHandlers();
            services.AddHolderTransactionHandlers();
            services.AddHolderVoucherHandlers();
            services.AddHolderTransactionRequestHandlers();
        }

        public static void AddIdentityHandlers(this IServiceCollection services)
        {
            services.AddHandler<string, Guid?, IdentityQueryHandler>();
            services.AddHandler<Guid?, IdentityDetailDto, IdentityDetailQueryHandler>();
        }

        public static void AddDomainOfferHandlers(this IServiceCollection services)
        {
            services.AddHandler<DomainOffersQuery, IEnumerable<DomainOfferDto>, DomainOffersQueryHandler>();
            services.AddHandler<IdentityDomainOffersQuery, IEnumerable<DomainOfferDto>, IdentityDomainOffersQueryHandler>();
        }

        public static void AddDomainHandlers(this IServiceCollection services)
        {
            services.AddHandler<DomainsQuery, IEnumerable<DomainDto>, DomainsQueryHandler>();
            services.AddHandler<Guid, DomainDetailDto, DomainDetailQueryHandler>();
        }

        public static void AddDomainAccountHandlers(this IServiceCollection services)
        {
            services.AddHandler<DomainAccountsQuery, IEnumerable<DomainAccountDto>, DomainAccountsQueryHandler>();
            services.AddHandler<IdentityDomainAccountsQuery, IEnumerable<DomainAccountDto>, IdentityDomainAccountsQueryHandler>();
        }

        public static void AddIssuerValueHandlers(this IServiceCollection services)
        {
            services.AddHandler<IssuerValuesQuery, IEnumerable<VoucherValueDto>, IssuerValuesQueryHandler>();
            services.AddHandler<Guid, VoucherValueDetailDto, VoucherValueDetailQueryHandler>();
        }

        public static void AddIssuerVoucherHandlers(this IServiceCollection services)
        {
            services.AddHandler<IssuerVouchersQuery, IEnumerable<VoucherDto>, IssuerVouchersQueryHandler>();
        }

        public static void AddIssuerTransactionHandlers(this IServiceCollection services)
        {
            services.AddHandler<IssuerTransactionsQuery, IEnumerable<IssuerTransactionDto>, IssuerTransactionsQueryHandler>();
        }

        public static void AddHolderValueHandlers(this IServiceCollection services)
        {
            services.AddHandler<HolderValuesQuery, IEnumerable<VoucherValueDto>, HolderValuesQueryHandler>();
        }

        public static void AddHolderTransactionHandlers(this IServiceCollection services)
        {
            services.AddHandler<HolderTransactionsQuery, IEnumerable<HolderTransactionDto>, HolderTransactionsQueryHandler>();
        }

        public static void AddHolderVoucherHandlers(this IServiceCollection services)
        {
            services.AddHandler<HolderVouchersQuery, IEnumerable<VoucherDto>, HolderVouchersQueryHandler>();
        }

        public static void AddHolderTransactionRequestHandlers(this IServiceCollection services)
        {
            services.AddHandler<HolderTransactionRequestsQuery, IEnumerable<HolderTransactionRequestDto>, HolderTransactionRequestsQueryHandler>();
            services.AddHandler<DomainValuesQuery, IEnumerable<VoucherValueDto>, DomainValuesQueryHandler>();
            services.AddHandler<Guid, HolderTransactionRequestDto, HolderTransactionRequestQueryHandler>();
        }

        public static void AddHandler<TReq, TRes, THandler>(this IServiceCollection services) where THandler : class, IHandler<TReq, TRes>
        {
            services.AddScoped<IHandler<TReq, TRes>, THandler>();
        }

        public static void AddHandler<TReq, THandler>(this IServiceCollection services) where THandler : class, IHandler<TReq>
        {
            services.AddScoped<IHandler<TReq>, THandler>();
        }
    }
}
