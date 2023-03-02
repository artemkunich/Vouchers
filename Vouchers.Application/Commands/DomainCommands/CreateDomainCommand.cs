using System;
using System.ComponentModel.DataAnnotations;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Dtos;

namespace Vouchers.Application.Commands.DomainCommands;

public sealed class CreateDomainCommand : IRequest<IdDto<Guid>>
{
    [Required]
    public Guid OfferId { get; set; }

    [Required]
    public string DomainName { get; set; }
}