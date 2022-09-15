using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Commands;
using Vouchers.Application.Infrastructure;
using Vouchers.Core;
using Vouchers.Files;
using Vouchers.Identities;

namespace Vouchers.Application.UseCases
{
    public class UpdateIdentityCommandHandler : IAuthIdentityHandler<UpdateIdentityCommand>
    {
        private readonly IRepository<Identity> _identityRepository;
        private readonly IRepository<AppImage> _appImageRepository;
        private readonly IImageService _imageService;

        public UpdateIdentityCommandHandler(IRepository<Identity> identityRepository, IRepository<AppImage> appImageRepository, IImageService imageService)
        {
            _identityRepository = identityRepository;
            _appImageRepository = appImageRepository;
            _imageService = imageService;
        }

        public async Task HandleAsync(UpdateIdentityCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var identity = await _identityRepository.GetByIdAsync(authIdentityId);
            if (identity is null)
                throw new ApplicationException("Identity does not exist");

            var identityDetailDto = command.IdentityDetail;

            if (identityDetailDto.CropParameters is not null)
            {
                Stream imageStream = null;

                if (identityDetailDto.Image is not null)
                    imageStream = identityDetailDto.Image.OpenReadStream();

                if (imageStream is null && identity.ImageId is not null)
                    imageStream = _imageService.GetImageStream(identity.ImageId.Value);

                if (imageStream is not null)
                {
                    var cropParametersDto = identityDetailDto.CropParameters;
                    var croppedContent = await _imageService.CropImageAsync(imageStream, cropParametersDto);
                    var cropParameters = CropParameters.Create(cropParametersDto.X, cropParametersDto.Y, cropParametersDto.Width, cropParametersDto.Height);

                    if (identity.ImageId is null)
                    {
                        var image = AppImage.Create(croppedContent, cropParameters);
                        await _appImageRepository.AddAsync(image);

                        identity.ImageId = image.Id;
                    }
                    else
                    {
                        var image = await _appImageRepository.GetByIdAsync(identity.ImageId.Value);
                        image.CroppedContent = croppedContent;
                        image.CropParameters = cropParameters;
                        await _appImageRepository.UpdateAsync(image);
                    }

                    if (identityDetailDto.Image is not null)
                        await _imageService.SaveImageAsync(identityDetailDto.Image.OpenReadStream(), identity.ImageId.Value);
                }
            }

            identity.FirstName = identityDetailDto.FirstName;
            identity.LastName = identityDetailDto.LastName;
            identity.Email = identityDetailDto.Email;  

            _identityRepository.Update(identity);
        }
    }
}