using System;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Common.Application.Abstractions;
using Vouchers.Files.Domain;
using Vouchers.Domains.Domain;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Domains.Application.DomainEvents;
using Vouchers.Domains.Application.Errors;
using Vouchers.Domains.Application.Services;

namespace Vouchers.Domains.Application.UseCases.VoucherValueCases;

internal sealed class CreateVoucherValueCommandHandler : IRequestHandler<CreateVoucherValueCommand, Dtos.IdDto<Guid>>
{
    private readonly IIdentityIdProvider _identityIdProvider;
    private readonly IReadOnlyRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IRepository<VoucherValue,Guid> _voucherValueRepository;
    private readonly IRepository<CroppedImage, Guid> _croppedRepository;
    private readonly IAppImageService _appImageService;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    public CreateVoucherValueCommandHandler(IIdentityIdProvider identityIdProvider, IAppImageService appImageService,
        IReadOnlyRepository<DomainAccount,Guid> domainAccountRepository,
        IRepository<VoucherValue,Guid> voucherValueRepository, IIdentifierProvider<Guid> identifierProvider, IRepository<CroppedImage, Guid> croppedRepository, IDomainEventDispatcher domainEventDispatcher)
    {
        _identityIdProvider = identityIdProvider;
        _domainAccountRepository = domainAccountRepository;
        _voucherValueRepository = voucherValueRepository;
        _identifierProvider = identifierProvider;
        _croppedRepository = croppedRepository;
        _domainEventDispatcher = domainEventDispatcher;
        _appImageService = appImageService;
    }

    public async Task<Result<Dtos.IdDto<Guid>>> HandleAsync(CreateVoucherValueCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.CurrentIdentityId;

        var issuerDomainAccount = await _domainAccountRepository.GetByIdAsync(command.IssuerAccountId);
        if (issuerDomainAccount?.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();
        if (!issuerDomainAccount.IsConfirmed)
            return new IssuerDomainAccountIsNotActivatedError();
        if (!issuerDomainAccount.IsIssuer)
            return new IssuerOperationsAreNotAllowedError();

        CroppedImage croppedImage = null;
        if (command.Image is not null && command.CropParameters is not null)
        {
            var imageStream = command.Image.OpenReadStream();
            croppedImage = await _appImageService.CreateCroppedImageAsync(imageStream, command.CropParameters);
            
            await _croppedRepository.AddAsync(croppedImage);
        }
        
        var valueId = _identifierProvider.CreateNewId();
        var value = VoucherValue.Create(valueId, issuerDomainAccount.DomainId, issuerDomainAccount.IdentityId, command.Ticker);
        value.Description = command.Description;
        value.ImageId = croppedImage?.Id;
        
        await _voucherValueRepository.AddAsync(value);

        var voucherValueCreated = new VoucherValueCreatedDomainEvent(value.Id, issuerDomainAccount.Id, value.DomainId, value.IssuerIdentityId, value.Ticker, value.Description, value.ImageId);
        var result = await _domainEventDispatcher.DispatchAsync(voucherValueCreated, cancellation);
        if (result.IsFailure)
            return result.Errors;
        
        return new Dtos.IdDto<Guid>(value.Id);
    }
}