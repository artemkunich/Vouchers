using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Domains;

namespace Vouchers.EntityFramework.Configurations
{
    public class DomainDetailConfiguration : IEntityTypeConfiguration<DomainDetail>
    {
        public void Configure(EntityTypeBuilder<DomainDetail> builder)
        {
            builder.ToTable(nameof(DomainDetail));

            builder.HasKey(domainDetail => domainDetail.Id);

            builder.Property<Guid>("DomainId").HasColumnName("DomainId").IsRequired();
            builder.HasIndex("DomainId").IsUnique();
            builder.HasOne(domainDetail => domainDetail.Contract).WithMany().HasForeignKey("ContractId").OnDelete(DeleteBehavior.Restrict);

            builder.Property<byte[]>("RowVersion").IsRowVersion();
        }
            
    }
}
