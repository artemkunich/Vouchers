using Akunich.Domain.Abstractions;

namespace Vouchers.Files.Domain;

public class EntityWithImage : AggregateRoot<Guid>
{
    public Guid IdentityId { get; init; }

    public Guid? CurrentImageId { get; private set; }
    
    public Image CurrentImage { get; private set; }
    
    public static EntityWithImage Create(Guid id, Guid identityId) => 
        new()
        {
            Id = id,
            IdentityId = identityId,
        };

    public void SetImage(Image image)
    {
        CurrentImageId = image.Id;
        CurrentImage = image;
    }
}