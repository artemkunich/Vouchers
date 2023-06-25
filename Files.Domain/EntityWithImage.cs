using Akunich.Domain.Abstractions;

namespace Vouchers.Files.Domain;

public class EntityWithImage : AggregateRoot<Guid>
{
    public Guid IdentityId { get; init; }

    public static EntityWithImage Create(Guid id, Guid identityId) => 
        new()
        {
            Id = id,
            IdentityId = identityId,
        };
}