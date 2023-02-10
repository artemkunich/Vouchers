using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vouchers.Application.Queries;

[ApplicationRoles(ApplicationRole.User)]
public sealed class DomainAccountsQuery : ListQuery
{
    [Required]
    public Guid DomainId { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    public bool IncludeConfirmed { get; set; } = true;

    public bool IncludeNotConfirmed { get; set; } = false;
}