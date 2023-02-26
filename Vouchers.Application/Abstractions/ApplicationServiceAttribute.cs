using System;

namespace Vouchers.Application.Abstractions;

[AttributeUsage(AttributeTargets.Class)]
public class ApplicationServiceAttribute : Attribute
{
    public Type ServiceType { get; }

    public ApplicationServiceAttribute(Type serviceType)
    {
        ServiceType = serviceType;
    }
}