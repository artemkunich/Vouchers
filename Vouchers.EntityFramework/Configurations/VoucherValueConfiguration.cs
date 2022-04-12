using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;

namespace Vouchers.EntityFramework.Configurations
{
    public class VoucherValueConfiguration : IEntityTypeConfiguration<VoucherValue>
    {
        public void Configure(EntityTypeBuilder<VoucherValue> builder)
        {
            builder.ToTable(nameof(VoucherValue));

            builder.HasKey(value => value.Id);

            builder.Property<Guid>("IssuerId").IsRequired().HasColumnName("IssuerId");

            builder.Property<byte[]>("RowVersion").IsRowVersion();

            builder.Property(value => value.Supply)
                    .IsRequired()
                    .HasPrecision(18, 2)
                    .HasColumnName("Supply");

            builder
                .HasOne(value => value.Issuer)
                .WithMany()
                .IsRequired()
                .HasForeignKey("IssuerId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
