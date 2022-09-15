using Vouchers.Entities;

namespace Vouchers.Files
{
    public class AppImage : Entity
    {
        public byte[] CroppedContent { get; set; }

        public CropParameters CropParameters { get; set; }

        private AppImage() { }
        public AppImage(Guid id, byte[] croppedContent, CropParameters cropParameters) : base(id)
        {
            CroppedContent = croppedContent;
            CropParameters = cropParameters;
        }

        public static AppImage Create(byte[] croppedContent, CropParameters cropParameters) =>
            new AppImage(Guid.NewGuid(), croppedContent, cropParameters);
        
    }
}