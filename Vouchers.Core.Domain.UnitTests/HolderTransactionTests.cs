using FluentAssertions;
using Vouchers.Core.Domain.Exceptions;

namespace Vouchers.Core.Domain.UnitTests;

public class HolderTransactionTests
{
    private readonly Account _issuerAccount;
    private readonly UnitType _unitType;
    private readonly Unit _unit;
    private readonly AccountItem _issuerAccountItem;

    public HolderTransactionTests()
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
    public void Create_WithTheSameAccounts_ThrowsCoreException()
    {
        var amount = 1;
        
        var issuerTransaction = IssuerTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccountItem, amount);
        issuerTransaction.Perform();
        
        var createTransactionFromIssuerToIssuer = () => HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccount, _issuerAccount, _unitType, "");

        createTransactionFromIssuerToIssuer
            .Should()
            .Throw<CreditorAndDebtorAccountsAreEqualException>();
        
        var holderAccountId = Guid.NewGuid();
        var holderAccount = Account.Create(holderAccountId, DateTime.Now);
        var holderAccountItem = AccountItem.Create(Guid.NewGuid(), holderAccount, _unit);
        
        var transactionFromIssuerToHolder = HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccount, holderAccount, _unitType, "");
        HolderTransactionItem.Create(Guid.NewGuid(), amount, _issuerAccountItem, holderAccountItem, transactionFromIssuerToHolder);
        transactionFromIssuerToHolder.Perform();
        
        var createTransactionFromHolderHolder = () => HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, holderAccount, holderAccount, _unitType, "");
        createTransactionFromHolderHolder
            .Should()
            .Throw<CreditorAndDebtorAccountsAreEqualException>();

    }
    
    [Fact]
    public void Perform_ReduceCreditorBalanceAndIncreaseDebtorBalance_ByAmount()
    {
        var amount = 1;
        
        var issuerTransaction = IssuerTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccountItem, amount);
        issuerTransaction.Perform();

        var holderAccountId = Guid.NewGuid();
        var holderAccount = Account.Create(holderAccountId, DateTime.Now);
        var holderAccountItem = AccountItem.Create(Guid.NewGuid(), holderAccount, _unit);

        
        var expectedIssuerAccountItemBalance = _issuerAccountItem.Balance - amount;
        var expectedHolderAccountItemBalance = holderAccountItem.Balance + amount;
        
        var transactionFromIssuerToHolder = HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, _issuerAccount, holderAccount, _unitType, "");
        HolderTransactionItem.Create(Guid.NewGuid(), amount, _issuerAccountItem, holderAccountItem, transactionFromIssuerToHolder);
        transactionFromIssuerToHolder.Perform();

        _issuerAccountItem.Balance
            .Should()
            .Be(expectedIssuerAccountItemBalance);

        holderAccountItem.Balance
            .Should()
            .Be(expectedHolderAccountItemBalance);
        
            
        var anotherHolderAccountId = Guid.NewGuid();
        var anotherHolderAccount = Account.Create(anotherHolderAccountId, DateTime.Now);
        var anotherHolderAccountItem = AccountItem.Create(Guid.NewGuid(), anotherHolderAccount, _unit);

        expectedHolderAccountItemBalance = expectedHolderAccountItemBalance - amount;
        var expectedAnotherHolderAccountItemBalance = expectedHolderAccountItemBalance + amount;
        
        var transactionFromHolderToAnotherHolder = HolderTransaction.Create(Guid.NewGuid(), DateTime.Now, holderAccount, anotherHolderAccount, _unitType, "");
        HolderTransactionItem.Create(Guid.NewGuid(), 1, holderAccountItem, anotherHolderAccountItem, transactionFromHolderToAnotherHolder);
        transactionFromHolderToAnotherHolder.Perform();
        
        holderAccountItem.Balance
            .Should()
            .Be(expectedHolderAccountItemBalance);

        anotherHolderAccountItem.Balance
            .Should()
            .Be(expectedAnotherHolderAccountItemBalance);
        
    }
}