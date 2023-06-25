using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identity.Abstractions;
using Vouchers.Files.Application.Dtos;
using Vouchers.Files.Application.Errors;
using Vouchers.Files.Application.Services;
using Vouchers.Files.Domain;

namespace Vouchers.Files.Application.UseCases.ImageCases;

public class CreateImageCommandHandler : IRequestHandler<CreateImageCommand, IdDto<Guid>>
{
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;
    private readonly IAppImageService _appImageService;

    private readonly IRepository<Image,Guid> _imageRepository;
    private readonly IReadOnlyRepository<EntityWithImage,Guid> _entityWithImageRepository;

    public CreateImageCommandHandler(
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

    public async Task<Result<IdDto<Guid>>> HandleAsync(CreateImageCommand command, CancellationToken cancellation)
    {
        var identityId = _identityIdProvider.GetIdentityId();
        var entitiesWithImage = (await _entityWithImageRepository.GetByExpressionAsync(x => x.Id == command.SubjectId && x.IdentityId == identityId)).ToArray();
        if (!entitiesWithImage.Any())
            return new EntityDoesNotExistError();

        var entityWithImage = entitiesWithImage.First();
        var imageStream = command.Image.OpenReadStream();
        var image = await _appImageService.CreateCroppedImageAsync(imageStream, command.CropParameters, entityWithImage);
        await _imageRepository.AddAsync(image, cancellation);
        
        return new IdDto<Guid>(image.Id);
    }
}