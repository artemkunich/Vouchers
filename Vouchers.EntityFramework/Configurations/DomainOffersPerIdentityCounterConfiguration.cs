using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vouchers.Domains;

namespace Vouchers.EntityFramework.Configurations
{
    internal class DomainOffersPerIdentityCounterConfiguration : IEntityTypeConfiguration<DomainOffersPerIdentityCounter>
    {
        public void Configure(EntityTypeBuilder<DomainOffersPerIdentityCounter> builder)
        {
            builder.ToTable(nameof(DomainOffersPerIdentityCounter));

            builder.HasKey(counter => counter.Id);

            builder
                .Property(counter => counter.OfferId)
                .IsRequired();
            builder
                .HasIndex(counter => counter.OfferId);
            builder
                .HasOne(contract => contract.Offer)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .Property(counter => counter.IdentityId)
                .IsRequired();
            builder
                .HasIndex(counter => counter.IdentityId);


            builder.Property(counter => counter.Counter);

            builder.HasIndex(counter => new { counter.OfferId, counter.IdentityId });

            builder.Property<byte[]>("RowVersion").IsRowVersion();
          
            builder.Ignore(x => x.OutboxEvents);
        }
    }
}
