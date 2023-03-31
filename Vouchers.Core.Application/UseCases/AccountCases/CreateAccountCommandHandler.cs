using Vouchers.Common.Application.Abstractions;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Core.Domain;
using Unit = Vouchers.Common.Application.Abstractions.Unit;

namespace Vouchers.Core.Application.UseCases.AccountCases;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand,Unit>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IRepository<Account,Guid> _accountRepository;

    public CreateAccountCommandHandler(IDateTimeProvider dateTimeProvider, IRepository<Account, Guid> accountRepository)
    {
        _dateTimeProvider = dateTimeProvider;
        _accountRepository = accountRepository;
    }

    public async Task<Result<Unit>> HandleAsync(CreateAccountCommand command, CancellationToken cancellation)
    {
        var account = Account.Create(command.AccountId, command.IdentityId, _dateTimeProvider.CurrentDateTime());
        if (command.IsConfirmed)
            account.IsActive = true;
        
        await _accountRepository.AddAsync(account);

        return Unit.Value;
    }
}