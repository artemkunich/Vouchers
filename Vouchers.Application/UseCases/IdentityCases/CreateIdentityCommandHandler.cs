using System;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Commands.IdentityCommands;
using Vouchers.Application.Dtos;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Files.Domain;
using Vouchers.Identities.Domain;

namespace Vouchers.Application.UseCases.IdentityCases;

internal sealed class CreateIdentityCommandHandler : IHandler<CreateIdentityCommand, IdDto<Guid>>
{
    private readonly ILoginNameProvider _loginNameProvider;
    private readonly IRepository<Login,Guid> _loginRepository;
    private readonly IRepository<CroppedImage, Guid> _croppedRepository;
    private readonly IAppImageService _appImageService;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    public CreateIdentityCommandHandler(ILoginNameProvider loginNameProvider, IRepository<Login,Guid> loginRepository, IAppImageService appImageService, IIdentifierProvider<Guid> identifierProvider, IRepository<CroppedImage, Guid> croppedRepository)
    {
        _loginNameProvider = loginNameProvider;
        _loginRepository = loginRepository;
        _appImageService = appImageService;
        _identifierProvider = identifierProvider;
        _croppedRepository = croppedRepository;
    }

    public async Task<Result<IdDto<Guid>>> HandleAsync(CreateIdentityCommand command, CancellationToken cancellation)
    {
        var loginName = _loginNameProvider.CurrentLoginName;
        CroppedImage croppedImage = null;
        if (command.Image is not null && command.CropParameters is not null)
        {
            var imageStream = command.Image.OpenReadStream();
            croppedImage = await _appImageService.CreateCroppedImageAsync(imageStream, command.CropParameters);
            await _croppedRepository.AddAsync(croppedImage);
        }

        var identityId = _identifierProvider.CreateNewId();
        var identity = Identity.Create(identityId, command.Email, command.FirstName, command.LastName);
        
        var loginId = _identifierProvider.CreateNewId();
        var login = Login.Create(loginId, loginName, identity);
        identity.ImageId = croppedImage?.Id;

        await _loginRepository.AddAsync(login);

        return new IdDto<Guid>(identity.Id);
    }
}