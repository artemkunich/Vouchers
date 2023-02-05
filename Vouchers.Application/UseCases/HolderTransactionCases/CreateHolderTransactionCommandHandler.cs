using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Core.Domain;
using Vouchers.Application.Infrastructure;
using Vouchers.Domains.Domain;
using Vouchers.Application.Commands.HolderTransactionCommands;
using Vouchers.Application.Services;

namespace Vouchers.Application.UseCases.HolderTransactionCases;

internal sealed class CreateHolderTransactionCommandHandler : IHandler<CreateHolderTransactionCommand, Result<Guid>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IReadOnlyRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IReadOnlyRepository<Account,Guid> _accountRepository;
    private readonly IReadOnlyRepository<Unit,Guid> _unitRepository;
    private readonly IReadOnlyRepository<UnitType,Guid> _unitTypeRepository;
    private readonly IReadOnlyRepository<AccountItem,Guid> _accountItemRepository;
    private readonly IRepository<HolderTransaction,Guid> _holderTransactionRepository;
    private readonly IRepository<HolderTransactionRequest,Guid> _holderTransactionRequestRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICultureInfoProvider _cultureInfoProvider;
    
    public CreateHolderTransactionCommandHandler(IAuthIdentityProvider authIdentityProvider, IReadOnlyRepository<DomainAccount,Guid> domainAccountRepository, 
        IReadOnlyRepository<Account,Guid> accountRepository, IReadOnlyRepository<Unit,Guid> unitRepository, 
        IReadOnlyRepository<UnitType,Guid> unitTypeRepository, IReadOnlyRepository<AccountItem,Guid> accountItemRepository, 
        IRepository<HolderTransaction,Guid> holderTransactionRepository, IRepository<HolderTransactionRequest,Guid> holderTransactionRequestRepository, IIdentifierProvider<Guid> identifierProvider, IDateTimeProvider dateTimeProvider, ICultureInfoProvider cultureInfoProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _domainAccountRepository = domainAccountRepository;
        _accountRepository = accountRepository;
        _unitRepository = unitRepository;
        _unitTypeRepository = unitTypeRepository;
        _accountItemRepository = accountItemRepository;
        _holderTransactionRepository = holderTransactionRepository;
        _holderTransactionRequestRepository = holderTransactionRequestRepository;
        _identifierProvider = identifierProvider;
        _dateTimeProvider = dateTimeProvider;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task<Result<Guid>> HandleAsync(CreateHolderTransactionCommand command, CancellationToken cancellation)
    {
        var cultureInfo = _cultureInfoProvider.GetCultureInfo();
        
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();
        if (authIdentityId is null)
            return Error.NotAuthorized(cultureInfo);
        
        var creditorDomainAccount = await _domainAccountRepository.GetByIdAsync(command.CreditorAccountId);
        if (creditorDomainAccount?.IdentityId != authIdentityId)
            return Error.OperationIsNotAllowed(cultureInfo);
        if (!creditorDomainAccount.IsConfirmed)
            return Error.CreditorAccountIsNotActivated(cultureInfo);

        HolderTransactionRequest holderTransactionRequest = null;
        if (command.HolderTransactionRequestId is not null)
        {
            holderTransactionRequest = await _holderTransactionRequestRepository.GetByIdAsync(command.HolderTransactionRequestId.Value);
            if(holderTransactionRequest is null)
                return Error.TransactionRequestIsNotFound(cultureInfo);
        }

        var debtorDomainAccount = await _domainAccountRepository.GetByIdAsync(command.DebtorAccountId);
        if(debtorDomainAccount is null)
            return Error.DebtorAccountDoesNotExist(cultureInfo);
        if (!debtorDomainAccount.IsConfirmed)
            return Error.DebtorAccountIsNotActivated(cultureInfo);

        var unitType = await _unitTypeRepository.GetByIdAsync(command.UnitTypeId);
        if (unitType is null)
            return Error.VoucherValueDoesNotExist(cultureInfo);
        
        var creditorAccount = await _accountRepository.GetByIdAsync(command.CreditorAccountId);
        if(creditorAccount is null)
            return Error.CreditorAccountDoesNotExist(cultureInfo);
            
        var debtorAccount = await _accountRepository.GetByIdAsync(command.DebtorAccountId);
        if (debtorAccount is null)
            return Error.DebtorAccountDoesNotExist(cultureInfo);

        var transactionId = _identifierProvider.CreateNewId();
        var currentDateTime = _dateTimeProvider.CurrentDateTime();
        HolderTransaction transaction = HolderTransaction.Create(transactionId, currentDateTime, creditorAccount, debtorAccount, unitType, command.Message);

        foreach (var item in command.Items)
        {
            var creditAccountItem = (await _accountItemRepository.GetByExpressionAsync(accItem => accItem.HolderAccountId == command.CreditorAccountId && accItem.Unit.Id == item.Item1)).FirstOrDefault();
            if (creditAccountItem is null)
            {
                return Error.UserDoesNotHaveAccountForVoucher(command.CreditorAccountId, item.Item1, cultureInfo);
            }
            var debitAccountItem = (await _accountItemRepository.GetByExpressionAsync(accItem => accItem.HolderAccountId == command.DebtorAccountId && accItem.Unit.Id == item.Item1)).FirstOrDefault();
            if (debitAccountItem is null)
            {
                var voucherId = _identifierProvider.CreateNewId();
                var voucher = await _unitRepository.GetByIdAsync(item.Item1);
                debitAccountItem = AccountItem.Create(voucherId, debtorAccount, 0, voucher);
            }

            var transactionItemId = _identifierProvider.CreateNewId();
            var transactionItem = HolderTransactionItem.Create(transactionItemId,
                UnitQuantity.Create(item.Item2, debitAccountItem.Unit), creditAccountItem, debitAccountItem);
            transaction.AddTransactionItem(transactionItem);
        }
    
        if (holderTransactionRequest is null)
        {
            transaction.Perform();
            await _holderTransactionRepository.AddAsync(transaction);
            return transaction.Id;
        }

        holderTransactionRequest.Perform(transaction);
        await _holderTransactionRequestRepository.UpdateAsync(holderTransactionRequest);
        return transaction.Id;
    }
}