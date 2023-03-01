using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Primitives;

namespace Vouchers.Application.Abstractions;

public interface IEventHandler<in TRequest> where TRequest : IEvent
{
    Task<Result<Unit>> HandleAsync(TRequest request, CancellationToken cancellation);
}