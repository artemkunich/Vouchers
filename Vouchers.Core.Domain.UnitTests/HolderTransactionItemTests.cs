using FluentAssertions;

namespace Vouchers.Core.Domain.UnitTests;

public class HolderTransactionItemTests
{
    private readonly Account _issuerAccount;
    private readonly UnitType _unitType;
    private readonly Unit _unit;
    private readonly AccountItem _issuerAccountItem;

    public HolderTransactionItemTests()
    {
        var issuerAccountId = Guid.NewGuid();
        _issuerAccount = Account.Create(issuerAccountId, DateTime.Now);
        
        var unitTypeId = Guid.NewGuid();
        _unitType = UnitType.Create(unitTypeId, _issuerAccount);

        var validFrom = DateTime.Now;
        var validTo = validFrom.AddHours(1);
        var unitId = Guid.NewGuid();
        _unit = Unit.Create(unitId, validFrom, validTo, DateTime.Now, true, _unitType);

        _issuerAccountItem = AccountItem.Create(Guid.NewGuid(), _issuerAccount, _unit);

    }

    [Fact]
    public void Create_WithNotPositiveAmount_ThrowsCoreException()
    {
        var issuerTransaction = IssuerTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccountItem, 1);
        issuerTransaction.Perform();
        
        var holderAccountId = Guid.NewGuid();
        var holderAccount = Account.Create(holderAccountId, DateTime.Now);
        
        var holderAccountItem = AccountItem.Create(Guid.NewGuid(), holderAccount, _unit);
        var transaction = HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccount, holderAccount, _unitType, "");
        var createWithZeroAmount = () => HolderTransactionItem.Create(Guid.NewGuid(), 0, _issuerAccountItem, holderAccountItem, transaction);
        var createWithNegativeAmount = () => HolderTransactionItem.Create(Guid.NewGuid(), -1, _issuerAccountItem, holderAccountItem, transaction);

        createWithZeroAmount
            .Should()
            .Throw<CoreException>()
            .WithMessage(CoreException.AmountIsNotPositive.Message);
        
        createWithNegativeAmount
            .Should()
            .Throw<CoreException>()
            .WithMessage(CoreException.AmountIsNotPositive.Message);
        
    }

    [Fact]
    public void Create_WithSameAccountItems_ThrowsCoreException()
    {
        var issuerTransaction = IssuerTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccountItem, 1);
        issuerTransaction.Perform();
        
        var holderAccountId = Guid.NewGuid();
        var holderAccount = Account.Create(holderAccountId, DateTime.Now);
        var holderAccountItem = AccountItem.Create(Guid.NewGuid(), holderAccount, _unit);
        
        var transactionFromIssuerToHolder = HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccount, holderAccount, _unitType, "");
        var createWithIssuerAccountItems = () => HolderTransactionItem.Create(Guid.NewGuid(), 1, _issuerAccountItem, _issuerAccountItem, transactionFromIssuerToHolder);
        
        createWithIssuerAccountItems
            .Should()
            .Throw<CoreException>()
            .WithMessage(CoreException.CreditorAndDebtorAccountsAreTheSame.Message);
        
        HolderTransactionItem.Create(Guid.NewGuid(), 1, _issuerAccountItem, holderAccountItem, transactionFromIssuerToHolder);
        transactionFromIssuerToHolder.Perform();

        transactionFromIssuerToHolder = HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccount, holderAccount, _unitType, "");
        var createWithBobAccountItems = () => HolderTransactionItem.Create(Guid.NewGuid(), 1, holderAccountItem, holderAccountItem, transactionFromIssuerToHolder);
        
        createWithBobAccountItems
            .Should()
            .Throw<CoreException>()
            .WithMessage(CoreException.CreditorAndDebtorAccountsAreTheSame.Message);
    }
    
    [Fact]
    public void Create_WithAccountItemsWithAnotherUnit_ThrowsCoreException()
    {
        var anotherUnitValidFrom = DateTime.Now;
        var anotherUnitValidTo = anotherUnitValidFrom.AddHours(1);
        var anotherUnitId = Guid.NewGuid();
        var anotherUnit = Unit.Create(anotherUnitId, anotherUnitValidFrom, anotherUnitValidTo, DateTime.Now, true, _unitType);
        
        var holderAccountWithAnotherUnitId = Guid.NewGuid();
        var holderAccountWithAnotherUnit = Account.Create(holderAccountWithAnotherUnitId, DateTime.Now);
        
        var holderAccountItemWithAnotherUnit = AccountItem.Create(Guid.NewGuid(), holderAccountWithAnotherUnit, anotherUnit);
        
        var transactionFromIssuerToHolderWithAnotherUnit = HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccount, holderAccountWithAnotherUnit, _unitType, "");
        var createWithAnotherUnit = () => HolderTransactionItem.Create(Guid.NewGuid(), 1, _issuerAccountItem, holderAccountItemWithAnotherUnit, transactionFromIssuerToHolderWithAnotherUnit);

        createWithAnotherUnit
            .Should()
            .Throw<CoreException>()
            .WithMessage(CoreException.CreditAccountAndDebitAccountHaveDifferentUnits.Message);
    }
    
    [Fact]
    public void Create_FromHolderToAnotherHolderWithNotChangeableUnit_ThrowsCoreException()
    {
        var notChangeableUnitValidFrom = DateTime.Now;
        var notChangeableUnitValidTo = notChangeableUnitValidFrom.AddHours(1);
        var notChangeableUnitId = Guid.NewGuid();
        var notChangeableUnit = Unit.Create(notChangeableUnitId, notChangeableUnitValidFrom, notChangeableUnitValidTo, DateTime.Now, false, _unitType);

        var issuerAccountItem = AccountItem.Create(Guid.NewGuid(), _issuerAccount, notChangeableUnit);
        var issuerTransaction = IssuerTransaction.Create(Guid.NewGuid(), DateTime.Now, issuerAccountItem, 1);
        issuerTransaction.Perform();
        
        var holderAccountId = Guid.NewGuid();
        var holderAccount = Account.Create(holderAccountId, DateTime.Now);
        var holderAccountItem = AccountItem.Create(Guid.NewGuid(), holderAccount, notChangeableUnit);

        var transactionFromIssuerToHolder = HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccount, holderAccount, _unitType, "");
        var transactionItemFromIssuerToHolder = HolderTransactionItem.Create(Guid.NewGuid(), 1, issuerAccountItem, holderAccountItem, transactionFromIssuerToHolder);
        transactionFromIssuerToHolder.Perform();
        
        var anotherHolderAccountId = Guid.NewGuid();
        var anotherHolderAccount = Account.Create(anotherHolderAccountId, DateTime.Now);
        var anotherHolderAccountItem = AccountItem.Create(Guid.NewGuid(), anotherHolderAccount, notChangeableUnit);
        
        var transactionFromHolderToAnotherHolder = HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, holderAccount, anotherHolderAccount, _unitType, "");
        var createTransactionFromHolderToAnotherHolder = () => HolderTransactionItem.Create(Guid.NewGuid(), 1, holderAccountItem, anotherHolderAccountItem, transactionFromHolderToAnotherHolder);
        createTransactionFromHolderToAnotherHolder
            .Should()
            .Throw<CoreException>()
            .WithMessage(CoreException.ItemUnitCannotBeExchanged.Message);
        
    }
    
    [Fact]
    public void Create_WithTheSameIdInTheSameTransaction_ThrowsCoreException()
    {
        var issuerTransaction = IssuerTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccountItem, 1);
        issuerTransaction.Perform();
        
        var holderAccountId = Guid.NewGuid();
        var holderAccount = Account.Create(holderAccountId, DateTime.Now);
        
        var holderAccountItem = AccountItem.Create(Guid.NewGuid(), holderAccount, _unit);
        var transaction = HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccount, holderAccount, _unitType, "");

        var transactionItemId = Guid.NewGuid();
        var createWithAmount = () => HolderTransactionItem.Create(transactionItemId, 1, _issuerAccountItem, holderAccountItem, transaction);

        createWithAmount();
        createWithAmount
            .Should()
            .Throw<CoreException>()
            .WithMessage(CoreException.TransactionAlreadyContainsItem.Message);
    }
    
    [Fact]
    public void Create_WithExpiredUnit_ThrowsCoreException()
    {
        var issuerTransaction = IssuerTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccountItem, 1);
        issuerTransaction.Perform();
        
        var holderAccountId = Guid.NewGuid();
        var holderAccount = Account.Create(holderAccountId, DateTime.Now);
        
        var holderAccountItem = AccountItem.Create(Guid.NewGuid(), holderAccount, _unit);

        var currentDateTime = _unit.ValidTo.AddMinutes(1);
        
        var transaction = HolderTransaction.Create(Guid.NewGuid(), currentDateTime, _issuerAccount, holderAccount, _unitType, "");

        var transactionItemId = Guid.NewGuid();
        var createWithAmount = () => HolderTransactionItem.Create(transactionItemId, 1, _issuerAccountItem, holderAccountItem, transaction);
        
        createWithAmount
            .Should()
            .Throw<CoreException>()
            .WithMessage(CoreException.TransactionContainsExpiredUnits.Message);
    }
}