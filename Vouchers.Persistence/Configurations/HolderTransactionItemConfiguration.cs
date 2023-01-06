using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;

namespace Vouchers.Persistence.Configurations;

internal class HolderTransactionItemConfiguration : IEntityTypeConfiguration<HolderTransactionItem>
{
    public void Configure(EntityTypeBuilder<HolderTransactionItem> builder)
    {
        builder.ToTable(nameof(HolderTransactionItem));
            
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).IsRequired().ValueGeneratedNever();

        builder
            .OwnsOne(item => item.Quantity)
            .Property(quantity => quantity.Amount)
            .HasPrecision(18, 2);

        builder
            .OwnsOne(item => item.Quantity)
            .HasOne(quantity => quantity.Unit)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

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