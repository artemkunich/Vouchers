using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vouchers.Core.Domain;

namespace Vouchers.Core.Persistence.Configurations;

internal class AccountItemConfiguration : IEntityTypeConfiguration<AccountItem>
{
    public void Configure(EntityTypeBuilder<AccountItem> builder)
    {
        builder.ToTable(nameof(AccountItem));

        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).IsRequired().ValueGeneratedNever();

        builder.HasIndex(item => new { item.HolderAccountId, item.UnitId });  //account => new { account.OwnerId, account.UnitId }).IsUnique();

        builder.Property<byte[]>("RowVersion").IsRowVersion();

        builder.Property(item => item.Balance)
            .IsRequired()
            .HasPrecision(18, 2);

        builder
            .HasOne(item => item.HolderAccount)
            .WithMany()
            .HasForeignKey(account => account.HolderAccountId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Navigation(item => item.HolderAccount)
            .AutoInclude();
        
        builder
            .HasOne(item => item.Unit)
            .WithMany()
            .HasForeignKey(account => account.UnitId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Navigation(item => item.Unit)
            .AutoInclude();
    }
            
}