using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Vouchers.Domains.Domain;

namespace Vouchers.Domains.Persistence.Configurations;

internal class DomainAccountConfiguration : IEntityTypeConfiguration<DomainAccount>
{
    public void Configure(EntityTypeBuilder<DomainAccount> builder)
    {
        builder.ToTable(nameof(DomainAccount));

        builder.HasKey(account => account.Id);

        builder.Property(account => account.DomainId).IsRequired();

        builder.Property(account => account.IdentityId).IsRequired();

        builder
            .HasIndex(account => new { account.DomainId, account.IdentityId })
            .IsUnique();

        builder
            .HasOne(account => account.Domain)
            .WithMany()
            .HasForeignKey(account => account.DomainId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property<byte[]>("RowVersion").IsRowVersion();
    }
}