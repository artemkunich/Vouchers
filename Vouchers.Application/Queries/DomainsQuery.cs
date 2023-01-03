using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vouchers.Application.Queries;

public sealed class DomainsQuery : ListQuery
{
    public string Name { get; set; }

    public string OwnerName { get; set; }
}