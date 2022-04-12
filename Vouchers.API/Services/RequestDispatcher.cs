using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.UseCases;

namespace Vouchers.MVC.Services
{
    public interface IRequestDispatcher
    {
        Task DispatchAsync<TRequest>(TRequest request, CancellationToken cancellation = default(CancellationToken));
        Task<TResult> DispatchAsync<TRequest, TResult>(TRequest request, CancellationToken cancellation = default(CancellationToken));
    }

    public class RequestDispatcher : IRequestDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public RequestDispatcher(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;

        public async Task DispatchAsync<TRequest>(TRequest request, CancellationToken cancellation = default(CancellationToken))
        {
            var handler = serviceProvider.GetRequiredService<IHandler<TRequest>>();
            await handler.HandleAsync(request, cancellation);
        }

        public async Task<TResult> DispatchAsync<TRequest, TResult>(TRequest request, CancellationToken cancellation = default(CancellationToken))
        {
            var handler = serviceProvider.GetRequiredService<IHandler<TRequest, TResult>>();
            return await handler.HandleAsync(request, cancellation);
        }
    }
}
