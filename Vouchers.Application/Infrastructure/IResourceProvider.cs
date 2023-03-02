namespace Vouchers.Application.Infrastructure;

public interface IResourceProvider
{
    string GetString(string resourceKey);

    string GetString(string resourceKey, params object[] args);
}