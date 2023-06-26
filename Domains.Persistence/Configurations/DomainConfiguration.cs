using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Vouchers.Domains.Domain;

namespace Vouchers.Domains.Persistence.Configurations;

internal class DomainConfiguration : IEntityTypeConfiguration<Domain.Domain>
{
    public void Configure(EntityTypeBuilder<Domain.Domain> builder)
    {
        builder.ToTable(nameof(Domain));

        builder.HasKey(domain => domain.Id);

        builder.Property(domain => domain.ContractId).IsRequired();
        builder
            .HasIndex(domain => domain.ContractId).IsUnique();
        builder
            .HasOne(contract => contract.Contract)
            .WithMany()
            .HasForeignKey(domain => domain.ContractId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(domain => domain.Credit)
            .HasPrecision(18, 2)
            .HasDefaultValue(0);

        builder.Property<byte[]>("RowVersion").IsRowVersion();
            
    }
}