using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;

namespace Vouchers.EntityFramework.Configurations
{
    public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.ToTable(nameof(Voucher));

            builder.HasKey(voucher => voucher.Id);

            builder.Property<Guid>("ValueId").HasColumnName("ValueId");

            builder.HasIndex("ValueId", "ValidFrom", "ValidTo", "CanBeExchanged").IsUnique();

            builder.Property<byte[]>("RowVersion").IsRowVersion();

            builder
                .Property(voucher => voucher.Supply)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasColumnName("Supply");

            builder
                .Property(voucher => voucher.ValidFrom)
                .IsConcurrencyToken()
                .IsRequired()
                .HasColumnName("ValidFrom");
            builder
                .Property(voucher => voucher.ValidTo)
                .IsConcurrencyToken()
                .IsRequired()
                .HasColumnName("ValidTo");
            builder
                .Property(voucher => voucher.CanBeExchanged)
                .IsConcurrencyToken()
                .IsRequired()
                .HasColumnName("CanBeExchanged");

            builder
                .HasOne(voucher => voucher.Value)
                .WithMany()
                .IsRequired()
                .HasForeignKey("ValueId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
