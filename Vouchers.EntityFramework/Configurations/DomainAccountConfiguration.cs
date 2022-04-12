using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;

namespace Vouchers.EntityFramework.Configurations
{
    public class DomainAccountConfiguration : IEntityTypeConfiguration<DomainAccount>
    {
        public void Configure(EntityTypeBuilder<DomainAccount> builder)
        {
            builder.ToTable(nameof(DomainAccount));

            builder.HasKey(account => account.Id);

            builder
                .Property<Guid>("DomainId")
                .HasColumnName("DomainId")
                .IsRequired();

            builder
                .Property<Guid>("IdentityId")
                .HasColumnName("IdentityId")
                .IsRequired();

            builder
                .HasIndex("DomainId", "IdentityId")
                .IsUnique();

            builder
                .Property(userAccount => userAccount.Supply)
                .HasPrecision(18, 2)
                .HasDefaultValue(0);

            builder
                .HasOne(domainAccount => domainAccount.Domain)
                .WithMany()
                .HasForeignKey("DomainId")
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(domainAccount => domainAccount.Identity)
                .WithMany()
                .HasForeignKey("IdentityId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property<byte[]>("RowVersion").IsRowVersion();

        }
    }
}
