using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.VoucherValueCommands;

[ApplicationRoles(ApplicationRole.User)]
public sealed class CreateVoucherValueCommand
{
    [Required]
    public Guid IssuerAccountId { get; set; }

    [Required]
    public string Ticker { get; set; }

    public string Description { get; set; }

    public IFormFile Image { get; set; }

    public CropParametersDto CropParameters { get; set; }
    
}