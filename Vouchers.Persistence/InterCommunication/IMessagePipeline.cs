using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Abstractions;

namespace Vouchers.Persistence.InterCommunication;

public interface IMessagePipeline<in TMessage>
{
    Task<Result<Unit>> HandleAsync(TMessage message, CancellationToken cancellation);
}