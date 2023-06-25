using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;

namespace Vouchers.Persistence.InterCommunication;

public interface IIntegrationEventPipeline<in TEvent>
{
    Task<Result<Unit>> HandleAsync(TEvent message, CancellationToken cancellation);
}