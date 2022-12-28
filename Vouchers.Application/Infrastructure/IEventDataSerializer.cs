using System.Threading.Tasks;

namespace Vouchers.Application.Infrastructure;

public interface IEventDataSerializer
{
    Task<string> Serialize(object @event);
}