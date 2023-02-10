using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Queries;

[ApplicationRoles(ApplicationRole.User)]
public sealed class SubscribersQuery : ListQuery
{
    [Required]
    public Guid DomainId { get; set; }

    public string IdentityName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}