namespace Vouchers.Application.Infrastructure;

public interface IIdentifierProvider<TKey>
{
    TKey CreateNewId();
}