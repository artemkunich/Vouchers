namespace Vouchers.Common.Application.Infrastructure;

public interface IIdentityIdProvider
{
    Guid CurrentIdentityId { get; }
}