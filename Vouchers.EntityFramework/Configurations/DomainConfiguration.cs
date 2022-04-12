using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;

namespace Vouchers.EntityFramework.Configurations
{
    public class DomainConfiguration : IEntityTypeConfiguration<Domain>
    {
        public void Configure(EntityTypeBuilder<Domain> builder)
        {
            builder.ToTable(nameof(Domain));

            builder.HasKey(domain => domain.Id);

            builder
                .Property(domain => domain.Credit)
                .HasPrecision(18, 2)
                .HasDefaultValue(0);

            builder.Property<byte[]>("RowVersion").IsRowVersion();
        }
            
    }
}
