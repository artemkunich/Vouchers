using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Vouchers.Application.Commands;
using Vouchers.Application.Commands.DomainAccountCommands;
using Vouchers.Application.Commands.DomainCommands;
using Vouchers.Application.Commands.DomainOfferCommands;
using Vouchers.Application.Commands.HolderTransactionCommands;
using Vouchers.Application.Commands.HolderTransactionRequestCommands;
using Vouchers.Application.Commands.IdentityCommands;
using Vouchers.Application.Commands.IssuerTransactionCommands;
using Vouchers.Application.Commands.VoucherCommands;
using Vouchers.Application.Commands.VoucherValueCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.Application.Services;
using Vouchers.Application.UseCases;
using Vouchers.Application.UseCases.DomainAccountCases;
using Vouchers.Application.UseCases.DomainCases;
using Vouchers.Application.UseCases.DomainOfferCases;
using Vouchers.Application.UseCases.HolderTransactionCases;
using Vouchers.Application.UseCases.HolderTransactionRequestCases;
using Vouchers.Application.UseCases.IdentityCases;
using Vouchers.Application.UseCases.IssuerTransactionCases;
using Vouchers.Application.UseCases.VoucherCases;
using Vouchers.Application.UseCases.VoucherValueCases;

namespace Application.Infrastructure
{
    public static class IServiceCollectionExtension
    {        
        public static void AddCommandHandlers(this IServiceCollection services)
        {
            services.AddScoped<AppImageService>();
            services.AddIdentityHandlers();
            services.AddDomainOfferHandlers();
            services.AddDomainHandlers();
            services.AddDomainAccountHandlers();
            services.AddIssuerValueHandlers();
            services.AddIssuerVoucherHandlers();
            services.AddIssuerTransactionHandlers();
            services.AddHolderTransactionHandlers();
        }


        public static void AddIdentityHandlers(this IServiceCollection services)
        {
            services.AddHandler<CreateIdentityCommand, CreateIdentityCommandHandler>();
            services.AddAuthIdentityHandler<UpdateIdentityCommand, UpdateIdentityCommandHandler>();
        }

        public static void AddDomainOfferHandlers(this IServiceCollection services)
        {
            services.AddAuthIdentityHandler<CreateDomainOfferCommand, Guid, CreateDomainOfferCommandHandler>();
            services.AddAuthIdentityHandler<UpdateDomainOfferCommand, UpdateDomainOfferCommandHandler>();
        }

        public static void AddDomainHandlers(this IServiceCollection services)
        {
            services.AddAuthIdentityHandler<CreateDomainCommand, Guid?, CreateDomainCommandHandler>();
            services.AddAuthIdentityHandler<UpdateDomainDetailCommand, UpdateDomainDetailCommandHandler>();
        }

        public static void AddDomainAccountHandlers(this IServiceCollection services)
        {
            services.AddAuthIdentityHandler<CreateDomainAccountCommand, Guid, CreateDomainAccountCommandHandler>();
            services.AddAuthIdentityHandler<UpdateDomainAccountCommand, UpdateDomainAccountCommandHandler>();
        }

        public static void AddIssuerValueHandlers(this IServiceCollection services)
        {
            services.AddAuthIdentityHandler<CreateVoucherValueCommand, Guid, CreateVoucherValueCommandHandler>();
            services.AddAuthIdentityHandler<UpdateVoucherValueCommand, UpdateVoucherValueCommandHandler>();
        }

        public static void AddIssuerVoucherHandlers(this IServiceCollection services)
        {
            services.AddAuthIdentityHandler<CreateVoucherCommand, Guid, CreateVoucherCommandHandler>();
            services.AddAuthIdentityHandler<UpdateVoucherCommand, UpdateVoucherCommandHandler>();
        }

        public static void AddIssuerTransactionHandlers(this IServiceCollection services)
        {
            services.AddAuthIdentityHandler<CreateIssuerTransactionCommand, Guid, CreateIssuerTransactionCommandHandler>();
        }

        public static void AddHolderTransactionHandlers(this IServiceCollection services)
        {
            services.AddAuthIdentityHandler<CreateHolderTransactionCommand, Guid, CreateHolderTransactionCommandHandler>();
            services.AddAuthIdentityHandler<CreateHolderTransactionRequestCommand, Guid, CreateHolderTransactionRequestCommandHandler>();
            services.AddAuthIdentityHandler<DeleteHolderTransactionRequestCommand, DeleteHolderTransactionRequestCommandHandler>();
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
