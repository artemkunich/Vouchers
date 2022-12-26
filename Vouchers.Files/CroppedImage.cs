using Vouchers.Entities;

namespace Vouchers.Files
{
    public sealed class CroppedImage : Entity<Guid>
    {
        public Guid ImageId { get; }
        public CropParameters CropParameters { get; set; }

        private CroppedImage() { }
        private CroppedImage(Guid id, Guid imageId, CropParameters cropParameters) : base(id)
        {
            ImageId = imageId;
            CropParameters = cropParameters;
        }

        public static CroppedImage Create(Guid id, Guid imageId, CropParameters cropParameters) =>
            new CroppedImage(id, imageId, cropParameters);
        
    }
}