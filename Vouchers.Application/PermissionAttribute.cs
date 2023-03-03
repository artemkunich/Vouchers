using System;
using System.Collections.Generic;
using Vouchers.Identities.Domain;

namespace Vouchers.Application;

[AttributeUsage(AttributeTargets.Class)]
public class PermissionAttribute : Attribute
{
    public static readonly IEnumerable<IdentityRole> DefaultRoles = new[] {IdentityRole.User};
    
    private readonly HashSet<IdentityRole> _roles;
    public IEnumerable<IdentityRole> Roles => _roles;

    public PermissionAttribute(params IdentityRole[] roles)
    {
        _roles = new HashSet<IdentityRole>();

        foreach (var role in roles)
        {
            _roles.Add(role);
        }
    }
}