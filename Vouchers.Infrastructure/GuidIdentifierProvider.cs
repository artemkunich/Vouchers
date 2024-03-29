using Vouchers.Application.Infrastructure;

namespace Vouchers.Infrastructure;

public class GuidIdentifierProvider : IIdentifierProvider<Guid>
{
    public Guid CreateNewId() => Guid.NewGuid();
}