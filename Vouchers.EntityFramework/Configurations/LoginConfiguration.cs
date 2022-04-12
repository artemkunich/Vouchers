using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;
using Vouchers.Identities;

namespace Vouchers.EntityFramework.Configurations
{
    public class LoginConfiguration : IEntityTypeConfiguration<Login>
    {
        public void Configure(EntityTypeBuilder<Login> builder)
        {
            builder.ToTable(nameof(Login));

            builder.HasKey(login => login.Id);

            builder.Property(login => login.LoginName).IsRequired();

            builder.Property<Guid>("IdentityId").HasColumnName("IdentityId").IsRequired();
            builder.HasIndex("IdentityId").IsUnique();
            builder
                .HasOne(login => login.Identity)
                .WithMany()
                .HasForeignKey("IdentityId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property<byte[]>("RowVersion").IsRowVersion();
        }
            
    }
}
