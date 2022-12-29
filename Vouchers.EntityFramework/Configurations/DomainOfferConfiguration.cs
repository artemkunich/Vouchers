using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;
using Vouchers.Domains;

namespace Vouchers.EntityFramework.Configurations
{
    internal class DomainOfferConfiguration : IEntityTypeConfiguration<DomainOffer>
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
            builder.Property(offer => offer.MaxMembersCount);

            builder.Property(offer => offer.MaxContractsPerIdentity);

            builder.Property(offer => offer.RecipientId);

            builder.Property<byte[]>("RowVersion").IsRowVersion();
            
        }
    }
}
