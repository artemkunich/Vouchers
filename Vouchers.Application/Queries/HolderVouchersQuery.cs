using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Queries;

[ApplicationRoles(ApplicationRole.User)]
public sealed class HolderVouchersQuery : ListQuery
{
    public Guid ValueId { get; set; }
}