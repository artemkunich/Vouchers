using Vouchers.Common.Application.Infrastructure;

namespace Vouchers.Infrastructure;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime CurrentDateTime() => DateTime.UtcNow;
}