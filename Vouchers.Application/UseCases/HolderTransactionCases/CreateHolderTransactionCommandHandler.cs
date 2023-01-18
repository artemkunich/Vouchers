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

internal sealed class CreateHolderTransactionCommandHandler : IHandler<CreateHolderTransactionCommand, Guid>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IRepository<Account,Guid> _accountRepository;
    private readonly IRepository<Unit,Guid> _unitRepository;
    private readonly IRepository<UnitType,Guid> _unitTypeRepository;
    private readonly IRepository<AccountItem,Guid> _accountItemRepository;
    private readonly IRepository<HolderTransaction,Guid> _holderTransactionRepository;
    private readonly IRepository<HolderTransactionRequest,Guid> _holderTransactionRequestRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public CreateHolderTransactionCommandHandler(IAuthIdentityProvider authIdentityProvider, IRepository<DomainAccount,Guid> domainAccountRepository, 
        IRepository<Account,Guid> accountRepository, IRepository<Unit,Guid> unitRepository, 
        IRepository<UnitType,Guid> unitTypeRepository, IRepository<AccountItem,Guid> accountItemRepository, 
        IRepository<HolderTransaction,Guid> holderTransactionRepository, IRepository<HolderTransactionRequest,Guid> holderTransactionRequestRepository, IIdentifierProvider<Guid> identifierProvider, IDateTimeProvider dateTimeProvider)
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
    }

    public async Task<Guid> HandleAsync(CreateHolderTransactionCommand command, CancellationToken cancellation)
    {
        var authIdentityId = await _authIdentityProvider.GetAuthIdentityIdAsync();

        var creditorDomainAccount = await _domainAccountRepository.GetByIdAsync(command.CreditorAccountId);
        if (creditorDomainAccount?.IdentityId != authIdentityId)
            throw new ApplicationException(Properties.Resources.OperationIsNotAllowed);
        if (!creditorDomainAccount.IsConfirmed)
            throw new ApplicationException(Properties.Resources.CreditorAccountIsNotActivated);

        HolderTransactionRequest holderTransactionRequest = null;
        if (command.HolderTransactionRequestId is not null)
        {
            holderTransactionRequest = await _holderTransactionRequestRepository.GetByIdAsync(command.HolderTransactionRequestId.Value);
            if(holderTransactionRequest is null)
                throw new ApplicationException(Properties.Resources.TransactionRequestIsNotFound);
        }

        var debtorDomainAccount = await _domainAccountRepository.GetByIdAsync(command.DebtorAccountId);
        if(debtorDomainAccount is null)
            throw new ApplicationException(Properties.Resources.DebtorAccountDoesNotExist);
        if (!debtorDomainAccount.IsConfirmed)
            throw new ApplicationException(Properties.Resources.DebtorAccountIsNotActivated);

        var unitType = await _unitTypeRepository.GetByIdAsync(command.UnitTypeId);
        if (unitType is null)
            throw new ApplicationException(Properties.Resources.VoucherValueDoesNotExist);

        var creditorAccount = await _accountRepository.GetByIdAsync(command.CreditorAccountId);
        if(creditorAccount is null)
            throw new ApplicationException(Properties.Resources.CreditorAccountDoesNotExist);

        var debtorAccount = await _accountRepository.GetByIdAsync(command.DebtorAccountId);
        if (debtorAccount is null)
            throw new ApplicationException(Properties.Resources.DebtorAccountDoesNotExist);

        var transactionId = _identifierProvider.CreateNewId();
        var currentDateTime = _dateTimeProvider.CurrentDateTime();
        HolderTransaction transaction = HolderTransaction.Create(transactionId, currentDateTime, creditorAccount, debtorAccount, unitType, command.Message);

        foreach (var item in command.Items)
        {
            var creditAccountItem = (await _accountItemRepository.GetByExpressionAsync(accItem => accItem.HolderAccountId == command.CreditorAccountId && accItem.Unit.Id == item.Item1)).FirstOrDefault();
            if (creditAccountItem is null)
            {
                throw new ApplicationException(Properties.Resources.UserDoesNotHaveAccountForVoucher, command.CreditorAccountId, item.Item2);
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