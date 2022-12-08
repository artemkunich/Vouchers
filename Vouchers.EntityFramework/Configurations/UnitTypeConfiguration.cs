using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;

namespace Vouchers.EntityFramework.Configurations
{
    internal class UnitTypeConfiguration : IEntityTypeConfiguration<UnitType>
    {
        public void Configure(EntityTypeBuilder<UnitType> builder)
        {
            builder.ToTable(nameof(UnitType));

            builder.HasKey(unitType => unitType.Id);

            builder.Property(unitType => unitType.IssuerAccountId).IsRequired();

            builder.Property(unitType => unitType.Supply)
                    .IsRequired()
                    .HasPrecision(18, 2);

            builder
                .HasOne(unitType => unitType.IssuerAccount)
                .WithMany()
                .IsRequired()
                .HasForeignKey(unitType => unitType.IssuerAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property<byte[]>("RowVersion").IsRowVersion();
        }
    }
}
