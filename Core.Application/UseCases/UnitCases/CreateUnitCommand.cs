using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;
using Vouchers.Core.Application.Dtos;

namespace Vouchers.Core.Application.UseCases.UnitCases;

public sealed class CreateUnitCommand : IRequest<IdDto<Guid>>
{
    [Required]
    public Guid UnitTypeId { get; set; }

    [Required]
    public DateTime ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public bool CanBeExchanged { get; set; }
}