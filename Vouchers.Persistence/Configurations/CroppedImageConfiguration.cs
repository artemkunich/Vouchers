using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Files;

namespace Vouchers.Persistence.Configurations;

internal class CroppedImageConfiguration : IEntityTypeConfiguration<CroppedImage>
{
    public void Configure(EntityTypeBuilder<CroppedImage> builder)
    {
        builder.ToTable(nameof(CroppedImage));

        builder.HasKey(image => image.Id);
        builder.Property(image => image.Id).IsRequired().ValueGeneratedNever();

        builder.Property(image => image.ImageId).IsRequired();

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