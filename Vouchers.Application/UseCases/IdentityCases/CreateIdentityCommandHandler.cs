using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Commands.IdentityCommands;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Core;
using Vouchers.Files;
using Vouchers.Identities;

namespace Vouchers.Application.UseCases.IdentityCases
{
    internal sealed class CreateIdentityCommandHandler : IHandler<CreateIdentityCommand>
    {
        private readonly IRepository<Login> _loginRepository;
        private readonly AppImageService _appImageService;

        public CreateIdentityCommandHandler(IRepository<Login> loginRepository, AppImageService appImageService)
        {
            _loginRepository = loginRepository;
            _appImageService = appImageService;
        }

        public async Task HandleAsync(CreateIdentityCommand command, CancellationToken cancellation)
        {
            var identityDetailDto = command.IdentityDetail;
            
            CroppedImage image = null;
            if (identityDetailDto.Image is not null && identityDetailDto.CropParameters is not null)
            {
                var imageStream = identityDetailDto.Image.OpenReadStream();
                image = await _appImageService.CreateCroppedImage(imageStream, identityDetailDto.CropParameters);
            }

            var identity = Identity.Create(identityDetailDto.Email, identityDetailDto.FirstName, identityDetailDto.LastName);
            var login = Login.Create(command.LoginName, identity);
            identity.ImageId = image?.Id;

            await _loginRepository.AddAsync(login);
        }
    }
}
