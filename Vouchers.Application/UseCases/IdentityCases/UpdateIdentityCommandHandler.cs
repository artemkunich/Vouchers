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
    internal sealed class UpdateIdentityCommandHandler : IAuthIdentityHandler<UpdateIdentityCommand>
    {
        private readonly IRepository<Identity> _identityRepository;
        private readonly AppImageService _appImageService;

        public UpdateIdentityCommandHandler(IRepository<Identity> identityRepository, AppImageService appImageService)
        {
            _identityRepository = identityRepository;
            _appImageService = appImageService;
        }

        public async Task HandleAsync(UpdateIdentityCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var identity = await _identityRepository.GetByIdAsync(authIdentityId);
            if (identity is null)
                throw new ApplicationException("Identity does not exist");

            var identityDetailDto = command.IdentityDetail;

            if (identityDetailDto.Image is not null && identityDetailDto.CropParameters is not null)
            {
                var imageStream = identityDetailDto.Image.OpenReadStream();
                var image = await _appImageService.CreateCroppedImage(imageStream, identityDetailDto.CropParameters);
                identity.ImageId = image.Id;
            }

            if (identityDetailDto.Image is null && identity.ImageId is not null && identityDetailDto.CropParameters is not null)
            {
                var image = await _appImageService.CreateCroppedImage(identity.ImageId.Value, identityDetailDto.CropParameters);
                identity.ImageId = image.Id;
            }

            identity.FirstName = identityDetailDto.FirstName;
            identity.LastName = identityDetailDto.LastName;
            identity.Email = identityDetailDto.Email;  

            _identityRepository.Update(identity);
        }
    }
}