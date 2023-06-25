using System;
using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identity.Abstractions;
using Vouchers.Domains.Application.DomainEvents;
using Vouchers.Domains.Application.Errors;
using Vouchers.Domains.Domain;

namespace Vouchers.Domains.Application.UseCases.IdentityCases;

internal sealed class UpdateIdentityCommandHandler : IRequestHandler<UpdateIdentityCommand,Unit>
{
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;
    private readonly IRepository<Identity,Guid> _identityRepository;
    private readonly IRepository<CroppedImage, Guid> _croppedRepository;
    private readonly IAppImageService _appImageService;
    private readonly INotificationDispatcher _notificationDispatcher;
    
    public UpdateIdentityCommandHandler(
        IIdentityIdProvider<Guid> identityIdProvider, 
        IRepository<Identity,Guid> identityRepository, 
        IAppImageService appImageService, 
        IRepository<CroppedImage, Guid> croppedRepository, 
        INotificationDispatcher notificationDispatcher)
    {
        _identityIdProvider = identityIdProvider;
        _identityRepository = identityRepository;
        _appImageService = appImageService;
        _croppedRepository = croppedRepository;
        _notificationDispatcher = notificationDispatcher;
    }

    public async Task<Result<Unit>> HandleAsync(UpdateIdentityCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.GetIdentityId();

        var identity = await _identityRepository.GetByIdAsync(authIdentityId, cancellation);
        if (identity is null)
            return new IdentityDoesNotExistError();

        var isChanged = false;
        var identityUpdatedEvent = new IdentityUpdatedNotification();
            
        if (command.Image is not null && command.CropParameters is not null)
        {
            var imageStream = command.Image.OpenReadStream();
            var croppedImage = await _appImageService.CreateCroppedImageAsync(imageStream, command.CropParameters);
            await _croppedRepository.AddAsync(croppedImage, cancellation);
            identity.ImageId = croppedImage.Id;
            identityUpdatedEvent.NewImageId = croppedImage.Id;

            isChanged = true;
        }

        if (command.Image is null && identity.ImageId is not null && command.CropParameters is not null)
        {
            var croppedImage = await _croppedRepository.GetByIdAsync(identity.ImageId.Value, cancellation);
            if (croppedImage is null)
                return new ImageDoesNotExistError();
            
            var newCroppedImage = await _appImageService.CreateCroppedImageAsync(croppedImage, command.CropParameters);
            await _croppedRepository.AddAsync(newCroppedImage, cancellation);
            
            identity.ImageId = newCroppedImage.Id;

            identityUpdatedEvent.NewImageId = newCroppedImage.Id;

            isChanged = true;
        }

        if (identity.FirstName != command.FirstName)
        {
            identity.FirstName = command.FirstName;
            identityUpdatedEvent.NewFirstName = identity.FirstName;
                
            isChanged = true;
        }

        if (identity.LastName != command.LastName)
        {
            identity.LastName = command.LastName;
            identityUpdatedEvent.NewLastName = identity.LastName;
                
            isChanged = true;
        }

        if (identity.Email != command.Email)
        {
            identity.Email = command.Email;
            identityUpdatedEvent.NewEmail = identity.Email;
                
            isChanged = true;
        }

        if (isChanged)
        {
            await _identityRepository.UpdateAsync(identity, cancellation);
            var result = await _notificationDispatcher.DispatchAsync(identityUpdatedEvent, cancellation);
            if (result.IsFailure)
                return result;
        }
        
        return Unit.Value;
                
    }
}