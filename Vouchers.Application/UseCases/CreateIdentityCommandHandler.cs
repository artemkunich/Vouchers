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
    public class CreateIdentityCommandHandler : IHandler<CreateIdentityCommand>
    {
        private readonly ILoginRepository loginRepository;

        public CreateIdentityCommandHandler(ILoginRepository loginRepository)
        {
            this.loginRepository = loginRepository;
        }

        public async Task HandleAsync(CreateIdentityCommand command, CancellationToken cancellation)
        {
            var identity = Identity.Create();

            var login = Login.Create(command.LoginName, identity);     
            var identityDetail = IdentityDetail.Create(login.Identity, command.IdentityDetailDto.IdentityName, command.IdentityDetailDto.Email);
            identityDetail.FirstName = command.IdentityDetailDto?.FirstName;
            identityDetail.LastName = command.IdentityDetailDto?.LastName;

            await loginRepository.AddAsync(login, identityDetail);
            await loginRepository.SaveAsync();
        }
    }
}
