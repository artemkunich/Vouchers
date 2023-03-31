using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;
using Vouchers.Domains.Application.Permissions;

namespace Vouchers.Domains.Application.UseCases.IdentityCases;

[IdentityPermission]
public sealed class CreateIdentityCommand : IRequest<Dtos.IdDto<Guid>>
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Email { get; set; }

    public IFormFile Image { get; set; }

    public CropParametersDto CropParameters { get; set; }
}