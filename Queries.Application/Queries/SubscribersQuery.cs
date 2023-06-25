using System.ComponentModel.DataAnnotations;
using Akunich.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Queries;

public sealed class SubscribersQuery : ListQuery, IRequest<IReadOnlyList<SubscriberDto>>
{
    [Required]
    public Guid DomainId { get; set; }

    public string IdentityName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}