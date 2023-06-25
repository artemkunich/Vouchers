using System;
using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identifier;
using Akunich.Extensions.Identity.Abstractions;
using Vouchers.Domains.Domain;

namespace Vouchers.Domains.Application.UseCases.IdentityCases;

internal sealed class CreateIdentityCommandHandler : IRequestHandler<CreateIdentityCommand, Dtos.IdDto<Guid>>
{
    private readonly IIdentityIdProvider<string> _loginNameProvider;
    private readonly IRepository<Identity, Guid> _identityRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    public CreateIdentityCommandHandler(
        IIdentityIdProvider<string> loginNameProvider,
        IIdentifierProvider<Guid> identifierProvider,
        IRepository<Identity, Guid> identityRepository)
    {
        _loginNameProvider = loginNameProvider;
        _identifierProvider = identifierProvider;
        _identityRepository = identityRepository;
    }

    public async Task<Result<Dtos.IdDto<Guid>>> HandleAsync(CreateIdentityCommand command, CancellationToken cancellation)
    {
        var email = _loginNameProvider.GetIdentityId();

        var identityId = _identifierProvider.CreateNewId();
        var identity = Identity.Create(identityId, email, command.FirstName, command.LastName);

        await _identityRepository.AddAsync(identity, cancellation);

        return new Dtos.IdDto<Guid>(identity.Id);
    }
}