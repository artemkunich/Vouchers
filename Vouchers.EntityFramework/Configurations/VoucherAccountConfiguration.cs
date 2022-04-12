using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;

namespace Vouchers.EntityFramework.Configurations
{
    public class VoucherAccountConfiguration : IEntityTypeConfiguration<VoucherAccount>
    {
        public void Configure(EntityTypeBuilder<VoucherAccount> builder)
        {
            builder.ToTable(nameof(VoucherAccount));

            builder.HasKey(account => account.Id);

            builder.Property<Guid>("HolderId");
            builder.Property<Guid>("UnitId");

            builder.HasIndex("HolderId", "UnitId");  //account => new { account.OwnerId, account.UnitId }).IsUnique();

            builder.Property<byte[]>("RowVersion").IsRowVersion();

            builder.Property(account => account.Balance)
                    .IsRequired()
                    .HasPrecision(18, 2);

            builder
                .HasOne(account=>account.Holder)
                .WithMany()
                .HasForeignKey("HolderId")
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(account=>account.Unit)
                .WithMany()
                .HasForeignKey("UnitId")
                .OnDelete(DeleteBehavior.Restrict);
        }
            
    }
}
