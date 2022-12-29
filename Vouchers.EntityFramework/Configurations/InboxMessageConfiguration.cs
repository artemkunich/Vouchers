using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;
using Vouchers.Domains;
using Vouchers.Entities;
using Vouchers.InterCommunication;

namespace Vouchers.EntityFramework.Configurations
{
    internal class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
    {
        public void Configure(EntityTypeBuilder<InboxMessage> builder)
        {
            builder.ToTable(nameof(InboxMessage));

            builder.HasKey(x => x.Id);
            
            
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.OriginalId).IsRequired();
            builder.Property(x => x.Handler).IsRequired();
            builder.Property(x => x.Data);
            builder.Property(x => x.ReceivedDateTime);

            builder.HasIndex(x => new { x.OriginalId, x.Handler });
            
            builder.Property<byte[]>("RowVersion").IsRowVersion();
            
        }
    }
}
