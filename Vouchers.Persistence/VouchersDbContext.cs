using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Vouchers.Core;
using Vouchers.Domains;
using Vouchers.Primitives;
using Vouchers.Files;
using Vouchers.Identities;
using Vouchers.InterCommunication;
using Vouchers.Persistence.Configurations;
using Vouchers.Persistence.InterCommunication;
using Vouchers.Values;

namespace Vouchers.Persistence;

public sealed class VouchersDbContext : DbContext
{
    public VouchersDbContext()
    {

    }

    public VouchersDbContext(DbContextOptions<VouchersDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}