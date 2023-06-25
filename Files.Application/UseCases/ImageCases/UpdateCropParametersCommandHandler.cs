using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identity.Abstractions;
using Vouchers.Files.Application.Errors;
using Vouchers.Files.Application.Services;
using Vouchers.Files.Domain;

namespace Vouchers.Files.Application.UseCases.ImageCases;

public class UpdateCropParametersCommandHandler : IRequestHandler<UpdateCropParametersCommand,Unit>
{
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;
    private readonly IAppImageService _appImageService;

    private readonly IRepository<Image,Guid> _imageRepository;
    private readonly IReadOnlyRepository<EntityWithImage,Guid> _entityWithImageRepository;

    public UpdateCropParametersCommandHandler(
        IIdentityIdProvider<Guid> identityIdProvider, 
        IAppImageService appImageService, 
        IRepository<Image, Guid> imageRepository, 
        IReadOnlyRepository<EntityWithImage, Guid> entityWithImageRepository)
    {
        _identityIdProvider = identityIdProvider;
        _appImageService = appImageService;
        _imageRepository = imageRepository;
        _entityWithImageRepository = entityWithImageRepository;
    }

    public async Task<Result<Unit>> HandleAsync(UpdateCropParametersCommand command, CancellationToken cancellation)
    {
        var identityId = _identityIdProvider.GetIdentityId();
        var images = (await _imageRepository.GetByExpressionAsync(x => x.Id == command.ImageId && x.Entity.IdentityId == identityId, cancellation)).ToArray();

        if (!images.Any())
            return new ImageDoesNotExistError();

        var image = images.First();
        
        await _appImageService.UpdateCropParametersAsync(image, command.CropParameters);
        await _imageRepository.UpdateAsync(image, cancellation);

        return Unit.Value;
        
    }
}