using Akunich.Domain.Abstractions;

namespace Vouchers.Files.Domain;

public sealed class Image : AggregateRoot<Guid>
{
    public Guid CroppedImageId { get; private set; }
    public CropParameters CropParameters { get; private set; }
    public Guid EntityId { get; init; }
    public EntityWithImage Entity { get; init; }
    
    public static Image Create(Guid id, Guid coppedImageId, CropParameters cropParameters, EntityWithImage entity) => new()
    {
        Id = id,
        CroppedImageId = coppedImageId,
        CropParameters = cropParameters,
        EntityId = entity.Id,
        Entity = entity
    };

    public void SetCrop(Guid croppedImageId, CropParameters cropParameters)
    {
        CroppedImageId = croppedImageId;
        CropParameters = cropParameters;
    }

}