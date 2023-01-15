using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core.Domain;

namespace Vouchers.Persistence.Configurations;

internal class IssuerTransactionConfiguration : IEntityTypeConfiguration<IssuerTransaction>
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
            .HasOne(transaction => transaction.IssuerAccountItem)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

    }
}