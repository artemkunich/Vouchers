using Vouchers.Common.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Queries;

[Permission(IdentityRole.Manager)]
public sealed class DomainOffersQuery : ListQuery, IRequest<IReadOnlyList<DomainOfferDto>>
{
    public string Name { get; set; }

    public int? MinMaxSubscribersCount { get; set; }
    public int? MaxMaxSubscribersCount { get; set; }

    public int? InvoicePeriod { get; set; }
    public int? Currency { get; set; }
    public decimal? MaxAmount { get; set; }
    public decimal? MinAmount { get; set; }

    public Guid? RecipientId { get; set; }

    public bool? IncludeExpired { get; set; }
    public bool? IncludePlanned { get; set; }
}