using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Domains;

namespace Vouchers.EntityFramework.Configurations
{
    public class DomainContractConfiguration : IEntityTypeConfiguration<DomainContract>
    {
        public void Configure(EntityTypeBuilder<DomainContract> builder)
        {
            builder.ToTable(nameof(DomainContract));

            builder.HasKey(domainContract => domainContract.Id);

            builder.Property<Guid>("DomainId").HasColumnName("DomainId").IsRequired();
            builder.HasIndex("DomainId").IsUnique();
            builder.HasOne(domainContract => domainContract.Domain).WithMany().HasForeignKey("DomainId").OnDelete(DeleteBehavior.Restrict);

            builder.Property<Guid>("OfferId").HasColumnName("OfferId").IsRequired();
            builder.HasIndex("OfferId").IsUnique();
            builder.HasOne(domainContract => domainContract.Offer).WithMany().HasForeignKey("OfferId").OnDelete(DeleteBehavior.Restrict);

            builder.Property<Guid>("PartyId").HasColumnName("PartyId").IsRequired();
            builder.HasIndex("PartyId").IsUnique();
            builder.HasOne(domain => domain.Party).WithMany().HasForeignKey("PartyId").OnDelete(DeleteBehavior.Restrict);

            builder.Property(contract => contract.ContractNumber);
            builder.Property(contract => contract.DomainName);
            builder.Property(contract => contract.CreatedDate);

            builder.Property<byte[]>("RowVersion").IsRowVersion();
        }
            
    }
}
