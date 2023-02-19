using FluentAssertions;

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
    public void Create_WithValidToLessThenNow_ThrowsCoreException()
    {
        var currentDateTime = DateTime.Now;
        var validFrom = currentDateTime.AddHours(-1);
        var validTo = currentDateTime.AddMinutes(-1);

        var unitId = Guid.NewGuid();
        var createUnitWithValidToLessThenNow = () => Unit.Create(unitId, validFrom, validTo, currentDateTime, true, _unitType);
        createUnitWithValidToLessThenNow
            .Should()
            .Throw<CoreException>()
            .WithMessage(CoreException.ValidToIsLessThanToday.Message);
    }
    
    [Fact]
    public void Create_WithValidToLessThenValidFrom_ThrowsCoreException()
    {
        var currentDateTime = DateTime.Now;
        var validTo = currentDateTime.AddMinutes(1);
        var validFrom = currentDateTime.AddHours(1);
        
        var unitId = Guid.NewGuid();
        var createUnitWithValidToLessThenThenValidFrom = () => Unit.Create(unitId, validFrom, validTo, currentDateTime, true, _unitType);
        createUnitWithValidToLessThenThenValidFrom
            .Should()
            .Throw<CoreException>()
            .WithMessage(CoreException.ValidFromIsGreaterThanValidTo.Message);
    }
    
    [Fact]
    public void IncreaseSupply_ByNotPositiveAmount_ThrowsCoreException()
    {
        var increaseSupplyByZeroAmount = () => _unit.IncreaseSupply(0);
        var increaseSupplyByNegativeAmount = () => _unit.IncreaseSupply(-1);

        increaseSupplyByZeroAmount
            .Should()
            .Throw<CoreException>()
            .WithMessage(CoreException.AmountIsNotPositive.Message);
        
        increaseSupplyByNegativeAmount
            .Should()
            .Throw<CoreException>()
            .WithMessage(CoreException.AmountIsNotPositive.Message);
    }
    
    [Fact]
    public void ReduceSupply_ByNotPositiveAmount_ThrowsCoreException()
    {
        var reduceSupplyByZeroAmount = () => _unit.ReduceSupply(0);
        var reduceSupplyByNegativeAmount = () => _unit.ReduceSupply(-1);

        reduceSupplyByZeroAmount
            .Should()
            .Throw<CoreException>()
            .WithMessage(CoreException.AmountIsNotPositive.Message);
        
        reduceSupplyByNegativeAmount
            .Should()
            .Throw<CoreException>()
            .WithMessage(CoreException.AmountIsNotPositive.Message);
    }
    
    [Fact]
    public void ReduceSupply_ByAmountGreaterThenSupply_ThrowsCoreException()
    {
        _unit.IncreaseSupply(1);
        
        var reduceSupplyByAmountGreaterThenSupply = () => _unit.ReduceSupply(_unit.Supply + 1);
        reduceSupplyByAmountGreaterThenSupply
            .Should()
            .Throw<CoreException>()
            .WithMessage(CoreException.AmountIsGreaterThanSupply.Message);
        
    }
}