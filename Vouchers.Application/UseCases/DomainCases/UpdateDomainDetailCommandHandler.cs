﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Commands.DomainCommands;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Domains;

namespace Vouchers.Application.UseCases.DomainCases
{
    internal sealed class UpdateDomainDetailCommandHandler : IHandler<UpdateDomainDetailCommand>
    {
        private readonly IAuthIdentityProvider _authIdentityProvider;
        private readonly IRepository<DomainAccount> _domainAccountRepository;
        private readonly IRepository<Domain> _domainRepository;
        private readonly IAppImageService _appImageService;
        public UpdateDomainDetailCommandHandler(IAuthIdentityProvider authIdentityProvider, IRepository<DomainAccount> domainAccountRepository, IRepository<Domain> domainRepository, IAppImageService appImageService)
        {
            _authIdentityProvider = authIdentityProvider;
            _domainAccountRepository = domainAccountRepository;
            _domainRepository = domainRepository;
            _appImageService = appImageService;
        }

        public async Task HandleAsync(UpdateDomainDetailCommand command, CancellationToken cancellation)
        {
            var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

            var domainAccount = (await _domainAccountRepository.GetByExpressionAsync(account => account.DomainId == command.DomainId && account.IdentityId == authIdentityId)).FirstOrDefault();
            if (domainAccount is null)
                throw new ApplicationException(Properties.Resources.DomainAccountDoesNotExist);

            if (!domainAccount.IsAdmin && domainAccount.Domain.Contract.PartyId != authIdentityId)
                throw new ApplicationException(Properties.Resources.OperationIsNotAllowed);

            var requireUpdate = false;
            var domain = domainAccount.Domain;

            if (command.Image is not null && command.CropParameters is not null)
            {
                var imageStream = command.Image.OpenReadStream();
                var image = await _appImageService.CreateCroppedImage(imageStream, command.CropParameters);
                domain.ImageId = image.Id;
                requireUpdate = true;
            }

            if (command.Image is null && domain.ImageId is not null && command.CropParameters is not null)
            {
                var image = await _appImageService.CreateCroppedImage(domain.ImageId.Value, command.CropParameters);
                domain.ImageId = image.Id;
                requireUpdate = true;
            }

            if (domain.Description != command.Description)
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
                _domainRepository.Update(domain);
        }
    }
}