﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.UseCases;

namespace Vouchers.Infrastructure;

public sealed class Dispatcher : IDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public Dispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public async Task<Result<TResult>> DispatchAsync<TRequest, TResult>(TRequest request, CancellationToken cancellation = default(CancellationToken)) where TRequest: IRequest<TResult>
    {
        var handler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResult>>();
        return await handler.HandleAsync(request, cancellation);
    }
}