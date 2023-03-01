using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.DomainCommands;

[IdentityRoles(IdentityRole.User)]
public sealed class UpdateDomainDetailCommand : IRequest<Unit>
{
    public Guid DomainId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool? IsPublic { get; set; }

    public CropParametersDto CropParameters { get; set; }

    public IFormFile Image { get; set; }
}