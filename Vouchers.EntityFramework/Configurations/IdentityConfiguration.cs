using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;
using Vouchers.Identities;

namespace Vouchers.EntityFramework.Configurations
{
    public class IdentityConfiguration : IEntityTypeConfiguration<Identity>
    {
        public void Configure(EntityTypeBuilder<Identity> builder)
        {
            builder.ToTable(nameof(Identity));

            builder.HasKey(identity => identity.Id);

            builder.Property<byte[]>("RowVersion").IsRowVersion();
        }
            
    }
}
