using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Vouchers.Core.Domain;
using Vouchers.Files.Domain;

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
        modelBuilder
            .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
            .ApplyConfigurationsFromAssembly(typeof(Account).Assembly)
            .ApplyConfigurationsFromAssembly(typeof(Domains.Domain.Domain).Assembly)
            .ApplyConfigurationsFromAssembly(typeof(Image).Assembly);
    }
}