using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core.Domain;
using Vouchers.Application.Commands.VoucherValueCommands;
using Vouchers.Application.Infrastructure;
using Vouchers.Values.Domain;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Files.Domain;
using Vouchers.Domains.Domain;
using Vouchers.Application.Services;

namespace Vouchers.Application.UseCases.VoucherValueCases;

internal sealed class CreateVoucherValueCommandHandler : IHandler<CreateVoucherValueCommand, Guid>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IReadOnlyRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IReadOnlyRepository<Account,Guid> _accountRepository;
    private readonly IRepository<VoucherValue,Guid> _voucherValueRepository;
    private readonly IRepository<UnitType,Guid> _unitTypeRepository;
    private readonly IAppImageService _appImageService;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    
    public CreateVoucherValueCommandHandler(IAuthIdentityProvider authIdentityProvider, IAppImageService appImageService,
        IReadOnlyRepository<DomainAccount,Guid> domainAccountRepository, IReadOnlyRepository<Account,Guid> accountRepository,
        IRepository<VoucherValue,Guid> voucherValueRepository, IRepository<UnitType,Guid> unitTypeRepository, IIdentifierProvider<Guid> identifierProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _domainAccountRepository = domainAccountRepository;
        _accountRepository = accountRepository;
        _voucherValueRepository = voucherValueRepository;
        _unitTypeRepository = unitTypeRepository;
        _identifierProvider = identifierProvider;
        _appImageService = appImageService;
    }

    public async Task<Guid> HandleAsync(CreateVoucherValueCommand command, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var issuerDomainAccount = await _domainAccountRepository.GetByIdAsync(command.IssuerAccountId);
        if (issuerDomainAccount?.IdentityId != authIdentityId)
            throw new ApplicationException(Properties.Resources.OperationIsNotAllowed);
        if (!issuerDomainAccount.IsConfirmed)
            throw new ApplicationException(Properties.Resources.IssuerAccountIsNotActivated);
        if (!issuerDomainAccount.IsIssuer)
            throw new ApplicationException(Properties.Resources.IssuerOperationsAreNotAllowed);

        CroppedImage image = null;
        if (command.Image is not null && command.CropParameters is not null)
        {
            var imageStream = command.Image.OpenReadStream();
            image = await _appImageService.CreateCroppedImageAsync(imageStream, command.CropParameters);
        }

        var account = await _accountRepository.GetByIdAsync(command.IssuerAccountId);
        
        var unitTypeId = _identifierProvider.CreateNewId();
        var unitType = UnitType.Create(unitTypeId, account);
        await _unitTypeRepository.AddAsync(unitType);

        var value = VoucherValue.Create(unitType.Id, issuerDomainAccount.Domain.Id, issuerDomainAccount.IdentityId, command.Ticker);
        value.Description = command.Description;
        value.ImageId = image?.Id;


        await _voucherValueRepository.AddAsync(value);

        return value.Id;
    }
}