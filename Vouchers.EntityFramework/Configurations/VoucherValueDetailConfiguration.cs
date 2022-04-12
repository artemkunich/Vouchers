using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Values;
using Vouchers.Core;

namespace Vouchers.EntityFramework.Configurations
{
    public class VoucherValueDetailConfiguration : IEntityTypeConfiguration<VoucherValueDetail>
    {
        public void Configure(EntityTypeBuilder<VoucherValueDetail> builder)
        {
            //builder.HasIndex(value => new { value.Category, value.Name, value.Issuer}).IsUnique();

            builder.ToTable(nameof(VoucherValueDetail));

            builder.HasKey(valueDetail => valueDetail.Id);

            builder.Property<Guid>("DomainId").IsRequired();

            builder.HasIndex("Ticker", "DomainId").IsUnique();

            builder.Property<byte[]>("RowVersion").IsRowVersion();

            builder.Property(value => value.Ticker)
                    .IsRequired();

            builder.Property<Guid>("ValueId").HasColumnName("ValueId").IsRequired();
            builder.HasIndex("ValueId").IsUnique();
            builder
                .HasOne(detail => detail.Value)
                .WithMany()
                .HasForeignKey("ValueId")
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(detail => detail.Domain)
                .WithMany()
                .HasForeignKey("DomainId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
