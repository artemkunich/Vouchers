using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;
using Vouchers.Domains;

namespace Vouchers.EntityFramework.Configurations
{
    public class DomainOfferConfiguration : IEntityTypeConfiguration<DomainOffer>
    {
        public void Configure(EntityTypeBuilder<DomainOffer> builder)
        {
            builder.ToTable(nameof(DomainOffer));

            builder.HasKey(offer => offer.Id);

            builder.Property(offer => offer.Name);
            builder.Property(offer => offer.Description);
            builder.OwnsOne(offer => offer.Amount).Property(amount => amount.Amount).HasPrecision(18, 2).HasDefaultValue(0);
            builder.OwnsOne(offer => offer.Amount).Property(amount => amount.Currency);

            builder.Property(offer => offer.InvoicePeriod);
            builder.Property(offer => offer.MaxSubscribersCount);

            builder.Property<Guid>("RecipientId").HasColumnName("RecipientId");
            builder.HasOne(offer => offer.Recipient).WithMany().HasForeignKey("RecipientId").OnDelete(DeleteBehavior.Restrict);

            builder.Property<byte[]>("RowVersion").IsRowVersion();

        }
    }
}
