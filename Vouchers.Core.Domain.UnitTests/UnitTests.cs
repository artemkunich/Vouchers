using FluentAssertions;
using Vouchers.Core.Domain.Exceptions;

namespace Vouchers.Core.Domain.UnitTests;

public class UnitTests
{
    private readonly Account _issuerAccount;
    private readonly UnitType _unitType;
    private readonly Unit _unit;
    
    public UnitTests()
    {
        var issuerAccountId = Guid.NewGuid();
        _issuerAccount = Account.Create(issuerAccountId, DateTime.Now);

        var unitTypeId = Guid.NewGuid();
        _unitType = UnitType.Create(unitTypeId, _issuerAccount);
        
        var validFrom = DateTime.Now;
        var validTo = validFrom.AddHours(1);
        var unitId = Guid.NewGuid();
        _unit = Unit.Create(unitId, validFrom, validTo, DateTime.Now,true, _unitType);
    }

    [Fact]
    public void IncreaseSupply_IncreasesSupplyByAmount_ReduceSupply_ReducesSupplyByAmount()
    {
        var amount = 1;
        var expectedIncreasedSupply = _unit.Supply + amount;
        
        _unit.IncreaseSupply(amount);
        _unit.Supply
            .Should()
            .Be(expectedIncreasedSupply);
        
        var expectedReducedSupply = expectedIncreasedSupply - amount;
        _unit.ReduceSupply(amount);
        _unit.Supply
            .Should()
            .Be(expectedReducedSupply);
    }
    
    [Fact]
    public void Create_WithValidToLessThenNow_ThrowsValidToIsLessThanCurrentDateTimeException()
    {
        var currentDateTime = DateTime.Now;
        var validFrom = currentDateTime.AddHours(-1);
        var validTo = currentDateTime.AddMinutes(-1);

        var unitId = Guid.NewGuid();
        var createUnitWithValidToLessThenNow = () => Unit.Create(unitId, validFrom, validTo, currentDateTime, true, _unitType);
        createUnitWithValidToLessThenNow
            .Should()
            .Throw<ValidToIsLessThanCurrentDateTimeException>();
    }
    
    [Fact]
    public void Create_WithValidToLessThenValidFrom_ThrowsValidFromIsGreaterThanValidToException()
    {
        var currentDateTime = DateTime.Now;
        var validTo = currentDateTime.AddMinutes(1);
        var validFrom = currentDateTime.AddHours(1);
        
        var unitId = Guid.NewGuid();
        var createUnitWithValidToLessThenThenValidFrom = () => Unit.Create(unitId, validFrom, validTo, currentDateTime, true, _unitType);
        createUnitWithValidToLessThenThenValidFrom
            .Should()
            .Throw<ValidFromIsGreaterThanValidToException>();
    }
    
    [Fact]
    public void IncreaseSupply_ByNotPositiveAmount_ThrowsNotPositiveAmountException()
    {
        var increaseSupplyByZeroAmount = () => _unit.IncreaseSupply(0);
        var increaseSupplyByNegativeAmount = () => _unit.IncreaseSupply(-1);

        increaseSupplyByZeroAmount
            .Should()
            .Throw<NotPositiveAmountException>();
        
        increaseSupplyByNegativeAmount
            .Should()
            .Throw<NotPositiveAmountException>();
    }
    
    [Fact]
    public void ReduceSupply_ByNotPositiveAmount_ThrowsNotPositiveAmountException()
    {
        var reduceSupplyByZeroAmount = () => _unit.ReduceSupply(0);
        var reduceSupplyByNegativeAmount = () => _unit.ReduceSupply(-1);

        reduceSupplyByZeroAmount
            .Should()
            .Throw<NotPositiveAmountException>();
        
        reduceSupplyByNegativeAmount
            .Should()
            .Throw<NotPositiveAmountException>();
    }
    
    [Fact]
    public void ReduceSupply_ByAmountGreaterThenSupply_ThrowsAmountIsGreaterThanSupplyException()
    {
        _unit.IncreaseSupply(1);
        
        var reduceSupplyByAmountGreaterThenSupply = () => _unit.ReduceSupply(_unit.Supply + 1);
        reduceSupplyByAmountGreaterThenSupply
            .Should()
            .Throw<AmountIsGreaterThanSupplyException>();
        
    }
}