using System.Text.Json;
using Vouchers.Application.Infrastructure;

namespace Vouchers.Infrastructure;

public class MessageDataSerializer : IMessageDataSerializer
{
    public async Task<string> Serialize(object data)
    {
        using var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync(stream, data, data.GetType());
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}