using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;
using Vouchers.Primitives;
using Vouchers.Files.Domain;
using Vouchers.InterCommunication;
using Vouchers.Persistence.Configurations;
using Vouchers.Persistence.InterCommunication;

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