using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.UseCases;

namespace Vouchers.Infrastructure;

public sealed class Dispatcher : IDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public Dispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task DispatchAsync<TMessage>(TMessage message, CancellationToken cancellation = default(CancellationToken))
    {
        var handlers = _serviceProvider.GetServices<IHandler<TMessage>>();
        await Task.WhenAll(handlers.Select(handler => handler.HandleAsync(message, cancellation)));
    }

    public async Task<Result<TResult>> DispatchAsync<TRequest, TResult>(TRequest request, CancellationToken cancellation = default(CancellationToken))
    {
        var handler = _serviceProvider.GetRequiredService<IHandler<TRequest, TResult>>();
        return await handler.HandleAsync(request, cancellation);
    }
}