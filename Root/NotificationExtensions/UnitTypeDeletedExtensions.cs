using Vouchers.Core.Application.UseCases.UnitTypeCases;
using Vouchers.Domains.Application.UseCases.VoucherValueCases;

namespace Vouchers.Root.NotificationExtensions;

public static class UnitTypeDeletedExtensions
{
    public static DeleteVoucherValueCommand ToDeleteVoucherValueCommand(
        this UnitTypeDeletedNotification notification) => new()
    {
        VoucherValueId = notification.UnitTypeId
    };
}