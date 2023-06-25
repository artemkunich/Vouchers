using Akunich.Application.Abstractions;
using Vouchers.Domains.Application.Dtos;

namespace Vouchers.Domains.Application.Queries;

//[Permission(IdentityRole.Admin)]
public sealed class LoginsQuery : ListQuery, IRequest<IReadOnlyList<LoginDto>>
{
    public string LoginName { get; set; }

    public string IdentityName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}