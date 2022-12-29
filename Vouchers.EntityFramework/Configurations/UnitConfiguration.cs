using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;

namespace Vouchers.EntityFramework.Configurations
{
    internal class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.ToTable(nameof(Unit));

            builder.HasKey(unit => unit.Id);

            builder.HasIndex(unit => new {unit.UnitTypeId, unit.ValidFrom, unit.ValidTo, unit.CanBeExchanged}).IsUnique();
          
            builder
                .Property(voucher => voucher.Supply)
                .IsRequired()
                .HasPrecision(18, 2);

            builder
                .Property(voucher => voucher.ValidFrom)
                .IsRequired();

            builder
                .Property(voucher => voucher.ValidTo)
                .IsRequired();

            builder
                .Property(voucher => voucher.CanBeExchanged)
                .IsRequired();

            builder
                .HasOne(voucher => voucher.UnitType)
                .WithMany()
                .IsRequired()
                .HasForeignKey(voucher => voucher.UnitTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property<byte[]>("RowVersion").IsRowVersion();
            
        }
    }
}
