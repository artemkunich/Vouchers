using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Vouchers.Domains.Domain;

namespace Vouchers.Domains.Persistence.Configurations;

internal class VoucherValueConfiguration : IEntityTypeConfiguration<VoucherValue>
{
    public void Configure(EntityTypeBuilder<VoucherValue> builder)
    {
        builder.ToTable(nameof(VoucherValue));

        builder.HasKey(valueDetail => valueDetail.Id);

        builder.Property(value => value.DomainId).IsRequired();

        builder.HasIndex(value => new {value.Ticker, value.DomainId}).IsUnique();
            
        builder.Property(value => value.Ticker).IsRequired();

        builder.Property(value => value.IssuerIdentityId).IsRequired();

        builder.Property<byte[]>("RowVersion").IsRowVersion();
            
    }
}