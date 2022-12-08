using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Domains;

namespace Vouchers.EntityFramework.Configurations
{
    internal class DomainContractConfiguration : IEntityTypeConfiguration<DomainContract>
    {
        public void Configure(EntityTypeBuilder<DomainContract> builder)
        {
            builder.ToTable(nameof(DomainContract));

            builder.HasKey(contract => contract.Id);

            builder
                .Property(contract => contract.OfferId)
                .IsRequired();
            builder
                .HasIndex(contract => contract.OfferId);
            builder
                .HasOne(contract => contract.Offer)
                .WithMany()
                .HasForeignKey(contract => contract.OfferId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(contract => contract.OffersPerIdentityCounterId)
                .IsUnique();
            builder
                .HasOne(contract => contract.OffersPerIdentityCounter)
                .WithMany()
                .HasForeignKey(contract => contract.OffersPerIdentityCounterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(contract => contract.PartyId)
                .IsRequired();
            builder
                .HasIndex(contract => contract.PartyId);


            builder.Property(contract => contract.DomainName);
            builder.HasIndex(contract => contract.DomainName).IsUnique();

            builder.Property(contract => contract.CreatedDate);

            builder.Property<byte[]>("RowVersion").IsRowVersion();
        }
            
    }
}
