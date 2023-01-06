using System.Threading.Tasks;

namespace Vouchers.Persistence.InterCommunication;

public interface IMessageDataSerializer
{
    Task<string> SerializeAsync(object data);
}