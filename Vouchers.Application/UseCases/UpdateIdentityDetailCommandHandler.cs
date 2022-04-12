using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Infrastructure;
using Vouchers.Core;
using Vouchers.Identities;

namespace Vouchers.Application.UseCases
{
    public class UpdateIdentityDetailCommandHandler : IAuthIdentityHandler<UpdateIdentityDetailCommand>
    {
        private readonly IIdentityDetailRepository identityDetailRepository;

        public UpdateIdentityDetailCommandHandler(IIdentityDetailRepository identityDetailRepository)
        {
            this.identityDetailRepository = identityDetailRepository;
        }

        public async Task HandleAsync(UpdateIdentityDetailCommand command, Guid authIdentityId, CancellationToken cancellation)
        {
            var identityDetail = await identityDetailRepository.GetByIdentityIdAsync(authIdentityId);
            if (identityDetail is null)
                throw new ApplicationException("Identity's detail does not exist");

            var identityDetailDto = command.IdentityDetailDto;

            identityDetail.IdentityName = identityDetailDto.IdentityName;
            identityDetail.FirstName = identityDetailDto.FirstName;
            identityDetail.LastName = identityDetailDto.LastName;
            identityDetail.Email = identityDetailDto.Email;

            identityDetailRepository.Update(identityDetail);
            await identityDetailRepository.SaveAsync();
        }
    }
}
