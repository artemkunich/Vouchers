﻿using System.ComponentModel.DataAnnotations;
using Vouchers.Common.Application.Abstractions;

namespace Vouchers.Core.Application.UseCases.UnitCases;

public sealed class DeleteUnitCommand : IRequest<Unit>
{
    [Required]
    public Guid UnitTypeId { get; }

    [Required]
    public Guid UnitId { get; }
}