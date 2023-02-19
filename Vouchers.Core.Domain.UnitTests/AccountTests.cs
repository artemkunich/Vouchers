using FluentAssertions;

namespace Vouchers.Core.Domain.UnitTests;

public class AccountTests
{
    private readonly Account _account;
    
    public AccountTests()
    {
        var id = Guid.NewGuid();
        var date = DateTime.Now;
        _account = Account.Create(id, date);
    }

    [Fact]
    public void IncreaseSupply_IncreasesSupplyByAmount_ReduceSupply_ReducesSupplyByAmount()
    {
        var amount = 1;
        var expectedIncreasedSupply = _account.Supply + amount;
        
        _account.IncreaseSupply(amount);
        _account.Supply
            .Should()
            .Be(expectedIncreasedSupply);
        
        var expectedReducedSupply = expectedIncreasedSupply - amount;
        _account.ReduceSupply(amount);
        _account.Supply
            .Should()
            .Be(expectedReducedSupply);
    }
    
    [Fact]
    public void IncreaseSupply_ByNotPositiveAmount_ThrowsCoreException()
    {
        var increaseSupplyByZeroAmount = () => _account.IncreaseSupply(0);
        var increaseSupplyByNegativeAmount = () => _account.IncreaseSupply(-1);
        
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
        var reduceSupplyByZeroAmount = () => _account.ReduceSupply(0);
        var reduceSupplyByNegativeAmount = () => _account.ReduceSupply(-1);
        
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
        _account.IncreaseSupply(1);
        
        var reduceSupplyByAmountGreaterThenSupply = () => _account.ReduceSupply(_account.Supply + 1);
        reduceSupplyByAmountGreaterThenSupply
            .Should()
            .Throw<CoreException>()
            .WithMessage(CoreException.AmountIsGreaterThanSupply.Message);
    }
}