using Vouchers.Common.Application.Abstractions;

namespace Vouchers.Core.Application.UseCases.UnitTypeCases;

public record UnitTypeDeletedDomainEvent(Guid UnitTypeId) : IDomainEvent;