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
using Vouchers.Primitives;
using Vouchers.Files;
using Vouchers.Identities;
using Vouchers.Identities.DomainEvents;

namespace Vouchers.Application.UseCases.IdentityCases;

internal sealed class UpdateIdentityCommandHandler : IHandler<UpdateIdentityCommand>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IRepository<Identity,Guid> _identityRepository;
    private readonly IAppImageService _appImageService;

    public UpdateIdentityCommandHandler(IAuthIdentityProvider authIdentityProvider, 
        IRepository<Identity,Guid> identityRepository, IAppImageService appImageService)
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

        var isChanged = false;
        var identityUpdatedDomainEvent = new IdentityUpdatedDomainEvent();
            
        if (command.Image is not null && command.CropParameters is not null)
        {
            var imageStream = command.Image.OpenReadStream();
            var image = await _appImageService.CreateCroppedImageAsync(imageStream, command.CropParameters);
            identity.ImageId = image.Id;
                
            identityUpdatedDomainEvent.NewImageId = image.Id;

            isChanged = true;
        }

        if (command.Image is null && identity.ImageId is not null && command.CropParameters is not null)
        {
            var image = await _appImageService.CreateCroppedImageAsync(identity.ImageId.Value, command.CropParameters);
            identity.ImageId = image.Id;

            identityUpdatedDomainEvent.NewImageId = image.Id;

            isChanged = true;
        }

        if (identity.FirstName != command.FirstName)
        {
            identity.FirstName = command.FirstName;
            identityUpdatedDomainEvent.NewFirstName = identity.FirstName;
                
            isChanged = true;
        }

        if (identity.LastName != command.LastName)
        {
            identity.LastName = command.LastName;
            identityUpdatedDomainEvent.NewLastName = identity.LastName;
                
            isChanged = true;
        }

        if (identity.Email != command.Email)
        {
            identity.Email = command.Email;
            identityUpdatedDomainEvent.NewEmail = identity.Email;
                
            isChanged = true;
        }

        if (isChanged)
        {
            identityUpdatedDomainEvent.Id = Guid.NewGuid();
            identity.RaiseDomainEvent(identityUpdatedDomainEvent);
            await _identityRepository.UpdateAsync(identity);
        }
                
    }
}