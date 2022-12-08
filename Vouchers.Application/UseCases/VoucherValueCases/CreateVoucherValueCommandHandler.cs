﻿using System;
using System.Collections.Generic;
using System.Text;
using Vouchers.Core;
using Vouchers.Application.Commands.VoucherValueCommands;
using Vouchers.Application.Infrastructure;
using Vouchers.Values;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Files;
using Vouchers.Domains;
using Vouchers.Application.Services;

namespace Vouchers.Application.UseCases.VoucherValueCases
{
    internal sealed class CreateVoucherValueCommandHandler : IHandler<CreateVoucherValueCommand, Guid>
    {
        private readonly IAuthIdentityProvider _authIdentityProvider;
        private readonly IRepository<DomainAccount> _domainAccountRepository;
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<VoucherValue> _voucherValueRepository;
        private readonly IRepository<UnitType> _unitTypeRepository;
        private readonly IAppImageService _appImageService;

        public CreateVoucherValueCommandHandler(IAuthIdentityProvider authIdentityProvider, IRepository<DomainAccount> domainAccountRepository, IRepository<Account> accountRepository,
            IRepository<VoucherValue> voucherValueRepository, IRepository<UnitType> unitTypeRepository, IAppImageService appImageService)
        {
            _authIdentityProvider = authIdentityProvider;
            _domainAccountRepository = domainAccountRepository;
            _accountRepository = accountRepository;
            _voucherValueRepository = voucherValueRepository;
            _unitTypeRepository = unitTypeRepository;
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
                image = await _appImageService.CreateCroppedImage(imageStream, command.CropParameters);
            }

            var account = await _accountRepository.GetByIdAsync(command.IssuerAccountId);

            var unitType = UnitType.Create(account);
            await _unitTypeRepository.AddAsync(unitType);

            var value = VoucherValue.Create(unitType.Id, issuerDomainAccount.Domain.Id, issuerDomainAccount.IdentityId, command.Ticker);
            value.Description = command.Description;
            value.ImageId = image?.Id;


            await _voucherValueRepository.AddAsync(value);

            return value.Id;
        }
    }
}