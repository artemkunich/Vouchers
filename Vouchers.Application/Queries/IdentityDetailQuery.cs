using System;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Queries;

public sealed class IdentityDetailQuery
{
    public Guid? AccountId { get; init; }
}