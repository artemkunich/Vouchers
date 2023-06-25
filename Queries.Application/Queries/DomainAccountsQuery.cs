using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Queries;

public sealed class DomainAccountsQuery : ListQuery, IRequest<IReadOnlyList<DomainAccountDto>>
{
    [Required]
    public Guid DomainId { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    public bool IncludeConfirmed { get; set; } = true;

    public bool IncludeNotConfirmed { get; set; } = false;
}