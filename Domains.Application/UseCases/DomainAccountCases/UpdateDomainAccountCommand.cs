using System;
using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;

namespace Vouchers.Domains.Application.UseCases.DomainAccountCases;

public sealed class UpdateDomainAccountCommand : IRequest<Unit>
{      
    [Required]
    public Guid DomainAccountId { get; set; }

    public bool? IsConfirmed { get; set; }

    public bool? IsIssuer { get; set; }

    public bool? IsAdmin { get; set; }
}