using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Commands.IdentityCommands;
using Vouchers.Application.Events.IdentityEvents;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Core;
using Vouchers.Entities;
using Vouchers.Files;
using Vouchers.Identities;

namespace Vouchers.Application.UseCases.IdentityCases
{
    internal sealed class UpdateIdentityCommandHandler : IHandler<UpdateIdentityCommand>
    {
        private readonly IAuthIdentityProvider _authIdentityProvider;
        private readonly IRepository<Identity,Guid> _identityRepository;
        private readonly IAppImageService _appImageService;
        private readonly IMessageFactory _messageFactory;

        public UpdateIdentityCommandHandler(IAuthIdentityProvider authIdentityProvider, 
            IRepository<Identity,Guid> identityRepository, IAppImageService appImageService, IMessageFactory messageFactory)
        {
            _authIdentityProvider = authIdentityProvider;
            _identityRepository = identityRepository;
            _appImageService = appImageService;
            _messageFactory = messageFactory;
        }

        public async Task HandleAsync(UpdateIdentityCommand command, CancellationToken cancellation)
        {
            var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
            var identity = await _identityRepository.GetByIdAsync(authIdentityId);
            if (identity is null)
                throw new ApplicationException(Properties.Resources.IdentityDoesNotExist);

            var requireUpdate = false;
            var identityUpdatedEvent = new IdentityUpdatedEvent();
            
            if (command.Image is not null && command.CropParameters is not null)
            {
                var imageStream = command.Image.OpenReadStream();
                var image = await _appImageService.CreateCroppedImageAsync(imageStream, command.CropParameters);
                identity.ImageId = image.Id;
                
                identityUpdatedEvent.NewImageId = image.ImageId;
                identityUpdatedEvent.NewCroppedImageId = image.Id;
                identityUpdatedEvent.NewCropParameters = command.CropParameters;
                
                requireUpdate = true;
            }

            if (command.Image is null && identity.ImageId is not null && command.CropParameters is not null)
            {
                var image = await _appImageService.CreateCroppedImageAsync(identity.ImageId.Value, command.CropParameters);
                identity.ImageId = image.Id;

                identityUpdatedEvent.NewCroppedImageId = image.Id;
                identityUpdatedEvent.NewCropParameters = command.CropParameters;
                
                requireUpdate = true;
            }

            if (identity.FirstName != command.FirstName)
            {
                identity.FirstName = command.FirstName;
                identityUpdatedEvent.NewFirstName = identity.FirstName;
                
                requireUpdate = true;
            }

            if (identity.LastName != command.LastName)
            {
                identity.LastName = command.LastName;
                identityUpdatedEvent.NewLastName = identity.LastName;
                
                requireUpdate = true;
            }

            if (identity.Email != command.Email)
            {
                identity.Email = command.Email;
                identityUpdatedEvent.NewEmail = identity.Email;
                
                requireUpdate = true;
            }

            if (requireUpdate)
            {
                identityUpdatedEvent.EventId = Guid.NewGuid();
                var outboxMessage = await _messageFactory.CreateOutboxAsync(identityUpdatedEvent);
                await _identityRepository.UpdateAsync(identity, new []{outboxMessage});
            }
                
        }
    }
}