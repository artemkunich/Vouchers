using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;

namespace Vouchers.EntityFramework.Configurations
{
    public class HolderTransactionConfiguration : IEntityTypeConfiguration<HolderTransaction>
    {
        public void Configure(EntityTypeBuilder<HolderTransaction> builder)
        {
            builder.ToTable(nameof(HolderTransaction));

            builder.HasKey(transaction => transaction.Id);

            builder
                .OwnsOne(transaction => transaction.Quantity)
                .Property(quantity => quantity.Amount)
                .HasPrecision(18, 2);

            builder
                .OwnsOne(transaction => transaction.Quantity)
                .HasOne(quantity => quantity.Unit)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(transaction => transaction.Creditor)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            
            builder
                .HasOne(transaction => transaction.Debtor)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(transaction => transaction.TransactionItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(transaction => transaction.IsPerformed);
        }
    }
}
