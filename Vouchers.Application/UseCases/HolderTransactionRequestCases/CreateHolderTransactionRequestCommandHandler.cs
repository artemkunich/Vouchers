using System;
using Vouchers.Core.Domain;
using Vouchers.Application.Commands.HolderTransactionRequestCommands;
using Vouchers.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using Vouchers.Application.Abstractions;
using Vouchers.Domains.Domain;
using Vouchers.Application.Dtos;
using Vouchers.Application.Errors;
using Vouchers.Application.Services;

namespace Vouchers.Application.UseCases.HolderTransactionRequestCases;

internal sealed class CreateHolderTransactionRequestCommandHandler : IHandler<CreateHolderTransactionRequestCommand, IdDto<Guid>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IReadOnlyRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IReadOnlyRepository<Account,Guid> _accountRepository;
    private readonly IReadOnlyRepository<UnitType,Guid> _unitTypeRepository;
    private readonly IRepository<HolderTransactionRequest,Guid> _holderTransactionRequestRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    public CreateHolderTransactionRequestCommandHandler(IAuthIdentityProvider authIdentityProvider, 
        IReadOnlyRepository<DomainAccount,Guid> domainAccountRepository, IReadOnlyRepository<Account,Guid> accountRepository, 
        IReadOnlyRepository<UnitType,Guid> unitTypeRepository, IRepository<HolderTransactionRequest,Guid> holderTransactionRequestRepository, IIdentifierProvider<Guid> identifierProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _domainAccountRepository = domainAccountRepository;
        _accountRepository = accountRepository;
        _unitTypeRepository = unitTypeRepository;
        _holderTransactionRequestRepository = holderTransactionRequestRepository;
        _identifierProvider = identifierProvider;
    }

    public async Task<Result<IdDto<Guid>>> HandleAsync(CreateHolderTransactionRequestCommand command, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        
        var debtorDomainAccount = await _domainAccountRepository.GetByIdAsync(command.DebtorAccountId);
        if (debtorDomainAccount?.IdentityId != authIdentityId)
            return new OperationIsNotAllowedError();

        var debtorAccount = await _accountRepository.GetByIdAsync(command.DebtorAccountId);

        Account creditorAccount = null;
        if (command.CreditorAccountId != null)
            creditorAccount = await _accountRepository.GetByIdAsync(command.CreditorAccountId.Value);

        var unitType = await _unitTypeRepository.GetByIdAsync(command.UnitTypeId);

        var quantity = UnitTypeQuantity.Create(command.Amount, unitType);

        var maxDurationBeforeValidityStart = command.MaxDaysBeforeValidityStart is null ? TimeSpan.Zero : TimeSpan.FromDays(command.MaxDaysBeforeValidityStart.Value);
        var minDaysBeforeValidityEnd = command.MinDaysBeforeValidityEnd is null ? TimeSpan.Zero : TimeSpan.FromDays(command.MinDaysBeforeValidityEnd.Value);

        var requestId = _identifierProvider.CreateNewId();
        var request = HolderTransactionRequest.Create(requestId, command.DueDate, creditorAccount, debtorAccount, quantity, maxDurationBeforeValidityStart, minDaysBeforeValidityEnd, command.MustBeExchangeable, command.Message);

        await _holderTransactionRequestRepository.AddAsync(request);

        return new IdDto<Guid>(request.Id);
    }
}