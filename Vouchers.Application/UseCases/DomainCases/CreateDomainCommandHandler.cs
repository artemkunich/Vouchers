﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vouchers.Application.Commands.DomainCommands;
using Vouchers.Application.Infrastructure;
using Vouchers.Application.Services;
using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;
using Vouchers.Primitives;

namespace Vouchers.Application.UseCases.DomainCases;

internal sealed class CreateDomainCommandHandler : IHandler<CreateDomainCommand, Result<Guid?>>
{
    private readonly IAuthIdentityProvider _authIdentityProvider;
    private readonly IReadOnlyRepository<DomainOffer,Guid> _domainOfferRepository;
    private readonly IReadOnlyRepository<DomainOffersPerIdentityCounter,Guid> _domainOffersPerIdentityCounterRepository;
    private readonly IRepository<DomainContract,Guid> _domainContractRepository;
    private readonly IRepository<DomainAccount,Guid> _domainAccountRepository;
    private readonly IRepository<Account,Guid> _accountRepository;
    private readonly IIdentifierProvider<Guid> _identifierProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    public CreateDomainCommandHandler(IAuthIdentityProvider authIdentityProvider, IReadOnlyRepository<DomainOffer,Guid> domainOfferRepository, 
        IReadOnlyRepository<DomainOffersPerIdentityCounter,Guid> domainOffersPerIdentityCounterRepository, IRepository<DomainContract,Guid> domainContractRepository, 
        IRepository<DomainAccount,Guid> domainAccountRepository, IRepository<Account,Guid> accountRepository, IIdentifierProvider<Guid> identifierProvider, IDateTimeProvider dateTimeProvider)
    {
        _authIdentityProvider = authIdentityProvider;
        _domainOfferRepository = domainOfferRepository;
        _domainOffersPerIdentityCounterRepository = domainOffersPerIdentityCounterRepository;
        _domainContractRepository = domainContractRepository;
        _domainAccountRepository = domainAccountRepository;
        _accountRepository = accountRepository;
        _identifierProvider = identifierProvider;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<Guid?>> HandleAsync(CreateDomainCommand command, CancellationToken cancellation)
    {
        var authIdentityIdResult = await _authIdentityProvider.GetAuthIdentityIdAsync();
        var domainOfferResult = await authIdentityIdResult.MapAsync(_ => _domainOfferRepository.GetByIdAsync(command.OfferId));

        domainOfferResult.ProcessAsync(domainOffer => 
        )
        DomainOffersPerIdentityCounter domainOffersPerIdentityCounter = null;
        if (domainOffer.MaxContractsPerIdentity is not null)
        {
            domainOffersPerIdentityCounter = (await _domainOffersPerIdentityCounterRepository.GetByExpressionAsync(counter => counter.OfferId == domainOffer.Id && counter.IdentityId == authIdentityId)).FirstOrDefault();
            if (domainOffersPerIdentityCounter is null)
            {
                var domainOffersPerIdentityCounterId = _identifierProvider.CreateNewId();
                domainOffersPerIdentityCounter = DomainOffersPerIdentityCounter.Create(domainOffersPerIdentityCounterId, domainOffer, authIdentityId, 0);
                domainOffersPerIdentityCounter.AddContract();
            }
            else if (domainOffersPerIdentityCounter.Counter > domainOffer.MaxContractsPerIdentity)
            {
                throw new ApplicationException(Properties.Resources.MaxCountOfContractsExceeded);
            }
            else
            {
                domainOffersPerIdentityCounter.AddContract();
            }
        }

        var domainContract = DomainContract.Create(_identifierProvider.CreateNewId(), domainOffer, domainOffersPerIdentityCounter, authIdentityId, command.DomainName, _dateTimeProvider.CurrentDateTime());
            
        if (domainOffer.Amount.Amount == 0)
        {
            var accountId = _identifierProvider.CreateNewId();
            var accountResult = Account.Create(accountId, _dateTimeProvider.CurrentDateTime());
            if (accountResult.IsFailure)
                return accountResult.Errors;

            var account = accountResult.Value;
            await _accountRepository.AddAsync(account);

            var domainId = _identifierProvider.CreateNewId();
            var domain = Domain.Create(domainId, domainContract, 0, 0, _dateTimeProvider.CurrentDateTime());
            domain.IncreaseMembersCount();

            var domainAccount = DomainAccount.Create(account.Id, authIdentityId, domain, _dateTimeProvider.CurrentDateTime());
            domainAccount.IsIssuer = true;
            domainAccount.IsConfirmed = true;
                
            await _domainAccountRepository.AddAsync(domainAccount);
            return domain.Id;

        }
        
        await _domainContractRepository.AddAsync(domainContract);
        return null;
    }
}