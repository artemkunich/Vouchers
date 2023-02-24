using System;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Commands.IdentityCommands;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Files.Domain;
using Vouchers.Identities.Domain;
using Vouchers.Identities.Domain.DomainEvents;

namespace Vouchers.Application.UseCases.IdentityCases;

internal sealed class UpdateIdentityCommandHandler : IHandler<UpdateIdentityCommand,Unit>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IRepository<Identity,Guid> _identityRepository;
    private readonly IRepository<CroppedImage, Guid> _croppedRepository;
    private readonly IAppImageService _appImageService;
    private readonly ICultureInfoProvider _cultureInfoProvider;
    
    public UpdateIdentityCommandHandler(IAuthIdentityProvider authIdentityProvider, 
        IRepository<Identity,Guid> identityRepository, IAppImageService appImageService, ICultureInfoProvider cultureInfoProvider, IRepository<CroppedImage, Guid> croppedRepository)
    {
        _authIdentityProvider = authIdentityProvider;
        _identityRepository = identityRepository;
        _appImageService = appImageService;
        _cultureInfoProvider = cultureInfoProvider;
        _croppedRepository = croppedRepository;
    }

    public async Task<Result<Unit>> HandleAsync(UpdateIdentityCommand command, CancellationToken cancellation)
    {
        var cultureInfo = _cultureInfoProvider.GetCultureInfo();
        
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var identity = await _identityRepository.GetByIdAsync(authIdentityId);
        if (identity is null)
            return Error.IdentityDoesNotExist(cultureInfo);

        var isChanged = false;
        var identityUpdatedDomainEvent = new IdentityUpdatedDomainEvent();
            
        if (command.Image is not null && command.CropParameters is not null)
        {
            var imageStream = command.Image.OpenReadStream();
            var croppedImage = await _appImageService.CreateCroppedImageAsync(imageStream, command.CropParameters);
            await _croppedRepository.AddAsync(croppedImage);
            identity.ImageId = croppedImage.Id;
            identityUpdatedDomainEvent.NewImageId = croppedImage.Id;

            isChanged = true;
        }

        if (command.Image is null && identity.ImageId is not null && command.CropParameters is not null)
        {
            var croppedImage = await _croppedRepository.GetByIdAsync(identity.ImageId.Value);
            if (croppedImage is null)
                return Error.ImageDoesNotExist();
            
            var newCroppedImage = await _appImageService.CreateCroppedImageAsync(croppedImage, command.CropParameters);
            await _croppedRepository.AddAsync(newCroppedImage);
            
            identity.ImageId = newCroppedImage.Id;

            identityUpdatedDomainEvent.NewImageId = newCroppedImage.Id;

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
        
        return Unit.Value;
                
    }
}