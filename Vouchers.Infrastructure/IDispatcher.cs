using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application;
using Vouchers.Application.Abstractions;
using Vouchers.Application.UseCases;

namespace Vouchers.Infrastructure;

public interface IDispatcher
{
    Task<Result<TResult>> DispatchAsync<TRequest, TResult>(TRequest request, CancellationToken cancellation = default);
}