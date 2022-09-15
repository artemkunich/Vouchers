using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core;
using Vouchers.Application.Commands;
using Vouchers.Application.Infrastructure;
using Vouchers.Values;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Files;
using Vouchers.Domains;

namespace Vouchers.Application.UseCases
{
    public class CreateVoucherValueCommandHandler : IAuthIdentityHandler<CreateVoucherValueCommand, Guid>
    {
        private readonly IRepository<DomainAccount> _domainAccountRepository;
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<VoucherValue> _voucherValueRepository;
        private readonly IRepository<UnitType> _unitTypeRepository;
        private readonly IRepository<AppImage> _appImageRepository;
        private readonly IImageService _imageService;

        public CreateVoucherValueCommandHandler(IRepository<DomainAccount> domainAccountRepository, IRepository<Account> accountRepository,
            IRepository<VoucherValue> voucherValueRepository, IRepository<UnitType> unitTypeRepository, IImageService imageService, IRepository<AppImage> appImageRepository)
        {
            _domainAccountRepository = domainAccountRepository;
            _accountRepository = accountRepository;
            _voucherValueRepository = voucherValueRepository;
            _unitTypeRepository = unitTypeRepository;
            _imageService = imageService;
            _appImageRepository = appImageRepository;
        }

        public async Task<Guid> HandleAsync(CreateVoucherValueCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var issuerDomainAccount = await _domainAccountRepository.GetByIdAsync(command.IssuerDomainAccountId);
            if (issuerDomainAccount?.IdentityId != authIdentityId)
                throw new ApplicationException("Operation is not allowed");
            if (!issuerDomainAccount.IsConfirmed)
                throw new ApplicationException("Issuer account is not activated");
            if (!issuerDomainAccount.IsIssuer)
                throw new ApplicationException("Issue operations are not allowed");

            var detailDto = command.VoucherValueDetail;

            AppImage image = null;
            if (detailDto.CropParameters is not null)
            {
                var cropParametersDto = detailDto.CropParameters;

                var croppedContent = await _imageService.CropImageAsync(command.Image.OpenReadStream(), cropParametersDto);
                var cropParameters = CropParameters.Create(cropParametersDto.X, cropParametersDto.Y, cropParametersDto.Width, cropParametersDto.Height);
                image = AppImage.Create(croppedContent, cropParameters);
                await _appImageRepository.AddAsync(image);

                await _imageService.SaveImageAsync(command.Image.OpenReadStream(), image.Id);
            }

            var account = await _accountRepository.GetByIdAsync(command.IssuerDomainAccountId);

            var unitType = UnitType.Create(account);
            await _unitTypeRepository.AddAsync(unitType);

            var value = VoucherValue.Create(unitType.Id, issuerDomainAccount.Domain.Id, issuerDomainAccount.IdentityId, detailDto.Ticker);
            value.Description = detailDto.Description;
            value.ImageId = image?.Id;


            await _voucherValueRepository.AddAsync(value);

            return value.Id;
        }
    }
}
