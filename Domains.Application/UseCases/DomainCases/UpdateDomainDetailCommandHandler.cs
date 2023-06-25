using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identity.Abstractions;
using Vouchers.Domains.Application.Errors;
using Vouchers.Domains.Domain;

namespace Vouchers.Domains.Application.UseCases.DomainCases;

internal sealed class UpdateDomainDetailCommandHandler : IRequestHandler<UpdateDomainDetailCommand,Unit>
{
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;
    private readonly IReadOnlyRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IRepository<Domain.Domain, Guid> _domainRepository;
    private readonly IRepository<CroppedImage, Guid> _croppedRepository;
    private readonly IAppImageService _appImageService;
    public UpdateDomainDetailCommandHandler(
        IIdentityIdProvider<Guid> identityIdProvider, 
        IReadOnlyRepository<DomainAccount,Guid> domainAccountRepository, 
        IRepository<Domain.Domain,Guid> domainRepository, 
        IAppImageService appImageService, 
        IRepository<CroppedImage, Guid> croppedRepository)
    {
        _identityIdProvider = identityIdProvider;
        _domainAccountRepository = domainAccountRepository;
        _domainRepository = domainRepository;
        _appImageService = appImageService;
        _croppedRepository = croppedRepository;
    }

    public async Task<Result<Unit>> HandleAsync(UpdateDomainDetailCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.GetIdentityId();

        var domainAccount = (await _domainAccountRepository.GetByExpressionAsync(account => account.DomainId == command.DomainId && account.IdentityId == authIdentityId)).FirstOrDefault();
        if (domainAccount is null)
            return new DomainAccountDoesNotExistError();

        if (!domainAccount.IsAdmin && domainAccount.Domain.Contract.PartyId != authIdentityId)
            return new OperationIsNotAllowedError();

        var requireUpdate = false;
        var domain = domainAccount.Domain;

        if (command.Image is not null && command.CropParameters is not null)
        {
            var imageStream = command.Image.OpenReadStream();
            var newCroppedImage = await _appImageService.CreateCroppedImageAsync(imageStream, command.CropParameters);
            await _croppedRepository.AddAsync(newCroppedImage, cancellation);
            
            domain.ImageId = newCroppedImage.Id;
            requireUpdate = true;
        }

        if (command.Image is null && domain.ImageId is not null && command.CropParameters is not null)
        {
            var croppedImage = await _croppedRepository.GetByIdAsync(domain.ImageId.Value, cancellation);
            if (croppedImage is null)
                return new ImageDoesNotExistError();
            
            var newCroppedImage = await _appImageService.CreateCroppedImageAsync(croppedImage, command.CropParameters);
            await _croppedRepository.AddAsync(newCroppedImage, cancellation);
            
            domain.ImageId = newCroppedImage.Id;
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
            await _domainRepository.UpdateAsync(domain, cancellation);
        
        return Unit.Value;
    }
}