using System;

namespace Vouchers.Application;

[AttributeUsage(AttributeTargets.Class)]
public class ApplicationRolesAttribute : Attribute
{
    public ApplicationRole[] Roles { get; }

    public ApplicationRolesAttribute(params ApplicationRole[] roles)
    {
        Roles = roles;
    }
}