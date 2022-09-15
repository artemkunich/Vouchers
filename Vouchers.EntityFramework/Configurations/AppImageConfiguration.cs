using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

using Vouchers.Files;

namespace Vouchers.EntityFramework.Configurations
{
    public class AppImageConfiguration : IEntityTypeConfiguration<AppImage>
    {
        public void Configure(EntityTypeBuilder<AppImage> builder)
        {
            builder.ToTable(nameof(AppImage));

            builder.HasKey(image => image.Id);
            builder.Property(image => image.Id).IsRequired().ValueGeneratedNever();

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
}
