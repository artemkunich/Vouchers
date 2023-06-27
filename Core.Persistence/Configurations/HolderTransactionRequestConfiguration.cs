using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vouchers.Core.Domain;

namespace Vouchers.Core.Persistence.Configurations;

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
            .Navigation(request => request.CreditorAccount)
            .AutoInclude();

        builder
            .HasOne(request => request.DebtorAccount)
            .WithMany().HasForeignKey(transaction => transaction.DebtorAccountId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Navigation(request => request.DebtorAccount)
            .AutoInclude();
        
        builder.Property(request => request.TransactionId).IsRequired(false);
        builder.HasIndex(request => request.TransactionId).IsUnique();
        builder
            .HasOne(request => request.Transaction)
            .WithMany().HasForeignKey(request => request.TransactionId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Navigation(request => request.Transaction)
            .AutoInclude();
        
        builder.Property(request => request.DueDate);

        builder.Property(request => request.Message);

    }
}