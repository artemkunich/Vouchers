using System;
using System.Threading.Tasks;
using System.Threading;
using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Identifier;
using Akunich.Extensions.Identity.Abstractions;
using Vouchers.Domains.Domain;
using Vouchers.Domains.Application.DomainEvents;
using Vouchers.Domains.Application.Errors;

namespace Vouchers.Domains.Application.UseCases.VoucherValueCases;

internal sealed class CreateVoucherValueCommandHandler : IRequestHandler<CreateVoucherValueCommand, Dtos.IdDto<Guid>>
{
    private readonly IIdentityIdProvider<Guid> _identityIdProvider;
    private readonly IReadOnlyRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IRepository<VoucherValue,Guid> _voucherValueRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly INotificationDispatcher _notificationDispatcher;
    
    public CreateVoucherValueCommandHandler(
        IIdentityIdProvider<Guid> identityIdProvider,
        IReadOnlyRepository<DomainAccount,Guid> domainAccountRepository,
        IRepository<VoucherValue,Guid> voucherValueRepository, 
        IIdentifierProvider<Guid> identifierProvider,
        INotificationDispatcher notificationDispatcher)
    {
        _identityIdProvider = identityIdProvider;
        _domainAccountRepository = domainAccountRepository;
        _voucherValueRepository = voucherValueRepository;
        _identifierProvider = identifierProvider;
        _notificationDispatcher = notificationDispatcher;
    }

    public async Task<Result<Dtos.IdDto<Guid>>> HandleAsync(CreateVoucherValueCommand command, CancellationToken cancellation)
    {
        var authIdentityId = _identityIdProvider.GetIdentityId();

        var issuerDomainAccount = await _domainAccountRepository.GetByIdAsync(command.IssuerAccountId, cancellation);
        if (issuerDomainAccount?.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();
        if (!issuerDomainAccount.IsConfirmed)
            return new IssuerDomainAccountIsNotActivatedError();
        if (!issuerDomainAccount.IsIssuer)
            return new IssuerOperationsAreNotAllowedError();

        var valueId = _identifierProvider.CreateNewId();
        var value = VoucherValue.Create(valueId, issuerDomainAccount.DomainId, issuerDomainAccount.IdentityId, command.Ticker);
        value.Description = command.Description;

        await _voucherValueRepository.AddAsync(value, cancellation);

        var voucherValueCreated = new VoucherValueCreatedNotification(value.Id, issuerDomainAccount.Id, value.DomainId, value.IssuerIdentityId, value.Ticker, value.Description);
        var result = await _notificationDispatcher.DispatchAsync(voucherValueCreated, cancellation);
        if (result.IsFailure)
            return result.Errors;
        
        return new Dtos.IdDto<Guid>(value.Id);
    }
}