using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Commands.DomainCommands;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Domains.Domain;

namespace Vouchers.Application.UseCases.DomainCases;

internal sealed class UpdateDomainDetailCommandHandler : IHandler<UpdateDomainDetailCommand, Result>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IReadOnlyRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IRepository<Domain,Guid> _domainRepository;
    private readonly IAppImageService _appImageService;
    private readonly ICultureInfoProvider _cultureInfoProvider;
    public UpdateDomainDetailCommandHandler(IAuthIdentityProvider authIdentityProvider, IReadOnlyRepository<DomainAccount,Guid> domainAccountRepository, 
        IRepository<Domain,Guid> domainRepository, IAppImageService appImageService, ICultureInfoProvider cultureInfoProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _domainAccountRepository = domainAccountRepository;
        _domainRepository = domainRepository;
        _appImageService = appImageService;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task<Result> HandleAsync(UpdateDomainDetailCommand command, CancellationToken cancellation)
    {
        var cultureInfo = _cultureInfoProvider.GetCultureInfo();
        
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        if (authIdentityId is null)
            return Error.NotRegistered(cultureInfo);
        
        var domainAccount = (await _domainAccountRepository.GetByExpressionAsync(account => account.DomainId == command.DomainId && account.IdentityId == authIdentityId)).FirstOrDefault();
        if (domainAccount is null)
            return Error.DomainAccountDoesNotExist(cultureInfo);

        if (!domainAccount.IsAdmin && domainAccount.Domain.Contract.PartyId != authIdentityId)
            return Error.OperationIsNotAllowed(cultureInfo);

        var requireUpdate = false;
        var domain = domainAccount.Domain;

        if (command.Image is not null && command.CropParameters is not null)
        {
            var imageStream = command.Image.OpenReadStream();
            var image = await _appImageService.CreateCroppedImageAsync(imageStream, command.CropParameters);
            domain.ImageId = image.Id;
            requireUpdate = true;
        }

        if (command.Image is null && domain.ImageId is not null && command.CropParameters is not null)
        {
            var image = await _appImageService.CreateCroppedImageAsync(domain.ImageId.Value, command.CropParameters);
            domain.ImageId = image.Id;
            requireUpdate = true;
        }

        if (command.Description is not null && domain.Description != command.Description)
        {
            domain.Description = command.Description;
            requireUpdate = true;
        }

        if (command.IsPublic is not null && domain.IsPublic != command.IsPublic)
        {
            domain.IsPublic = command.IsPublic.Value;
            requireUpdate = true;
        }

        if (requireUpdate)
            await _domainRepository.UpdateAsync(domain);
        
        return Result.Create();
    }
}