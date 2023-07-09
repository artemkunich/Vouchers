using System;
using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identity.Abstractions;
using Vouchers.Domains.Application.DomainEvents;
using Vouchers.Domains.Application.Errors;
using Vouchers.Domains.Domain;

namespace Vouchers.Domains.Application.UseCases.IdentityCases;

internal sealed class UpdateIdentityCommandHandler : IRequestHandler<UpdateIdentityCommand,Unit>
{
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;
    private readonly IRepository<Identity,Guid> _identityRepository;
    private readonly INotificationDispatcher _notificationDispatcher;
    
    public UpdateIdentityCommandHandler(
        IIdentityIdProvider<Guid> identityIdProvider, 
        IRepository<Identity,Guid> identityRepository,
        INotificationDispatcher notificationDispatcher)
    {
        _identityIdProvider = identityIdProvider;
        _identityRepository = identityRepository;
        _notificationDispatcher = notificationDispatcher;
    }

    public async Task<Result<Unit>> HandleAsync(UpdateIdentityCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.GetIdentityId();

        var identity = await _identityRepository.GetByIdAsync(authIdentityId, cancellation);
        if (identity is null)
            return new IdentityDoesNotExistError();

        var isChanged = false;
        var identityUpdatedEvent = new IdentityUpdatedNotification();

        if (identity.FirstName != command.FirstName)
        {
            identity.FirstName = command.FirstName;
            identityUpdatedEvent.NewFirstName = identity.FirstName;
                
            isChanged = true;
        }

        if (identity.LastName != command.LastName)
        {
            identity.LastName = command.LastName;
            identityUpdatedEvent.NewLastName = identity.LastName;
                
            isChanged = true;
        }

        if (identity.Email != command.Email)
        {
            identity.Email = command.Email;
            identityUpdatedEvent.NewEmail = identity.Email;
                
            isChanged = true;
        }

        if (isChanged)
        {
            await _identityRepository.UpdateAsync(identity, cancellation);
            var result = await _notificationDispatcher.DispatchAsync(identityUpdatedEvent, cancellation);
            if (result.IsFailure)
                return result.Errors;
        }
        
        return Unit.Value;
                
    }
}