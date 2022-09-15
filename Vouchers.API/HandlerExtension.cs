using Microsoft.Extensions.DependencyInjection;
using Vouchers.Application.UseCases;

namespace Vouchers.API
{
    public static class HandlerExtension
    {
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
