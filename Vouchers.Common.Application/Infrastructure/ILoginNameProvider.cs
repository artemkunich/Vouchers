namespace Vouchers.Common.Application.Infrastructure;

public interface ILoginNameProvider
{
    string CurrentLoginName { get; }
}