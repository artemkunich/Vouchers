using Akunich.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Queries;

//[Permission]
public sealed class IdentityDetailQuery : IRequest<IdentityDetailDto>
{
    public Guid? AccountId { get; init; }
}