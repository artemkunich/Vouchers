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

namespace Vouchers.Application.UseCases.IdentityCases;

internal sealed class CreateIdentityCommandHandler : IHandler<CreateIdentityCommand, Guid>
{
    private readonly ILoginNameProvider _loginNameProvider;
    private readonly IRepository<Login,Guid> _loginRepository;
    private readonly IAppImageService _appImageService;
        
    public CreateIdentityCommandHandler(ILoginNameProvider loginNameProvider, IRepository<Login,Guid> loginRepository, IAppImageService appImageService)
    {
        _loginNameProvider = loginNameProvider;
        _loginRepository = loginRepository;
        _appImageService = appImageService;
    }

    public async Task<Guid> HandleAsync(CreateIdentityCommand command, CancellationToken cancellation)
    {
        var loginName = _loginNameProvider.CurrentLoginName;
        CroppedImage image = null;
        if (command.Image is not null && command.CropParameters is not null)
        {
            var imageStream = command.Image.OpenReadStream();
            image = await _appImageService.CreateCroppedImageAsync(imageStream, command.CropParameters);
        }

        var identity = Identity.Create(command.Email, command.FirstName, command.LastName);
        var login = Login.Create(loginName, identity);
        identity.ImageId = image?.Id;

        await _loginRepository.AddAsync(login);

        return identity.Id;
    }
}