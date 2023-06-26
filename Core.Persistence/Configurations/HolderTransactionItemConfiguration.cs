using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vouchers.Core.Domain;

namespace Vouchers.Core.Persistence.Configurations;

internal class HolderTransactionItemConfiguration : IEntityTypeConfiguration<HolderTransactionItem>
{
    public void Configure(EntityTypeBuilder<HolderTransactionItem> builder)
    {
        builder.ToTable(nameof(HolderTransactionItem));
            
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).IsRequired().ValueGeneratedNever();

        builder.Property(item => item.Amount).HasPrecision(18,2);

        builder
            .HasOne(item => item.CreditAccountItem)
            .WithMany().HasForeignKey(item => item.CreditAccountItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(item => item.DebitAccountItem)
            .WithMany().HasForeignKey(item => item.DebitAccountItemId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}