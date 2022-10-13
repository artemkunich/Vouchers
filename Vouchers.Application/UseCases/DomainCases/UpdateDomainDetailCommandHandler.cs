using System;
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
    internal sealed class UpdateDomainDetailCommandHandler : IAuthIdentityHandler<UpdateDomainDetailCommand>
    {
        private readonly IRepository<DomainAccount> _domainAccountRepository;
        private readonly IRepository<Domain> _domainRepository;
        private readonly AppImageService _appImageService;
        public UpdateDomainDetailCommandHandler(IRepository<DomainAccount> domainAccountRepository, IRepository<Domain> domainRepository, AppImageService appImageService)
        {
            _domainAccountRepository = domainAccountRepository;
            _domainRepository = domainRepository;
            _appImageService = appImageService;
        }

        public async Task HandleAsync(UpdateDomainDetailCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var domainAccount = (await _domainAccountRepository.GetByExpressionAsync(account => account.DomainId == command.DomainId && account.IdentityId == authIdentityId)).FirstOrDefault();
            if (domainAccount is null)
                throw new ApplicationException("Domain account does not exist");

            if (domainAccount.IsAdmin || domainAccount.Domain.Contract.PartyId != authIdentityId)
                throw new ApplicationException("Operation is not allowed");

            var requireUpdate = false;
            var domainDetailDto = command.DomainDetail;
            var domain = domainAccount.Domain;

            if (command.Image is not null && domainDetailDto.CropParameters is not null)
            {
                var imageStream = command.Image.OpenReadStream();
                var image = await _appImageService.CreateCroppedImage(imageStream, domainDetailDto.CropParameters);
                domain.ImageId = image.Id;
                requireUpdate = true;
            }

            if (command.Image is null && domain.ImageId is not null && domainDetailDto.CropParameters is not null)
            {
                var image = await _appImageService.CreateCroppedImage(domain.ImageId.Value, domainDetailDto.CropParameters);
                domain.ImageId = image.Id;
                requireUpdate = true;
            }

            if (domain.Description != domainDetailDto.Description)
            {
                domain.Description = domainDetailDto.Description;
                requireUpdate = true;
            }

            if (requireUpdate)
                _domainRepository.Update(domain);
        }
    }
}
