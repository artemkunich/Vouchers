using FluentAssertions;
using Vouchers.Core.Domain.Exceptions;

namespace Vouchers.Core.Domain.UnitTests;

public class HolderTransactionItemTests
{
    private readonly Account _issuerAccount;
    private readonly UnitType _unitType;
    private readonly Unit _unit;
    private readonly AccountItem _issuerAccountItem;

    public HolderTransactionItemTests()
    {
        var identityId = Guid.NewGuid();
        var issuerAccountId = Guid.NewGuid();
        _issuerAccount = Account.Create(issuerAccountId, identityId, DateTime.Now);
        
        var unitTypeId = Guid.NewGuid();
        _unitType = UnitType.Create(unitTypeId, _issuerAccount);

        var validFrom = DateTime.Now;
        var validTo = validFrom.AddHours(1);
        var unitId = Guid.NewGuid();
        _unit = Unit.Create(unitId, validFrom, validTo, DateTime.Now, true, _unitType);

        _issuerAccountItem = AccountItem.Create(Guid.NewGuid(), _issuerAccount, _unit);

    }

    [Fact]
    public void Create_WithNotPositiveAmount_ThrowsNotPositiveAmountException()
    {
        var issuerTransaction = IssuerTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccountItem, 1);
        issuerTransaction.Perform();
        
        var holderIdentityId = Guid.NewGuid();
        var holderAccountId = Guid.NewGuid();
        var holderAccount = Account.Create(holderAccountId, holderIdentityId, DateTime.Now);
        
        var holderAccountItem = AccountItem.Create(Guid.NewGuid(), holderAccount, _unit);
        var transaction = HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccount, holderAccount, _unitType, "");
        var createWithZeroAmount = () => HolderTransactionItem.Create(Guid.NewGuid(), 0, _issuerAccountItem, holderAccountItem, transaction);
        var createWithNegativeAmount = () => HolderTransactionItem.Create(Guid.NewGuid(), -1, _issuerAccountItem, holderAccountItem, transaction);

        createWithZeroAmount
            .Should()
            .Throw<NotPositiveAmountException>();
        
        createWithNegativeAmount
            .Should()
            .Throw<NotPositiveAmountException>();
        
    }

    [Fact]
    public void Create_WithSameAccountItems_ThrowsCoreException()
    {
        var issuerTransaction = IssuerTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccountItem, 1);
        issuerTransaction.Perform();
        
        var holderIdentityId = Guid.NewGuid();
        var holderAccountId = Guid.NewGuid();
        var holderAccount = Account.Create(holderAccountId, holderIdentityId, DateTime.Now);
        var holderAccountItem = AccountItem.Create(Guid.NewGuid(), holderAccount, _unit);
        
        var transactionFromIssuerToHolder = HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccount, holderAccount, _unitType, "");
        var createWithIssuerAccountItems = () => HolderTransactionItem.Create(Guid.NewGuid(), 1, _issuerAccountItem, _issuerAccountItem, transactionFromIssuerToHolder);
        
        createWithIssuerAccountItems
            .Should()
            .Throw<CreditorAndDebtorAccountsAreEqualException>();
        
        HolderTransactionItem.Create(Guid.NewGuid(), 1, _issuerAccountItem, holderAccountItem, transactionFromIssuerToHolder);
        transactionFromIssuerToHolder.Perform();

        transactionFromIssuerToHolder = HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccount, holderAccount, _unitType, "");
        var createWithBobAccountItems = () => HolderTransactionItem.Create(Guid.NewGuid(), 1, holderAccountItem, holderAccountItem, transactionFromIssuerToHolder);
        
        createWithBobAccountItems
            .Should()
            .Throw<CreditorAndDebtorAccountsAreEqualException>();
    }
    
    [Fact]
    public void Create_WithAccountItemsWithAnotherUnit_ThrowsCreditAccountAndDebitAccountHaveDifferentUnitsException()
    {
        var anotherUnitValidFrom = DateTime.Now;
        var anotherUnitValidTo = anotherUnitValidFrom.AddHours(1);
        var anotherUnitId = Guid.NewGuid();
        var anotherUnit = Unit.Create(anotherUnitId, anotherUnitValidFrom, anotherUnitValidTo, DateTime.Now, true, _unitType);
        
        var holderIdentityId = Guid.NewGuid();
        var holderAccountWithAnotherUnitId = Guid.NewGuid();
        var holderAccountWithAnotherUnit = Account.Create(holderAccountWithAnotherUnitId, holderIdentityId, DateTime.Now);
        
        var holderAccountItemWithAnotherUnit = AccountItem.Create(Guid.NewGuid(), holderAccountWithAnotherUnit, anotherUnit);
        
        var transactionFromIssuerToHolderWithAnotherUnit = HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccount, holderAccountWithAnotherUnit, _unitType, "");
        var createWithAnotherUnit = () => HolderTransactionItem.Create(Guid.NewGuid(), 1, _issuerAccountItem, holderAccountItemWithAnotherUnit, transactionFromIssuerToHolderWithAnotherUnit);

        createWithAnotherUnit
            .Should()
            .Throw<CreditAccountAndDebitAccountHaveDifferentUnitsException>();
    }
    
    [Fact]
    public void Create_FromHolderToAnotherHolderWithNotChangeableUnit_ThrowsItemUnitCannotBeExchangedException()
    {
        var notChangeableUnitValidFrom = DateTime.Now;
        var notChangeableUnitValidTo = notChangeableUnitValidFrom.AddHours(1);
        var notChangeableUnitId = Guid.NewGuid();
        var notChangeableUnit = Unit.Create(notChangeableUnitId, notChangeableUnitValidFrom, notChangeableUnitValidTo, DateTime.Now, false, _unitType);

        var issuerAccountItem = AccountItem.Create(Guid.NewGuid(), _issuerAccount, notChangeableUnit);
        var issuerTransaction = IssuerTransaction.Create(Guid.NewGuid(), DateTime.Now, issuerAccountItem, 1);
        issuerTransaction.Perform();
        
        var holderIdentityId = Guid.NewGuid();
        var holderAccountId = Guid.NewGuid();
        var holderAccount = Account.Create(holderAccountId, holderIdentityId, DateTime.Now);
        var holderAccountItem = AccountItem.Create(Guid.NewGuid(), holderAccount, notChangeableUnit);

        var transactionFromIssuerToHolder = HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccount, holderAccount, _unitType, "");
        var transactionItemFromIssuerToHolder = HolderTransactionItem.Create(Guid.NewGuid(), 1, issuerAccountItem, holderAccountItem, transactionFromIssuerToHolder);
        transactionFromIssuerToHolder.Perform();
        
        var anotherHolderIdentityId = Guid.NewGuid();
        var anotherHolderAccountId = Guid.NewGuid();
        var anotherHolderAccount = Account.Create(anotherHolderAccountId, anotherHolderIdentityId, DateTime.Now);
        var anotherHolderAccountItem = AccountItem.Create(Guid.NewGuid(), anotherHolderAccount, notChangeableUnit);
        
        var transactionFromHolderToAnotherHolder = HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, holderAccount, anotherHolderAccount, _unitType, "");
        var createTransactionFromHolderToAnotherHolder = () => HolderTransactionItem.Create(Guid.NewGuid(), 1, holderAccountItem, anotherHolderAccountItem, transactionFromHolderToAnotherHolder);
        createTransactionFromHolderToAnotherHolder
            .Should()
            .Throw<ItemUnitCannotBeExchangedException>();
        
    }
    
    [Fact]
    public void Create_WithTheSameIdInTheSameTransaction_ThrowsTransactionAlreadyContainsItemException()
    {
        var issuerTransaction = IssuerTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccountItem, 1);
        issuerTransaction.Perform();
        
        var holderIdentityId = Guid.NewGuid();
        var holderAccountId = Guid.NewGuid();
        var holderAccount = Account.Create(holderAccountId, holderIdentityId, DateTime.Now);
        
        var holderAccountItem = AccountItem.Create(Guid.NewGuid(), holderAccount, _unit);
        var transaction = HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccount, holderAccount, _unitType, "");

        var transactionItemId = Guid.NewGuid();
        var createWithAmount = () => HolderTransactionItem.Create(transactionItemId, 1, _issuerAccountItem, holderAccountItem, transaction);

        createWithAmount();
        createWithAmount
            .Should()
            .Throw<TransactionAlreadyContainsItemException>();
    }
    
    [Fact]
    public void Create_WithExpiredUnit_ThrowsTransactionContainsExpiredUnitsException()
    {
        var issuerTransaction = IssuerTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccountItem, 1);
        issuerTransaction.Perform();
        
        var holderIdentityId = Guid.NewGuid();
        var holderAccountId = Guid.NewGuid();
        var holderAccount = Account.Create(holderAccountId, holderIdentityId, DateTime.Now);
        
        var holderAccountItem = AccountItem.Create(Guid.NewGuid(), holderAccount, _unit);

        var currentDateTime = _unit.ValidTo.AddMinutes(1);
        
        var transaction = HolderTransaction.Create(Guid.NewGuid(), currentDateTime, _issuerAccount, holderAccount, _unitType, "");

        var transactionItemId = Guid.NewGuid();
        var createWithAmount = () => HolderTransactionItem.Create(transactionItemId, 1, _issuerAccountItem, holderAccountItem, transaction);
        
        createWithAmount
            .Should()
            .Throw<TransactionContainsExpiredUnitsException>();
    }
}