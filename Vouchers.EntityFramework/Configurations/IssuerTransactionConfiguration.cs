using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;

namespace Vouchers.EntityFramework.Configurations
{
    public class IssuerTransactionConfiguration : IEntityTypeConfiguration<IssuerTransaction>
    {
        public void Configure(EntityTypeBuilder<IssuerTransaction> builder)
        {
            builder.ToTable(nameof(IssuerTransaction));

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
                .HasOne(transaction => transaction.IssuerAccount)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
