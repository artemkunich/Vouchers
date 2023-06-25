using Akunich.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Queries;

public sealed class IdentityDomainOffersQuery : ListQuery, IRequest<IReadOnlyList<DomainOfferDto>>
{
}