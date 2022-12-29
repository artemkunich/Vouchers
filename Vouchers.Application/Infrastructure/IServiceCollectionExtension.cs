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
using Vouchers.Application.Events.IdentityEvents;
using Vouchers.Application.Queries;
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
        public static void AddCommandHandlers(this IServiceCollection services)
        {         
            services.AddScoped<IAuthIdentityProvider, AuthIdentityProvider>();
            services.AddScoped<IAppImageService, AppImageService>();
            services.AddScoped<IMessageFactory, MessageFactory>();
            services.AddIdentityHandlers();
            services.AddDomainOfferHandlers();
            services.AddDomainHandlers();
            services.AddDomainAccountHandlers();
            services.AddIssuerValueHandlers();
            services.AddIssuerVoucherHandlers();
            services.AddIssuerTransactionHandlers();
            services.AddHolderTransactionHandlers();
        }


        private static void AddIdentityHandlers(this IServiceCollection services)
        {
            services.AddHandler<CreateIdentityCommand, Guid, CreateIdentityCommandHandler>();
            services.AddHandler<UpdateIdentityCommand, UpdateIdentityCommandHandler>();
            services.AddHandler<IdentityUpdatedEvent, IdentityUpdatedEventHandler>();
        }

        private static void AddDomainOfferHandlers(this IServiceCollection services)
        {
            services.AddHandler<CreateDomainOfferCommand, Guid, CreateDomainOfferCommandHandler>();
            services.AddHandler<UpdateDomainOfferCommand, UpdateDomainOfferCommandHandler>();
        }

        private static void AddDomainHandlers(this IServiceCollection services)
        {
            services.AddHandler<CreateDomainCommand, Guid?, CreateDomainCommandHandler>();
            services.AddHandler<UpdateDomainDetailCommand, UpdateDomainDetailCommandHandler>();
        }

        private static void AddDomainAccountHandlers(this IServiceCollection services)
        {
            services.AddHandler<CreateDomainAccountCommand, Guid, CreateDomainAccountCommandHandler>();
            services.AddHandler<UpdateDomainAccountCommand, UpdateDomainAccountCommandHandler>();
        }

        private static void AddIssuerValueHandlers(this IServiceCollection services)
        {
            services.AddHandler<CreateVoucherValueCommand, Guid, CreateVoucherValueCommandHandler>();
            services.AddHandler<UpdateVoucherValueCommand, UpdateVoucherValueCommandHandler>();
        }

        private static void AddIssuerVoucherHandlers(this IServiceCollection services)
        {
            services.AddHandler<CreateVoucherCommand, Guid, CreateVoucherCommandHandler>();
            services.AddHandler<UpdateVoucherCommand, UpdateVoucherCommandHandler>();
        }

        private static void AddIssuerTransactionHandlers(this IServiceCollection services)
        {
            services.AddHandler<CreateIssuerTransactionCommand, Guid, CreateIssuerTransactionCommandHandler>();
        }

        private static void AddHolderTransactionHandlers(this IServiceCollection services)
        {
            services.AddHandler<CreateHolderTransactionCommand, Guid, CreateHolderTransactionCommandHandler>();
            services.AddHandler<CreateHolderTransactionRequestCommand, Guid, CreateHolderTransactionRequestCommandHandler>();
            services.AddHandler<DeleteHolderTransactionRequestCommand, DeleteHolderTransactionRequestCommandHandler>();
        }


        private static void AddHandler<TReq, TRes, THandler>(this IServiceCollection services) where THandler : class, IHandler<TReq, TRes>
        {
            services.AddScoped<IHandler<TReq, TRes>, THandler>();
        }

        private static void AddHandler<TReq, THandler>(this IServiceCollection services) where THandler : class, IHandler<TReq>
        {
            services.AddScoped<IHandler<TReq>, THandler>();
        }
    }
}
