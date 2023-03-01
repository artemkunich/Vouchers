using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vouchers.Application.Abstractions;

public interface IEventPipelineBehavior<in TRequest>
{
    Task<Result<Unit>> HandleAsync(TRequest request, CancellationToken cancellation, HandlerDelegate<Unit> next);
}