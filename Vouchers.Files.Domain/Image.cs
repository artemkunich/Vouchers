using Vouchers.Primitives;

namespace Vouchers.Files.Domain;

public class Image : Entity<Guid>
{
    public Guid SubjectId { get; }

    public Guid IdentityId { get; }

    public DateTime UploadDateTime { get; }
}