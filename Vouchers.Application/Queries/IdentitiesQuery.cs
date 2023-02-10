using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Queries;

[ApplicationRoles(ApplicationRole.Admin)]
public sealed class IdentitiesQuery : ListQuery
{
    public string Name { get; set; }

    public string OwnerName { get; set; }
}