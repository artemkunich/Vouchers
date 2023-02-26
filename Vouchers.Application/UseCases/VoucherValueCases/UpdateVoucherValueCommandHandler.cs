using System;
using Vouchers.Application.Commands.VoucherValueCommands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Application.Abstractions;
using Vouchers.Application.Errors;
using Vouchers.Values.Domain;
using Vouchers.Application.Services;
using Vouchers.Files.Domain;

namespace Vouchers.Application.UseCases.VoucherValueCases;

internal sealed class UpdateVoucherValueCommandHandler : IHandler<UpdateVoucherValueCommand,Unit>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IRepository<VoucherValue,Guid> _voucherValueRepository;
    private readonly IRepository<CroppedImage, Guid> _croppedRepository;
    private readonly IAppImageService _appImageService;

    public UpdateVoucherValueCommandHandler(IAuthIdentityProvider authIdentityProvider, IAppImageService appImageService,
        IRepository<VoucherValue,Guid> voucherValueRepository, IRepository<CroppedImage, Guid> croppedRepository)
    {
        _authIdentityProvider = authIdentityProvider;
        _voucherValueRepository = voucherValueRepository;
        _croppedRepository = croppedRepository;
        _appImageService = appImageService;
    }

    public async Task<Result<Unit>> HandleAsync(UpdateVoucherValueCommand command, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var value = await _voucherValueRepository.GetByIdAsync(command.Id);
        if (value is null)
            return new VoucherValueDoesNotExistError();

        if (value.IssuerIdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        var requireUpdate = false;

        if (command.Image is not null && command.CropParameters is not null)
        {
            var imageStream = command.Image.OpenReadStream();
            var croppedImage = await _appImageService.CreateCroppedImageAsync(imageStream, command.CropParameters);
            await _croppedRepository.AddAsync(croppedImage);
            
            value.ImageId = croppedImage.Id;
            requireUpdate = true;
        }
                
        if (command.Image is null && value.ImageId is not null && command.CropParameters is not null)
        {
            var croppedImage = await _croppedRepository.GetByIdAsync(value.ImageId.Value);
            if (croppedImage is null)
                return new ImageDoesNotExistError();
            
            var newCroppedImage = await _appImageService.CreateCroppedImageAsync(croppedImage, command.CropParameters);
            await _croppedRepository.AddAsync(newCroppedImage);
            
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
            await _voucherValueRepository.UpdateAsync(value);
        
        return Unit.Value;
    }

}