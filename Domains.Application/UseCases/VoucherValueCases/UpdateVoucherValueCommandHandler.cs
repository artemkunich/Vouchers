using System;
using System.Threading.Tasks;
using System.Threading;
using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identity.Abstractions;
using Vouchers.Domains.Application.Errors;
using Vouchers.Domains.Domain;

namespace Vouchers.Domains.Application.UseCases.VoucherValueCases;

internal sealed class UpdateVoucherValueCommandHandler : IRequestHandler<UpdateVoucherValueCommand,Unit>
{
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;
    private readonly IRepository<VoucherValue,Guid> _voucherValueRepository;
    private readonly IRepository<CroppedImage, Guid> _croppedRepository;
    private readonly IAppImageService _appImageService;

    public UpdateVoucherValueCommandHandler(
        IIdentityIdProvider<Guid> identityIdProvider,
        IAppImageService appImageService,
        IRepository<VoucherValue,Guid> voucherValueRepository, 
        IRepository<CroppedImage, Guid> croppedRepository)
    {
        _identityIdProvider = identityIdProvider;
        _voucherValueRepository = voucherValueRepository;
        _croppedRepository = croppedRepository;
        _appImageService = appImageService;
    }

    public async Task<Result<Unit>> HandleAsync(
        UpdateVoucherValueCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.CurrentIdentityId;

        var value = await _voucherValueRepository.GetByIdAsync(command.Id, cancellation);
        if (value is null)
            return new VoucherValueDoesNotExistError();

        if (value.IssuerIdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        var requireUpdate = false;

        if (command.Image is not null && command.CropParameters is not null)
        {
            var imageStream = command.Image.OpenReadStream();
            var croppedImage = await _appImageService.CreateCroppedImageAsync(imageStream, command.CropParameters);
            await _croppedRepository.AddAsync(croppedImage, cancellation);
            
            value.ImageId = croppedImage.Id;
            requireUpdate = true;
        }
                
        if (command.Image is null && value.ImageId is not null && command.CropParameters is not null)
        {
            var croppedImage = await _croppedRepository.GetByIdAsync(value.ImageId.Value, cancellation);
            if (croppedImage is null)
                return new ImageDoesNotExistError();
            
            var newCroppedImage = await _appImageService.CreateCroppedImageAsync(croppedImage, command.CropParameters);
            await _croppedRepository.AddAsync(newCroppedImage, cancellation);
            
            value.ImageId = newCroppedImage.Id;
            requireUpdate = true;
        }

        if (command.Ticker is not null && value.Ticker != command.Ticker)
        {
            value.Ticker = command.Ticker;
            requireUpdate = true;
        }

        if (value.Description != command.Description)
        { 
            value.Description = command.Description;
            requireUpdate = true;
        }

        if (requireUpdate)
            await _voucherValueRepository.UpdateAsync(value, cancellation);
        
        return Unit.Value;
    }

}