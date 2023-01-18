using Vouchers.Application.Infrastructure;

namespace Vouchers.Infrastructure.InterCommunication;

public class GuidIdentifierProvider : IIdentifierProvider<Guid>
{
    public Guid CreateNewId() => Guid.NewGuid();
}