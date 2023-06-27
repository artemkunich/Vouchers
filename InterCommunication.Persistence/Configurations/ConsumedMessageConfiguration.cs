using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;
using Vouchers.Primitives;
using Vouchers.InterCommunication;
using Vouchers.Persistence.InterCommunication;

namespace Vouchers.Persistence.Configurations;

internal class ConsumedMessageConfiguration : IEntityTypeConfiguration<ConsumedMessage>
{
    public void Configure(EntityTypeBuilder<ConsumedMessage> builder)
    {
        builder.ToTable(nameof(ConsumedMessage));

        builder.HasKey(x => x.Id);
            
            
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.MessageId).IsRequired();
        builder.Property(x => x.Consumer).IsRequired();
        builder.Property(x => x.ConsumedDate);

        builder.HasIndex(x => new {x.MessageId, x.Consumer }).IsUnique();
            
        builder.Property<byte[]>("RowVersion").IsRowVersion();
            
    }
}