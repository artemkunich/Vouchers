using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;
using Vouchers.Domains;
using Vouchers.Entities;

namespace Vouchers.EntityFramework.Configurations
{
    internal class OutboxEventConfiguration : IEntityTypeConfiguration<OutboxEvent>
    {
        public void Configure(EntityTypeBuilder<OutboxEvent> builder)
        {
            builder.ToTable(nameof(OutboxEvent));

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Type);
            builder.Property(x => x.Data);
            builder.Property(x => x.CreatedDateTime);

            builder.Property<byte[]>("RowVersion").IsRowVersion();
            
            builder.Ignore(x => x.OutboxEvents);
        }
    }
}
