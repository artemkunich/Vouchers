using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Configuration;
using Vouchers.Application.Commands.DomainAccountCommands;
using Vouchers.Application.Commands.DomainCommands;
using Vouchers.Application.Commands.DomainOfferCommands;
using Vouchers.Application.Commands.HolderTransactionCommands;
using Vouchers.Application.Commands.HolderTransactionRequestCommands;
using Vouchers.Application.Commands.IdentityCommands;
using Vouchers.Application.Commands.IssuerTransactionCommands;
using Vouchers.Application.Commands.VoucherCommands;
using Vouchers.Application.Commands.VoucherValueCommands;
using Vouchers.Application.Events.IdentityEvents;
using Vouchers.Application.ServiceProviders;
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
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configurations) =>
            services
                .AddScoped<IAuthIdentityProvider, AuthIdentityProvider>()
                .AddScoped<IAppImageService, AppImageService>()
                .AddScoped<IMessageFactory, MessageFactory>();


        private static IServiceCollection AddIdentityHandlers(this IServiceCollection services) =>
            services
                .AddHandler<CreateIdentityCommand, Guid, CreateIdentityCommandHandler>()
                .AddHandler<UpdateIdentityCommand, UpdateIdentityCommandHandler>()
                .AddHandler<IdentityUpdatedEvent, IdentityUpdatedEventHandler>();

        private static IServiceCollection AddDomainOfferHandlers(this IServiceCollection services) =>
            services
                .AddHandler<CreateDomainOfferCommand, Guid, CreateDomainOfferCommandHandler>()
                .AddHandler<UpdateDomainOfferCommand, UpdateDomainOfferCommandHandler>();

        private static IServiceCollection AddDomainHandlers(this IServiceCollection services) =>
            services
                .AddHandler<CreateDomainCommand, Guid?, CreateDomainCommandHandler>()
                .AddHandler<UpdateDomainDetailCommand, UpdateDomainDetailCommandHandler>();

        private static IServiceCollection AddDomainAccountHandlers(this IServiceCollection services) =>
            services
                .AddHandler<CreateDomainAccountCommand, Guid, CreateDomainAccountCommandHandler>()
                .AddHandler<UpdateDomainAccountCommand, UpdateDomainAccountCommandHandler>();

        private static IServiceCollection AddIssuerValueHandlers(this IServiceCollection services) =>
            services
                .AddHandler<CreateVoucherValueCommand, Guid, CreateVoucherValueCommandHandler>()
                .AddHandler<UpdateVoucherValueCommand, UpdateVoucherValueCommandHandler>();

        private static IServiceCollection AddIssuerVoucherHandlers(this IServiceCollection services) =>
            services
                .AddHandler<CreateVoucherCommand, Guid, CreateVoucherCommandHandler>()
                .AddHandler<UpdateVoucherCommand, UpdateVoucherCommandHandler>();
        

        private static IServiceCollection AddIssuerTransactionHandlers(this IServiceCollection services) =>
            services.AddHandler<CreateIssuerTransactionCommand, Guid, CreateIssuerTransactionCommandHandler>();

        private static IServiceCollection AddHolderTransactionHandlers(this IServiceCollection services) =>
            services
                .AddHandler<CreateHolderTransactionCommand, Guid, CreateHolderTransactionCommandHandler>()
                .AddHandler<CreateHolderTransactionRequestCommand, Guid, CreateHolderTransactionRequestCommandHandler>()
                .AddHandler<DeleteHolderTransactionRequestCommand, DeleteHolderTransactionRequestCommandHandler>();


        private static IServiceCollection AddHandler<TReq, TRes, THandler>(this IServiceCollection services) where THandler : class, IHandler<TReq, TRes> =>
            services.AddScoped<IHandler<TReq, TRes>, THandler>();

        private static IServiceCollection AddHandler<TReq, THandler>(this IServiceCollection services) where THandler : class, IHandler<TReq> =>
            services.AddScoped<IHandler<TReq>, THandler>();
    }
}
