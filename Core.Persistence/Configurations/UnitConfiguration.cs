using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Vouchers.Core.Domain;

namespace Vouchers.Core.Persistence.Configurations;

internal class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.ToTable(nameof(Unit));

        builder.HasKey(unit => unit.Id);

        builder.HasIndex(unit => new {unit.UnitTypeId, unit.ValidFrom, unit.ValidTo, unit.CanBeExchanged}).IsUnique();
          
        builder
            .Property(unit => unit.Supply)
            .IsRequired()
            .HasPrecision(18, 2);

        builder
            .Property(unit => unit.ValidFrom)
            .IsRequired();

        builder
            .Property(unit => unit.ValidTo)
            .IsRequired();

        builder
            .Property(unit => unit.CanBeExchanged)
            .IsRequired();

        builder
            .HasOne(unit => unit.UnitType)
            .WithMany()
            .IsRequired()
            .HasForeignKey(unit => unit.UnitTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Navigation(unit => unit.UnitType)
            .AutoInclude();
        
        builder.Property<byte[]>("RowVersion").IsRowVersion();
            
    }
}