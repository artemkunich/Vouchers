using System;
using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.UseCases.IdentityCases;

//[IdentityPermission]
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