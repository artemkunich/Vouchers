using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.VoucherValueCommands;

public sealed class UpdateVoucherValueCommand : IRequest<Unit>
{
    [Required]
    public Guid Id { get; set; }

    public string Ticker { get; set; }

    public string Description { get; set; }

    public IFormFile Image { get; set; }

    public CropParametersDto CropParameters { get; set; }
}