using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;
using Vouchers.Domains;
using Vouchers.Primitives;
using Vouchers.InterCommunication;
using Vouchers.Persistence.InterCommunication;

namespace Vouchers.Persistence.Configurations;

internal class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable(nameof(OutboxMessage));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Type);
        builder.Property(x => x.Data);
        builder.Property(x => x.CreatedDateTime);

        builder.Property<byte[]>("RowVersion").IsRowVersion();
            
    }
}