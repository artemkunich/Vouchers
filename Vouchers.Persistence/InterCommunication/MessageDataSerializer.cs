using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Vouchers.Persistence.InterCommunication;

public class MessageDataSerializer : IMessageDataSerializer
{
    public async Task<string> SerializeAsync(object data)
    {
        using var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync(stream, data, data.GetType());
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}