using FluentAssertions;
using Vouchers.Core.Domain.Exceptions;

namespace Vouchers.Core.Domain.UnitTests;

public class IssuerTransactionTests
{
    private readonly Account _issuerAccount;
    private readonly Account _holderAccount;
    private readonly UnitType _unitType;
    private readonly Unit _unit;
    private readonly AccountItem _accountItem;
    
    public IssuerTransactionTests()
    {
        var issuerAccountId = Guid.NewGuid();
        _issuerAccount = Account.Create(issuerAccountId, DateTime.Now);
        
        var holderAccountId = Guid.NewGuid();
        _holderAccount = Account.Create(holderAccountId, DateTime.Now);

        var unitTypeId = Guid.NewGuid();
        _unitType = UnitType.Create(unitTypeId, _issuerAccount);

        var validFrom = DateTime.Now;
        var validTo = validFrom.AddHours(1);
        var unitId = Guid.NewGuid();
        _unit = Unit.Create(unitId, validFrom, validTo, DateTime.Now, true, _unitType);

        _accountItem = AccountItem.Create(Guid.NewGuid(), _issuerAccount, _unit);
        
    }

    [Fact]
    public void Create_WithZeroAmount_ThrowsZeroAmountException()
    {
        var issuerTransactionId = Guid.NewGuid();
        var currentDateDime = _accountItem.Unit.ValidFrom;
        var createIssuerTransactionWithZeroAmount = () => IssuerTransaction.Create(issuerTransactionId, currentDateDime, _accountItem, 0);

        createIssuerTransactionWithZeroAmount
            .Should()
            .Throw<ZeroAmountException>();
    }
    
    [Fact]
    public void Create_WithExpiredUnit_ThrowsUnitIsExpiredException()
    {
        var issuerTransactionId = Guid.NewGuid();
        var currentDateDime = _accountItem.Unit.ValidTo.AddDays(1);
        var createIssuerTransactionWithExpiredUnit = () => IssuerTransaction.Create(issuerTransactionId, currentDateDime, _accountItem, 1);

        createIssuerTransactionWithExpiredUnit
            .Should()
            .Throw<UnitIsExpiredException>();
    }
    
    [Fact]
    public void Create_WithAccountItemWhereHolderAccountAndUnitTypeIssuerAccountAreDifferent_ThrowsAccountHolderAndUnitTypeIssuerAreDifferentException()
    {
        var accountItem = AccountItem.Create(Guid.NewGuid(), _holderAccount, _unit);
        
        var issuerTransactionId = Guid.NewGuid();
        var currentDateDime = _unit.ValidFrom;
        var createIssuerTransactionWithAccountItemWhereHolderAccountAndUnitTypeIssuerAccountAreDifferent = () => IssuerTransaction.Create(issuerTransactionId, currentDateDime, accountItem, 1);

        createIssuerTransactionWithAccountItemWhereHolderAccountAndUnitTypeIssuerAccountAreDifferent
            .Should()
            .Throw<AccountHolderAndUnitTypeIssuerAreDifferentException>();
    }
    
    [Fact]
    public void Perform_WithPositiveAmountIncreases_WithNegativeAmountReduces_AccountItemBalance_UnitSupply_UnitTypeSupply_UnitTypeIssuerAccountSupply_ByAmount()
    {
        decimal[] positiveAmounts = {1, 5, 8, -1, -5, -8};
        
        foreach (var positiveAmount in positiveAmounts)
        {
            var expectedNewAccountItemBalance = _accountItem.Balance + positiveAmount;
            var expectedNewAccountItemUnitSupply = _accountItem.Unit.Supply + positiveAmount;
            var expectedNewAccountItemUnitTypeSupply = _accountItem.Unit.UnitType.Supply + positiveAmount;
        
            var issuerTransactionId = Guid.NewGuid();
            var currentDateDime = _accountItem.Unit.ValidFrom;
            var issuerTransaction = IssuerTransaction.Create(issuerTransactionId, currentDateDime, _accountItem, positiveAmount);
            issuerTransaction.Perform();

            _accountItem.Balance.Should().Be(expectedNewAccountItemBalance);
            _accountItem.Unit.Supply.Should().Be(expectedNewAccountItemUnitSupply);
            _accountItem.Unit.UnitType.Supply.Should().Be(expectedNewAccountItemUnitTypeSupply);
        }
    }
}