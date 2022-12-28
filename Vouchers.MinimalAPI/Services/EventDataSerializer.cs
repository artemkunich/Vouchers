using System.Text.Json;
using Vouchers.Application.Infrastructure;

namespace Vouchers.MinimalAPI.Services;

public class EventDataSerializer : IEventDataSerializer
{
    public async Task<string> Serialize(object @event)
    {
        using var stream = new MemoryStream();
        await JsonSerializer.SerializeAsync(stream, @event, @event.GetType());
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}