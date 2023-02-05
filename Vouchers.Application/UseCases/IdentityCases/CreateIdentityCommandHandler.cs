using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Commands.IdentityCommands;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Core.Domain;
using Vouchers.Files.Domain;
using Vouchers.Identities.Domain;

namespace Vouchers.Application.UseCases.IdentityCases;

internal sealed class CreateIdentityCommandHandler : IHandler<CreateIdentityCommand, Result<Guid>>
{
    private readonly ILoginNameProvider _loginNameProvider;
    private readonly IRepository<Login,Guid> _loginRepository;
    private readonly IAppImageService _appImageService;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    public CreateIdentityCommandHandler(ILoginNameProvider loginNameProvider, IRepository<Login,Guid> loginRepository, IAppImageService appImageService, IIdentifierProvider<Guid> identifierProvider)
    {
        _loginNameProvider = loginNameProvider;
        _loginRepository = loginRepository;
        _appImageService = appImageService;
        _identifierProvider = identifierProvider;
    }

    public async Task<Result<Guid>> HandleAsync(CreateIdentityCommand command, CancellationToken cancellation)
    {
        var loginName = _loginNameProvider.CurrentLoginName;
        CroppedImage image = null;
        if (command.Image is not null && command.CropParameters is not null)
        {
            var imageStream = command.Image.OpenReadStream();
            image = await _appImageService.CreateCroppedImageAsync(imageStream, command.CropParameters);
        }

        var identityId = _identifierProvider.CreateNewId();
        var identity = Identity.Create(identityId, command.Email, command.FirstName, command.LastName);
        
        var loginId = _identifierProvider.CreateNewId();
        var login = Login.Create(loginId, loginName, identity);
        identity.ImageId = image?.Id;

        await _loginRepository.AddAsync(login);

        return identity.Id;
    }
}