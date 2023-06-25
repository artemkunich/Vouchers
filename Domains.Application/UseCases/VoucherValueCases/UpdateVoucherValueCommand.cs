using System;
using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.UseCases.VoucherValueCases;

public sealed class UpdateVoucherValueCommand : IRequest<Unit>
{
    [Required]
    public Guid Id { get; set; }

    public string Ticker { get; set; }

    public string Description { get; set; }
    
}