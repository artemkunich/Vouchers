using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vouchers.EntityFramework
{
    public interface IDbContextFactory
    {
        VouchersDbContext CreateVouchersContext();
    }
}
