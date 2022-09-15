using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;

namespace Vouchers.EntityFramework.Configurations
{
    public class UnitTypeConfiguration : IEntityTypeConfiguration<UnitType>
    {
        public void Configure(EntityTypeBuilder<UnitType> builder)
        {
            builder.ToTable(nameof(UnitType));

            builder.HasKey(unitType => unitType.Id);

            builder.Property(unitType => unitType.IssuerId).IsRequired();

            builder.Property(unitType => unitType.Supply)
                    .IsRequired()
                    .HasPrecision(18, 2);

            builder
                .HasOne(unitType => unitType.Issuer)
                .WithMany()
                .IsRequired()
                .HasForeignKey(unitType => unitType.IssuerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property<byte[]>("RowVersion").IsRowVersion();
        }
    }
}
