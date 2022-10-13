using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.Application.Queries
{
    public sealed class HolderVouchersQuery : ListQuery
    {
        public Guid ValueId { get; set; }
    }
}
