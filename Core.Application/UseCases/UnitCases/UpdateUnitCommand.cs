using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;

namespace Vouchers.Core.Application.UseCases.UnitCases;

public sealed class UpdateUnitCommand  : IRequest<Unit>
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid UnitTypeId { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public bool? CanBeExchanged { get; set; }
  
}