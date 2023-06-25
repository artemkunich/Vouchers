using Akunich.Application.Abstractions;

namespace Vouchers.Core.Application.UseCases.UnitTypeCases;

public record UnitTypeDeletedNotification(Guid UnitTypeId) : INotification;