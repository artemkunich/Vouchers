using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vouchers.Files.Domain;

namespace Vouchers.Files.Persistence.Configurations;

internal class EntityWithImageConfiguration : IEntityTypeConfiguration<EntityWithImage>
{
    public void Configure(EntityTypeBuilder<EntityWithImage> builder)
    {
        builder.ToTable(nameof(EntityWithImage));

        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.Id).IsRequired().ValueGeneratedNever();

        builder
            .HasOne(entity => entity.CurrentImage)
            .WithMany().HasForeignKey(item => item.CurrentImageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property<byte[]>("RowVersion").IsRowVersion();
    }
}