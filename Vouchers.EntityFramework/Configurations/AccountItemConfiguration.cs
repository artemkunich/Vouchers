using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;

namespace Vouchers.EntityFramework.Configurations
{
    public class AccountItemConfiguration : IEntityTypeConfiguration<AccountItem>
    {
        public void Configure(EntityTypeBuilder<AccountItem> builder)
        {
            builder.ToTable(nameof(AccountItem));

            builder.HasKey(account => account.Id);
            builder.Property(account => account.Id).IsRequired().ValueGeneratedNever();

            builder.HasIndex(item => new { item.HolderAccountId, item.UnitId });  //account => new { account.OwnerId, account.UnitId }).IsUnique();

            builder.Property<byte[]>("RowVersion").IsRowVersion();

            builder.Property(account => account.Balance)
                    .IsRequired()
                    .HasPrecision(18, 2);

            builder
                .HasOne(account=>account.HolderAccount)
                .WithMany()
                .HasForeignKey(account => account.HolderAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(account=>account.Unit)
                .WithMany()
                .HasForeignKey(account => account.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
            
    }
}
