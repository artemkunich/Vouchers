using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Commands.IdentityCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Core;
using Vouchers.Files;
using Vouchers.Identities;

namespace Vouchers.Application.UseCases.IdentityCases
{
    internal sealed class UpdateIdentityCommandHandler : IHandler<UpdateIdentityCommand>
    {
        private readonly IAuthIdentityProvider _authIdentityProvider;
        private readonly IRepository<Identity> _identityRepository;
        private readonly IAppImageService _appImageService;

        public UpdateIdentityCommandHandler(IAuthIdentityProvider authIdentityProvider, IRepository<Identity> identityRepository, IAppImageService appImageService)
        {
            _authIdentityProvider = authIdentityProvider;
            _identityRepository = identityRepository;
            _appImageService = appImageService;
        }

        public async Task HandleAsync(UpdateIdentityCommand command, CancellationToken cancellation)
        {
            var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
            var identity = await _identityRepository.GetByIdAsync(authIdentityId);
            if (identity is null)
                throw new ApplicationException(Properties.Resources.IdentityDoesNotExist);

            var requireUpdate = false;

            if (command.Image is not null && command.CropParameters is not null)
            {
                var imageStream = command.Image.OpenReadStream();
                var image = await _appImageService.CreateCroppedImage(imageStream, command.CropParameters);
                identity.ImageId = image.Id;
                requireUpdate = true;
            }

            if (command.Image is null && identity.ImageId is not null && command.CropParameters is not null)
            {
                var image = await _appImageService.CreateCroppedImage(identity.ImageId.Value, command.CropParameters);
                identity.ImageId = image.Id;
                requireUpdate = true;
            }

            if (identity.FirstName != command.FirstName)
            {
                identity.FirstName = command.FirstName;
                requireUpdate = true;
            }

            if (identity.LastName != command.LastName)
            {
                identity.LastName = command.LastName;
                requireUpdate = true;
            }

            if (identity.Email != command.Email)
            {
                identity.Email = command.Email;
                requireUpdate = true;
            }

            if(requireUpdate)
                await _identityRepository.UpdateAsync(identity);
        }
    }
}