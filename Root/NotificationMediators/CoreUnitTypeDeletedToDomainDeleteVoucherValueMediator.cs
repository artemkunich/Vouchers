using Akunich.Application.Abstractions;
using Vouchers.Core.Application.UseCases.UnitTypeCases;
using Vouchers.Domains.Application.UseCases.VoucherValueCases;

namespace Vouchers.Infrastructure.NotificationMediators;

public class CoreUnitTypeDeletedToDomainDeleteVoucherValueMediator 
    : NotificationMediator<UnitTypeDeletedNotification, DeleteVoucherValueCommand>
{
    public CoreUnitTypeDeletedToDomainDeleteVoucherValueMediator(IRequestHandler<DeleteVoucherValueCommand, Unit> commandHandler) : base(commandHandler)
    {
    }

    protected override DeleteVoucherValueCommand Map(
        UnitTypeDeletedNotification notification) => new()
    {
        VoucherValueId = notification.UnitTypeId
    };
}