using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Vouchers.Core.Domain;

namespace Vouchers.Core.Persistence.Configurations;

internal class IssuerTransactionConfiguration : IEntityTypeConfiguration<IssuerTransaction>
{
    public void Configure(EntityTypeBuilder<IssuerTransaction> builder)
    {
        builder.ToTable(nameof(IssuerTransaction));

        builder.HasKey(transaction => transaction.Id);
            
        builder.Property(transaction => transaction.Amount).HasPrecision(18,2);

        builder
            .HasOne(transaction => transaction.IssuerAccountItem)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

    }
}