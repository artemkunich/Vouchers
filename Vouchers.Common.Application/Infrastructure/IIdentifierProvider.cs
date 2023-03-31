namespace Vouchers.Common.Application.Infrastructure;

public interface IIdentifierProvider<TKey>
{
    TKey CreateNewId();
}