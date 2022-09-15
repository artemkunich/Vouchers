using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.UseCases;

namespace Vouchers.API.Services
{
    public class Dispatcher : IDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;

        public async Task DispatchAsync<TMessage>(TMessage message, CancellationToken cancellation = default(CancellationToken))
        {
            var handlers = serviceProvider.GetServices<IHandler<TMessage>>();
            await Task.WhenAll(handlers.Select(handler => handler.HandleAsync(message, cancellation)));
        }

        public async Task<TResult> DispatchAsync<TRequest, TResult>(TRequest request, CancellationToken cancellation = default(CancellationToken))
        {
            var handler = serviceProvider.GetRequiredService<IHandler<TRequest, TResult>>();
            return await handler.HandleAsync(request, cancellation);
        }
    }
}
