using System;

namespace Vouchers.Application.PipelineBehaviors;

[AttributeUsage(AttributeTargets.Class)]
public class PipelineBehaviorPriorityAttribute : Attribute
{
    public uint Priority { get; }

    public PipelineBehaviorPriorityAttribute(uint priority)
    {
        Priority = priority;
    }
}