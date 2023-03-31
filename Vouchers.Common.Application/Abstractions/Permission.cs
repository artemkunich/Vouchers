namespace Vouchers.Common.Application.Abstractions;

[AttributeUsage(AttributeTargets.Class)]
public abstract class PermissionAttribute : Attribute
{
    public virtual string[] Roles => Array.Empty<string>();
}