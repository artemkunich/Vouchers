using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using Vouchers.EntityFramework.QueryHandlers;

namespace Vouchers.EntityFramework
{
    public static class IServiceCollectionExtension
    {        
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
            services.AddAuthIdentityHandler<Guid?, IdentityDetailDto, IdentityDetailQueryHandler>();
        }

        public static void AddDomainOfferHandlers(this IServiceCollection services)
        {
            services.AddHandler<DomainOffersQuery, IEnumerable<DomainOfferDto>, DomainOffersQueryHandler>();
            services.AddAuthIdentityHandler<IdentityDomainOffersQuery, IEnumerable<DomainOfferDto>, IdentityDomainOffersQueryHandler>();
        }

        public static void AddDomainHandlers(this IServiceCollection services)
        {
            services.AddAuthIdentityHandler<DomainsQuery, IEnumerable<DomainDto>, DomainsQueryHandler>();
            services.AddAuthIdentityHandler<Guid, DomainDetailDto, DomainDetailQueryHandler>();
        }

        public static void AddDomainAccountHandlers(this IServiceCollection services)
        {
            services.AddAuthIdentityHandler<DomainAccountsQuery, IEnumerable<DomainAccountDto>, DomainAccountsQueryHandler>();
            services.AddAuthIdentityHandler<IdentityDomainAccountsQuery, IEnumerable<DomainAccountDto>, IdentityDomainAccountsQueryHandler>();
        }

        public static void AddIssuerValueHandlers(this IServiceCollection services)
        {
            services.AddAuthIdentityHandler<IssuerValuesQuery, IEnumerable<VoucherValueDto>, IssuerValuesQueryHandler>();
            services.AddAuthIdentityHandler<Guid, VoucherValueDetailDto, VoucherValueDetailQueryHandler>();
        }

        public static void AddIssuerVoucherHandlers(this IServiceCollection services)
        {
            services.AddAuthIdentityHandler<IssuerVouchersQuery, IEnumerable<VoucherDto>, IssuerVouchersQueryHandler>();
        }

        public static void AddIssuerTransactionHandlers(this IServiceCollection services)
        {
            services.AddAuthIdentityHandler<IssuerTransactionsQuery, IEnumerable<IssuerTransactionDto>, IssuerTransactionsQueryHandler>();
        }

        public static void AddHolderValueHandlers(this IServiceCollection services)
        {
            services.AddAuthIdentityHandler<HolderValuesQuery, IEnumerable<VoucherValueDto>, HolderValuesQueryHandler>();
        }

        public static void AddHolderTransactionHandlers(this IServiceCollection services)
        {
            services.AddAuthIdentityHandler<HolderTransactionsQuery, IEnumerable<HolderTransactionDto>, HolderTransactionsQueryHandler>();
        }

        public static void AddHolderVoucherHandlers(this IServiceCollection services)
        {
            services.AddAuthIdentityHandler<HolderVouchersQuery, IEnumerable<VoucherDto>, HolderVouchersQueryHandler>();
        }

        public static void AddHolderTransactionRequestHandlers(this IServiceCollection services)
        {
            services.AddAuthIdentityHandler<HolderTransactionRequestsQuery, IEnumerable<HolderTransactionRequestDto>, HolderTransactionRequestsQueryHandler>();
            services.AddAuthIdentityHandler<DomainValuesQuery, IEnumerable<VoucherValueDto>, DomainValuesQueryHandler>();
            services.AddAuthIdentityHandler<Guid, HolderTransactionRequestDto, HolderTransactionRequestQueryHandler>();
        }

        public static void AddAuthIdentityHandler<TReq, TRes, THandler>(this IServiceCollection services) where THandler : class, IAuthIdentityHandler<TReq, TRes>
        {
            services.AddScoped<IAuthIdentityHandler<TReq, TRes>, THandler>();
            services.AddScoped<IHandler<TReq, TRes>, AuthIdentityHandler<TReq, TRes>>();
        }

        public static void AddAuthIdentityHandler<TReq, THandler>(this IServiceCollection services) where THandler : class, IAuthIdentityHandler<TReq>
        {
            services.AddScoped<IAuthIdentityHandler<TReq>, THandler>();
            services.AddScoped<IHandler<TReq>, AuthIdentityHandler<TReq>>();
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
