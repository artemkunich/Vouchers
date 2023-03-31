using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core.Domain;

namespace Vouchers.Persistence.Configurations;

internal class LoginConfiguration : IEntityTypeConfiguration<Login>
{
    public void Configure(EntityTypeBuilder<Login> builder)
    {
        builder.ToTable(nameof(Login));

        builder.HasKey(login => login.Id);

        builder.Property(login => login.LoginName).IsRequired();

        builder.Property(login => login.IdentityId).IsRequired();
        builder.HasIndex(login => login.IdentityId).IsUnique();
        builder
            .HasOne(login => login.Identity)
            .WithMany()
            .HasForeignKey(login => login.IdentityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property<byte[]>("RowVersion").IsRowVersion();
            
    }
}