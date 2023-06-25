using System;
using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identifier;
using Akunich.Extensions.Identity.Abstractions;
using Vouchers.Domains.Domain;

namespace Vouchers.Domains.Application.UseCases.IdentityCases;

internal sealed class CreateIdentityCommandHandler : IRequestHandler<CreateIdentityCommand, Dtos.IdDto<Guid>>
{
    private readonly IIdentityIdProvider<string> _loginNameProvider;
    private readonly IRepository<CroppedImage, Guid> _croppedRepository;
    private readonly IRepository<Identity, Guid> _identityRepository;
    private readonly IAppImageService _appImageService;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    public CreateIdentityCommandHandler(
        IIdentityIdProvider<string> loginNameProvider,
        IAppImageService appImageService, 
        IIdentifierProvider<Guid> identifierProvider, 
        IRepository<CroppedImage, Guid> croppedRepository, 
        IRepository<Identity, Guid> identityRepository)
    {
        _loginNameProvider = loginNameProvider;
        _appImageService = appImageService;
        _identifierProvider = identifierProvider;
        _croppedRepository = croppedRepository;
        _identityRepository = identityRepository;
    }

    public async Task<Result<Dtos.IdDto<Guid>>> HandleAsync(CreateIdentityCommand command, CancellationToken cancellation)
    {
        var email = _loginNameProvider.GetIdentityId();
        CroppedImage croppedImage = null;
        if (command.Image is not null && command.CropParameters is not null)
        {
            var imageStream = command.Image.OpenReadStream();
            croppedImage = await _appImageService.CreateCroppedImageAsync(imageStream, command.CropParameters);
            await _croppedRepository.AddAsync(croppedImage, cancellation);
        }

        var identityId = _identifierProvider.CreateNewId();
        var identity = Identity.Create(identityId, email, command.FirstName, command.LastName);
        
        identity.ImageId = croppedImage?.Id;

        await _identityRepository.AddAsync(identity, cancellation);

        return new Dtos.IdDto<Guid>(identity.Id);
    }
}