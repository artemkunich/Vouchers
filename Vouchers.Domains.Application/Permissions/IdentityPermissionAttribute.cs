using Vouchers.Common.Application.Abstractions;

namespace Vouchers.Domains.Application.Permissions;

public class IdentityPermissionAttribute : PermissionAttribute
{
    public override string[] Roles { get; } =
    {
        IdentityRoles.User, 
        IdentityRoles.Manager, 
        IdentityRoles.Admin
    };
}