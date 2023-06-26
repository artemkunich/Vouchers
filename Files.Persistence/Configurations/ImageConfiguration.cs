using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vouchers.Files.Domain;

namespace Vouchers.Files.Persistence.Configurations;

internal class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ToTable(nameof(Image));

        builder.HasKey(image => image.Id);
        builder.Property(image => image.Id).IsRequired().ValueGeneratedNever();

        builder.Property(image => image.EntityId).IsRequired();

        builder
            .HasOne(image => image.Entity)
            .WithMany().HasForeignKey(image => image.EntityId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .OwnsOne(image => image.CropParameters)
            .Property(crop => crop.Width)
            .HasPrecision(18, 15);

        builder
            .OwnsOne(image => image.CropParameters)
            .Property(crop => crop.Height)
            .HasPrecision(18, 15);

        builder
            .OwnsOne(image => image.CropParameters)
            .Property(crop => crop.X)
            .HasPrecision(18, 15);

        builder
            .OwnsOne(image => image.CropParameters)
            .Property(crop => crop.Y)
            .HasPrecision(18, 15);

        builder.Property<byte[]>("RowVersion").IsRowVersion();
    }
}