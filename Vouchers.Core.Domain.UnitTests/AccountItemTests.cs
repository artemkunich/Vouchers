
using FluentAssertions;
using Vouchers.Core.Domain.Exceptions;

namespace Vouchers.Core.Domain.UnitTests;

public class AccountItemTests
{
    private readonly AccountItem _accountItem;

    public AccountItemTests()
    {
        var issuerAccountId = Guid.NewGuid();
        var issuerAccount = Account.Create(issuerAccountId, DateTime.Now);
        
        var holderAccountId = Guid.NewGuid();
        var holderAccount = Account.Create(holderAccountId, DateTime.Now);

        var unitTypeId = Guid.NewGuid();
        var unitType = UnitType.Create(unitTypeId, issuerAccount);

        var validFrom = DateTime.Now;
        var validTo = validFrom.AddHours(1);
        var unitId = Guid.NewGuid();
        var unit = Unit.Create(unitId, validFrom, validTo, DateTime.Now, true, unitType);

        _accountItem = AccountItem.Create(Guid.NewGuid(), holderAccount, unit);
    }
    
    [Fact]
    public void Create_AccountItem_ReturnAccountItemWithZeroBalance()
    {
        _accountItem.Balance.Should().Be(0, "0 is initial balance value");
    }

    [Fact]
    public void ProcessDebit_WithNotPositiveAmount_ThrowsNotPositiveAmountException()
    {
        var processDebitWithZeroAmount = () => _accountItem.ProcessDebit(0);
        var processDebitWithNegativeAmount = () => _accountItem.ProcessDebit(-1);

        processDebitWithZeroAmount
            .Should()
            .Throw<NotPositiveAmountException>();

        processDebitWithNegativeAmount
            .Should()
            .Throw<NotPositiveAmountException>();
    }

    [Fact]
    public void ProcessCredit_WithNotPositiveAmount_ThrowsNotPositiveAmountExceptionn()
    {
        var processCreditWithZeroAmount = () => _accountItem.ProcessCredit(0);
        var processCreditWithNegativeAmount = () => _accountItem.ProcessCredit(-1);

        processCreditWithZeroAmount
            .Should()
            .Throw<NotPositiveAmountException>();

        processCreditWithNegativeAmount
            .Should()
            .Throw<NotPositiveAmountException>();
    }
    
    [Fact]
    public void ProcessCredit_WithAmountGreaterThenBalance_ThrowsAmountIsGreaterThanBalanceException()
    {
        var amount = 1;
        _accountItem.ProcessDebit(amount);
        var processCredit = () => _accountItem.ProcessCredit(amount + 1);
        processCredit
            .Should()
            .Throw<AmountIsGreaterThanBalanceException>();
    }
    
    [Fact]
    public void ProcessDebit_IncreasesBalanceByAmount_ProcessCredit_ReducesBalanceByAmount()
    {
        var amount = 1;
        var expectedBalanceAfterDebit = _accountItem.Balance + amount;
        
        _accountItem.ProcessDebit(amount);
        
        _accountItem.Balance
            .Should()
            .Be(expectedBalanceAfterDebit);
        
        var expectedBalanceAfterCredit = _accountItem.Balance - amount;
        
        _accountItem.ProcessCredit(amount);
        _accountItem.Balance
            .Should()
            .Be(expectedBalanceAfterCredit);
    }
}