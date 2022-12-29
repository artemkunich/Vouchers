using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.UseCases;

namespace Vouchers.Application.Infrastructure
{
    public interface IDispatcher
    {
        Task DispatchAsync<TMessage>(TMessage request, CancellationToken cancellation = default);
        Task<TResult> DispatchAsync<TRequest, TResult>(TRequest request, CancellationToken cancellation = default);
    }
}
