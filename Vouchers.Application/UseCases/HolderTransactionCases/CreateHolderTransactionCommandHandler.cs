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
using Vouchers.Primitives;

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
        var authIdentityIdResult = await _authIdentityProvider.GetAuthIdentityIdAsync();
        var holderTransactionRequestResult = await authIdentityIdResult.ToResultAsync(async _ =>
            command.HolderTransactionRequestId is null
                ? Result.Create<HolderTransactionRequest>(null)
                : Result
                    .Create(await _holderTransactionRequestRepository.GetByIdAsync(
                        command.HolderTransactionRequestId.Value))
                    .IfTrueAddError(transactionRequest => transactionRequest is null, 
                        Errors.TransactionRequestIsNotFound(cultureInfo)));
        
        
        var creditorDomainAccountResult = (await holderTransactionRequestResult
            .MapAsync(_ => _domainAccountRepository.GetByIdAsync(command.CreditorAccountId)))
            .IfTrueAddError(creditorAccount => creditorAccount?.IdentityId != authIdentityIdResult.Value,
                Errors.OperationIsNotAllowed(cultureInfo))
            .IfTrueAddError(creditorAccount => !creditorAccount.IsConfirmed,
                Errors.CreditorAccountIsNotActivated(cultureInfo));

        var debtorDomainAccountResult = (await creditorDomainAccountResult
            .MapAsync(_ => _domainAccountRepository.GetByIdAsync(command.DebtorAccountId)))
            .IfValueIsNullAddError(Errors.DebtorAccountDoesNotExist(cultureInfo))
            .IfTrueAddError(debtorAccount => !debtorAccount.IsConfirmed, Errors.DebtorAccountIsNotActivated(cultureInfo));

        var unitTypeResult = (await debtorDomainAccountResult
            .MapAsync(_ => _unitTypeRepository.GetByIdAsync(command.UnitTypeId)))
            .IfValueIsNullAddError(Errors.VoucherValueDoesNotExist(cultureInfo));

        var creditorAccountResult = (await unitTypeResult
            .MapAsync(_ => _accountRepository.GetByIdAsync(command.CreditorAccountId)))
            .IfValueIsNullAddError(Errors.CreditorAccountDoesNotExist(cultureInfo));

        var debtorAccountResult = (await creditorAccountResult
            .MapAsync(_ => _accountRepository.GetByIdAsync(command.DebtorAccountId)))
            .IfValueIsNullAddError(Errors.DebtorAccountDoesNotExist(cultureInfo));


        var transactionResult = debtorAccountResult
            .ToResult(_ =>
            {
                var transactionId = _identifierProvider.CreateNewId();
                var currentDateTime = _dateTimeProvider.CurrentDateTime();
                var creditorAccount = creditorAccountResult.Value;
                var debtorAccount = debtorAccountResult.Value;
                var unitType = unitTypeResult.Value;
                return HolderTransaction.Create(transactionId, currentDateTime, creditorAccount, debtorAccount, unitType, command.Message);
            });

        await transactionResult.ForeachWhileSuccessAsync(_ => command.Items, async (item,transaction) =>
        {
            var creditAccountItemResult = Result.Create((await _accountItemRepository.GetByExpressionAsync(accItem =>
                        accItem.HolderAccountId == command.CreditorAccountId && accItem.Unit.Id == item.Item1))
                    .FirstOrDefault())
                .IfValueIsNullAddError(
                    Errors.UserDoesNotHaveAccountForVoucher(command.CreditorAccountId, item.Item1, cultureInfo));

            var debtorAccountItemResult = await (await creditAccountItemResult.MapAsync(async _ =>
                    (await _accountItemRepository.GetByExpressionAsync(accItem =>
                        accItem.HolderAccountId == command.DebtorAccountId && accItem.Unit.Id == item.Item1))
                    .FirstOrDefault()))
                .ToResultAsync(async debtorAccountItem =>
                {
                    if (debtorAccountItem is null)
                    {
                        return await Result.Create(_identifierProvider.CreateNewId())
                            .ToResultAsync(async accountItemId =>
                                Result
                                    .Create(await _unitRepository.GetByIdAsync(item.Item1))
                                    .IfValueIsNullAddError(Errors.VoucherValueDoesNotExist(cultureInfo))
                                    .ToResult(voucher => AccountItem.Create(accountItemId, debtorAccountResult.Value, 0, voucher))
                            );
                    }

                    return debtorAccountItem;
                });

            return debtorAccountItemResult
                .ToResult(debtorAccountItem => UnitQuantity.Create(item.Item2, debtorAccountItem.Unit))
                .ToResult(quantity => HolderTransactionItem.Create(_identifierProvider.CreateNewId(),
                    quantity, creditAccountItemResult.Value, debtorAccountItemResult.Value))
                .Process(transactionItem => transaction.AddTransactionItem(transactionItem));
        });

        return await transactionResult.MapAsync(async transaction =>
        {
            var holderTransactionRequest = holderTransactionRequestResult.Value;
            if (holderTransactionRequest is null)
            {
                transaction.Perform();
                await _holderTransactionRepository.AddAsync(transaction);
                return transaction.Id;
            }
            
            holderTransactionRequest.Perform(transaction);
            await _holderTransactionRequestRepository.UpdateAsync(holderTransactionRequest);
            return transaction.Id;
        });



    }
}