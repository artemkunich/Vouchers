using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;
using Vouchers.Identities;

namespace Vouchers.EntityFramework.Configurations
{
    public class IdentityDetailConfiguration : IEntityTypeConfiguration<IdentityDetail>
    {
        public void Configure(EntityTypeBuilder<IdentityDetail> builder)
        {
            builder.ToTable(nameof(IdentityDetail));

            builder.HasKey(identityDetail => identityDetail.Id);

            builder
                .Property<Guid>("IdentityId")
                .HasColumnName("IdentityId")
                .IsRequired();
            builder
                .HasIndex("IdentityId")
                .IsUnique();
            builder
                .HasOne(detail => detail.Identity)
                .WithMany()
                .HasForeignKey("IdentityId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property<byte[]>("RowVersion").IsRowVersion();
        }
            
    }
}
