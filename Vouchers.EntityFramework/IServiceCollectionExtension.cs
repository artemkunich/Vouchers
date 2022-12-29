using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Queries;
using Vouchers.Application.UseCases;
using Vouchers.Core;
using Vouchers.Domains;
using Vouchers.Entities;
using Vouchers.EntityFramework.QueryHandlers;
using Vouchers.EntityFramework.Repositories;
using Vouchers.Files;
using Vouchers.Identities;
using Vouchers.InterCommunication;
using Vouchers.Values;

namespace Vouchers.EntityFramework
{
    public static class IServiceCollectionExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<AccountItem,Guid>, AccountItemRepository>();
            services.AddScoped<IRepository<HolderTransactionRequest,Guid>, HolderTransactionRequestRepository>();
            services.AddScoped<IRepository<DomainAccount,Guid>, DomainAccountRepository>();
            services.AddScoped<IRepository<DomainContract,Guid>, DomainContractRepository>();
            services.AddScoped<IRepository<DomainOffersPerIdentityCounter,Guid>, DomainOffersPerIdentityCounterRepository>();
            services.AddScoped<IRepository<Domain,Guid>, DomainRepository>();
            services.AddScoped<IRepository<Login,Guid>, LoginRepository>();
            services.AddScoped<IRepository<Unit,Guid>, UnitRepository>();
            services.AddScoped<IRepository<UnitType,Guid>, UnitTypeRepository>();

            services.AddScoped<IRepository<Account,Guid>, GenericRepository<Account,Guid>>();
            services.AddScoped<IRepository<HolderTransaction,Guid>, GenericRepository<HolderTransaction,Guid>>();
            services.AddScoped<IRepository<HolderTransactionItem,Guid>, GenericRepository<HolderTransactionItem,Guid>>();
            services.AddScoped<IRepository<IssuerTransaction,Guid>, GenericRepository<IssuerTransaction,Guid>>();
            services.AddScoped<IRepository<DomainOffer,Guid>, GenericRepository<DomainOffer,Guid>>();
            services.AddScoped<IRepository<VoucherValue,Guid>, GenericRepository<VoucherValue,Guid>>();
            services.AddScoped<IRepository<Identity,Guid>, GenericRepository<Identity,Guid>>();
            services.AddScoped<IRepository<CroppedImage,Guid>, GenericRepository<CroppedImage,Guid>>();
            
            services.AddScoped<IRepository<OutboxMessage,Guid>, GenericRepository<OutboxMessage,Guid>>();
            services.AddScoped<IRepository<InboxMessage,Guid>, GenericRepository<InboxMessage,Guid>>();
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

        private static void AddIdentityHandlers(this IServiceCollection services)
        {
            services.AddHandler<string, Guid?, IdentityQueryHandler>();
            services.AddHandler<Guid?, IdentityDetailDto, IdentityDetailQueryHandler>();
        }

        private static void AddDomainOfferHandlers(this IServiceCollection services)
        {
            services.AddHandler<DomainOffersQuery, IEnumerable<DomainOfferDto>, DomainOffersQueryHandler>();
            services.AddHandler<IdentityDomainOffersQuery, IEnumerable<DomainOfferDto>, IdentityDomainOffersQueryHandler>();
        }

        private static void AddDomainHandlers(this IServiceCollection services)
        {
            services.AddHandler<DomainsQuery, IEnumerable<DomainDto>, DomainsQueryHandler>();
            services.AddHandler<Guid, DomainDetailDto, DomainDetailQueryHandler>();
        }

        private static void AddDomainAccountHandlers(this IServiceCollection services)
        {
            services.AddHandler<DomainAccountsQuery, IEnumerable<DomainAccountDto>, DomainAccountsQueryHandler>();
            services.AddHandler<IdentityDomainAccountsQuery, IEnumerable<DomainAccountDto>, IdentityDomainAccountsQueryHandler>();
        }

        private static void AddIssuerValueHandlers(this IServiceCollection services)
        {
            services.AddHandler<IssuerValuesQuery, IEnumerable<VoucherValueDto>, IssuerValuesQueryHandler>();
            services.AddHandler<Guid, VoucherValueDetailDto, VoucherValueDetailQueryHandler>();
        }

        private static void AddIssuerVoucherHandlers(this IServiceCollection services)
        {
            services.AddHandler<IssuerVouchersQuery, IEnumerable<VoucherDto>, IssuerVouchersQueryHandler>();
        }

        private static void AddIssuerTransactionHandlers(this IServiceCollection services)
        {
            services.AddHandler<IssuerTransactionsQuery, IEnumerable<IssuerTransactionDto>, IssuerTransactionsQueryHandler>();
        }

        private static void AddHolderValueHandlers(this IServiceCollection services)
        {
            services.AddHandler<HolderValuesQuery, IEnumerable<VoucherValueDto>, HolderValuesQueryHandler>();
        }

        private static void AddHolderTransactionHandlers(this IServiceCollection services)
        {
            services.AddHandler<HolderTransactionsQuery, IEnumerable<HolderTransactionDto>, HolderTransactionsQueryHandler>();
        }

        private static void AddHolderVoucherHandlers(this IServiceCollection services)
        {
            services.AddHandler<HolderVouchersQuery, IEnumerable<VoucherDto>, HolderVouchersQueryHandler>();
        }

        private static void AddHolderTransactionRequestHandlers(this IServiceCollection services)
        {
            services.AddHandler<HolderTransactionRequestsQuery, IEnumerable<HolderTransactionRequestDto>, HolderTransactionRequestsQueryHandler>();
            services.AddHandler<DomainValuesQuery, IEnumerable<VoucherValueDto>, DomainValuesQueryHandler>();
            services.AddHandler<Guid, HolderTransactionRequestDto, HolderTransactionRequestQueryHandler>();
        }

        private static void AddHandler<TReq, TRes, THandler>(this IServiceCollection services) where THandler : class, IHandler<TReq, TRes>
        {
            services.AddScoped<IHandler<TReq, TRes>, THandler>();
        }
    }
}
