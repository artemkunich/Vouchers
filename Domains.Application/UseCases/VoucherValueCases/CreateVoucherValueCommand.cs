using System;
using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.UseCases.VoucherValueCases;

public sealed class CreateVoucherValueCommand : IRequest<Dtos.IdDto<Guid>>
{
    [Required]
    public Guid IssuerAccountId { get; set; }

    [Required]
    public string Ticker { get; set; }

    public string Description { get; set; }

}