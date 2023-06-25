using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;

namespace Vouchers.Core.Application.UseCases.UnitTypeCases;

public sealed class DeleteUnitTypeCommand : IRequest<Unit>
{
    [Required]
    public Guid UnitTypeId { get; }
}