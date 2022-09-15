using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Core;
using Vouchers.Files;
using Vouchers.Identities;

namespace Vouchers.Application.UseCases
{
    public class CreateIdentityCommandHandler : IHandler<CreateIdentityCommand>
    {
        private readonly IRepository<Login> _loginRepository;
        private readonly IRepository<AppImage> _appImageRepository;
        private readonly IImageService _imageService;

        public CreateIdentityCommandHandler(IRepository<Login> loginRepository, IRepository<AppImage> appImageRepository, IImageService imageService)
        {
            _loginRepository = loginRepository;
            _imageService = imageService;
            _appImageRepository = appImageRepository;
        }

        public async Task HandleAsync(CreateIdentityCommand command, CancellationToken cancellation)
        {
            var identityDetailDto = command.IdentityDetail;
            

            AppImage image = null;
            if (identityDetailDto.CropParameters is not null)
            {
                var cropParametersDto = identityDetailDto.CropParameters;

                var croppedContent = await _imageService.CropImageAsync(identityDetailDto.Image.OpenReadStream(), cropParametersDto);
                var cropParameters = CropParameters.Create(cropParametersDto.X, cropParametersDto.Y, cropParametersDto.Width, cropParametersDto.Height);
                image = AppImage.Create(croppedContent, cropParameters);
                await _appImageRepository.AddAsync(image);

                await _imageService.SaveImageAsync(identityDetailDto.Image.OpenReadStream(), image.Id);
            }

            var identity = Identity.Create(identityDetailDto.Email, identityDetailDto.FirstName, identityDetailDto.LastName);
            var login = Login.Create(command.LoginName, identity);
            identity.ImageId = image?.Id;

            await _loginRepository.AddAsync(login);
        }
    }
}
