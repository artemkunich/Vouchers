using System;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Queries;

[ApplicationRoles(ApplicationRole.User)]
public sealed class DomainDetailQuery
{
    [Required]
    public Guid Id { get; init; }
}