using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application;
using Vouchers.Application.Abstractions;
using Vouchers.Application.UseCases;

namespace Vouchers.Application.Infrastructure;

public interface IEventDispatcher
{
    Task<Result<Unit>> DispatchAsync<TEvent>(TEvent request, CancellationToken cancellation = default) where TEvent: IDomainEvent;
}