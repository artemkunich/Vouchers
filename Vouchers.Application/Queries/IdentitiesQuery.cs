using Vouchers.Identities.Domain;

namespace Vouchers.Application.Queries;

[Permission(IdentityRole.Admin)]
public sealed class IdentitiesQuery : ListQuery
{
    public string Name { get; set; }

    public string OwnerName { get; set; }
}