using Microsoft.EntityFrameworkCore;
using System.Reflection;

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