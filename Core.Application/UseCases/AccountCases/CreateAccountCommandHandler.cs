using Akunich.Application.Abstractions;
using Akunich.Domain.Abstractions;
using Akunich.Extensions.Time;
using Vouchers.Core.Domain;
using Unit = Akunich.Application.Abstractions.Unit;

namespace Vouchers.Core.Application.UseCases.AccountCases;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand,Unit>
{
    private readonly ITimeProvider _timeProvider;
    private readonly IRepository<Account,Guid> _accountRepository;

    public CreateAccountCommandHandler(ITimeProvider timeProvider, IRepository<Account, Guid> accountRepository)
    {
        _timeProvider = timeProvider;
        _accountRepository = accountRepository;
    }

    public async Task<Result<Unit>> HandleAsync(CreateAccountCommand command, CancellationToken cancellation)
    {
        var account = Account.Create(command.AccountId, command.IdentityId, _timeProvider.GetUtcNow());
        if (command.IsConfirmed)
            account.IsActive = true;
        
        await _accountRepository.AddAsync(account, cancellation);

        return Unit.Value;
    }
}