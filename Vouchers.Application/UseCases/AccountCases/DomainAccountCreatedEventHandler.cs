using System;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Abstractions;
using Vouchers.Application.DomainEvents;
using Vouchers.Application.Infrastructure;
using Vouchers.Core.Domain;
using Unit = Vouchers.Application.Abstractions.Unit;

namespace Vouchers.Application.UseCases.AccountCases;

public class DomainAccountCreatedEventHandler : IDomainEventHandler<DomainAccountCreatedEvent>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IRepository<Account,Guid> _accountRepository;

    public DomainAccountCreatedEventHandler(IDateTimeProvider dateTimeProvider, IRepository<Account, Guid> accountRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _accountRepository = accountRepository;
    }

    public async Task<Result<Unit>> HandleAsync(DomainAccountCreatedEvent domainEvent, CancellationToken cancellation)
    {
        var account = Account.Create(domainEvent.DomainAccountId, _dateTimeProvider.CurrentDateTime());
        await _accountRepository.AddAsync(account);

        return Unit.Value;
    }
}