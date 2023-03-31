using Vouchers.Primitives;

namespace Vouchers.Files.Domain;

public sealed class CroppedImage : AggregateRoot<Guid>
{
    public Guid ImageId { get; init; }
    
    public Image Image { get; init; }
    
    public CropParameters CropParameters { get; set; }

    public static CroppedImage Create(Guid id, Image image, CropParameters cropParameters) => new()
    {
        Id = id,
        
        ImageId = image.Id,
        Image = image,
        
        CropParameters = cropParameters,
    };

}