using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Application.Commands.VoucherValueCommands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Vouchers.Files.Domain;
using Vouchers.Values.Domain;
using Vouchers.Application.Dtos;
using Vouchers.Application.Services;

namespace Vouchers.Application.UseCases.VoucherValueCases;

internal sealed class UpdateVoucherValueCommandHandler : IHandler<UpdateVoucherValueCommand,Result>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IRepository<VoucherValue,Guid> _voucherValueRepository;
    private readonly IAppImageService _appImageService;
    private readonly ICultureInfoProvider _cultureInfoProvider;
    
    public UpdateVoucherValueCommandHandler(IAuthIdentityProvider authIdentityProvider, IAppImageService appImageService,
        IRepository<VoucherValue,Guid> voucherValueRepository, ICultureInfoProvider cultureInfoProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _voucherValueRepository = voucherValueRepository;
        _cultureInfoProvider = cultureInfoProvider;
        _appImageService = appImageService;
    }

    public async Task<Result> HandleAsync(UpdateVoucherValueCommand command, CancellationToken cancellation)
    {
        var cultureInfo = _cultureInfoProvider.GetCultureInfo();
        
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        if (authIdentityId is null)
            return Error.NotRegistered(cultureInfo);

        var value = await _voucherValueRepository.GetByIdAsync(command.Id);
        if (value is null)
            return Error.VoucherValueDoesNotExist(cultureInfo);

        if (value.IssuerIdentityId != authIdentityId)
            return Error.OperationIsNotAllowed(cultureInfo);

        var requireUpdate = false;

        if (command.Image is not null && command.CropParameters is not null)
        {
            var imageStream = command.Image.OpenReadStream();
            var image = await _appImageService.CreateCroppedImageAsync(imageStream, command.CropParameters);
            value.ImageId = image.Id;
            requireUpdate = true;
        }
                
        if (command.Image is null && value.ImageId is not null && command.CropParameters is not null)
        {
            var image = await _appImageService.CreateCroppedImageAsync(value.ImageId.Value, command.CropParameters);
            value.ImageId = image.Id;
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
        
        return Result.Create();
    }

}