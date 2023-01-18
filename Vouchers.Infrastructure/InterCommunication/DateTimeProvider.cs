using Vouchers.Application.Infrastructure;

namespace Vouchers.Infrastructure.InterCommunication;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime CurrentDateTime() => DateTime.UtcNow;
}