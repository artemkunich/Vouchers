using System.Threading;
using System.Threading.Tasks;

namespace Vouchers.Persistence.InterCommunication;

public interface IMessagesProcessor
{
    Task<int> ProcessMessagesAsync(CancellationToken cancellation);
}