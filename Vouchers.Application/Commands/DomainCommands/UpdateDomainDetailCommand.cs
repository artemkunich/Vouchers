using Microsoft.AspNetCore.Http;
using System;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.DomainCommands;

public sealed class UpdateDomainDetailCommand : IRequest<Unit>
{
    public Guid DomainId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool? IsPublic { get; set; }

    public CropParametersDto CropParameters { get; set; }

    public IFormFile Image { get; set; }
}