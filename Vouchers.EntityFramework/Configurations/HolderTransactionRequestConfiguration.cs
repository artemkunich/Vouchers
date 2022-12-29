using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Core;

namespace Vouchers.EntityFramework.Configurations
{
    internal class HolderTransactionRequestConfiguration : IEntityTypeConfiguration<HolderTransactionRequest>
    {
        public void Configure(EntityTypeBuilder<HolderTransactionRequest> builder)
        {
            builder.ToTable(nameof(HolderTransactionRequest));

            builder.HasKey(request => request.Id);

            builder
                .OwnsOne(request => request.Quantity)
                .Property(quantity => quantity.Amount)
                .HasPrecision(18, 2);

            builder
                .OwnsOne(request => request.Quantity)
                .HasOne(quantity => quantity.UnitType)
                .WithMany().HasForeignKey(quantity => quantity.UnitTypeId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Property(request => request.CreditorAccountId).IsRequired(false);
            builder
                .HasOne(request => request.CreditorAccount)
                .WithMany().HasForeignKey(request => request.CreditorAccountId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder
                .HasOne(request => request.DebtorAccount)
                .WithMany().HasForeignKey(transaction => transaction.DebtorAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(request => request.TransactionId).IsRequired(false);
            builder.HasIndex(request => request.TransactionId).IsUnique();
            builder
                .HasOne(request => request.Transaction)
                .WithMany().HasForeignKey(request => request.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(request => request.DueDate);

            builder.Property(request => request.Message);

        }
    }
}
