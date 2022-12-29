using System.Threading.Tasks;
using Vouchers.Application.Events.IdentityEvents;

namespace Vouchers.Application.Infrastructure;

public interface IMessageDataSerializer
{
    Task<string> Serialize(object data);
}