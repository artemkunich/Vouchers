using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;

namespace Vouchers.EntityFramework.Configurations
{
    public class HolderTransactionItemConfiguration : IEntityTypeConfiguration<HolderTransactionItem>
    {
        public void Configure(EntityTypeBuilder<HolderTransactionItem> builder)
        {
            builder.ToTable(nameof(HolderTransactionItem));

            builder.HasKey(item => item.Id);

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
                .HasOne(item => item.CreditAccount)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(item => item.DebitAccount)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
