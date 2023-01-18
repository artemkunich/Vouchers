using System;

namespace Vouchers.Application.Infrastructure;

public interface IDateTimeProvider
{
    DateTime CurrentDateTime();
}