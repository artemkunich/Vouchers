using System;
using System.Collections.Generic;

namespace Vouchers.Application;

[AttributeUsage(AttributeTargets.Class)]
public class IdentityRolesAttribute : Attribute
{
    private readonly HashSet<IdentityRole> _roles;
    public IReadOnlySet<IdentityRole> Roles => _roles;

    public IdentityRolesAttribute(params IdentityRole[] roles)
    {
        _roles = new HashSet<IdentityRole>();

        foreach (var role in roles)
        {
            _roles.Add(role);
        }
    }
}