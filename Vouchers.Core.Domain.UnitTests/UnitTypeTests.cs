using FluentAssertions;

namespace Vouchers.Core.Domain.UnitTests;

public class UnitTypeTests
{
    private readonly UnitType _unitType;
    
    public UnitTypeTests()
    {
        var issuerAccountId = Guid.NewGuid();
        var issuerAccount = Account.Create(issuerAccountId, DateTime.Now);

        var unitTypeId = Guid.NewGuid();
        _unitType = UnitType.Create(unitTypeId, issuerAccount);
    }

    [Fact]
    public void IncreaseSupply_IncreasesSupplyByAmount_ReduceSupply_ReducesSupplyByAmount()
    {
        var amount = 1;
        var expectedIncreasedSupply = _unitType.Supply + amount;
        
        _unitType.IncreaseSupply(amount);
        _unitType.Supply
            .Should()
            .Be(expectedIncreasedSupply);
        
        var expectedReducedSupply = expectedIncreasedSupply - amount;
        _unitType.ReduceSupply(amount);
        _unitType.Supply
            .Should()
            .Be(expectedReducedSupply);
    }
    
    [Fact]
    public void IncreaseSupply_ByNotPositiveAmount_ThrowsCoreException()
    {
        var increaseSupplyByZeroAmount = () => _unitType.IncreaseSupply(0);
        var increaseSupplyByNegativeAmount = () => _unitType.IncreaseSupply(-1);

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
        var reduceSupplyByZeroAmount = () => _unitType.IncreaseSupply(0);
        var reduceSupplyByNegativeAmount = () => _unitType.IncreaseSupply(-1);

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
        _unitType.IncreaseSupply(1);
        var amount = _unitType.Supply + 1;

        var reduceSupplyByAmountGreaterThenSupply = () => _unitType.ReduceSupply(amount);

        reduceSupplyByAmountGreaterThenSupply
            .Should()
            .Throw<CoreException>()
            .WithMessage(CoreException.AmountIsGreaterThanSupply.Message);

    }
}