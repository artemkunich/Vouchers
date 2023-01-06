using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;

namespace Vouchers.Persistence.Configurations;

internal class HolderTransactionConfiguration : IEntityTypeConfiguration<HolderTransaction>
{
    public void Configure(EntityTypeBuilder<HolderTransaction> builder)
    {
        builder.ToTable(nameof(HolderTransaction));

        builder.HasKey(transaction => transaction.Id);
        builder.Property(transaction => transaction.Id).IsRequired().ValueGeneratedNever();

        builder
            .OwnsOne(transaction => transaction.Quantity)
            .Property(quantity => quantity.Amount)
            .HasPrecision(18, 2);

        builder
            .OwnsOne(transaction => transaction.Quantity)
            .HasOne(quantity => quantity.UnitType)
            .WithMany().HasForeignKey(quantity => quantity.UnitTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(transaction => transaction.CreditorAccount)
            .WithMany().HasForeignKey(transaction => transaction.CreditorAccountId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder
            .HasOne(transaction => transaction.DebtorAccount)
            .WithMany().HasForeignKey(transaction => transaction.DebtorAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(transaction => transaction.TransactionItems)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(transaction => transaction.IsPerformed);

        builder.Property(transaction => transaction.Message);

    }
}