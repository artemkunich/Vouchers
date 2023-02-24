using FluentAssertions;
using Vouchers.Core.Domain.Exceptions;
using Xunit.Sdk;

namespace Vouchers.Core.Domain.UnitTests;

public class UnitQuantityTests
{
    private Unit _unit;
    
    public UnitQuantityTests()
    {
        var issuerAccountId = Guid.NewGuid();
        var issuerAccount = Account.Create(issuerAccountId, DateTime.Now);

        var unitTypeId = Guid.NewGuid();
        var unitType = UnitType.Create(unitTypeId, issuerAccount);
        
        var validFrom = DateTime.Now;
        var validTo = validFrom.AddHours(1);
        var unitId = Guid.NewGuid();
        _unit = Unit.Create(unitId, validFrom, validTo, DateTime.Now,true, unitType);
    }

    [Fact]
    public void Create_WithAmountAndUnitType_ReturnsUnitQuantityWithAmountAndUnitType()
    {
        var amount = 1;
        var unitQuantity = UnitQuantity.Create(amount, _unit);

        unitQuantity.Amount
            .Should()
            .Be(amount);

        unitQuantity.UnitId
            .Should()
            .Be(_unit.Id);
        
        unitQuantity.Unit
            .Should()
            .Be(_unit);
    }
    
    [Fact]
    public void Create_WithNotPositiveAmount_ThrowsNotPositiveAmountException()
    {
        var createUnitQuantityByZeroAmount = () => UnitQuantity.Create(0, _unit);
        var createUnitQuantityByNegativeAmount = () => UnitQuantity.Create(-1, _unit);

        createUnitQuantityByZeroAmount
            .Should()
            .Throw<NotPositiveAmountException>();
        
        createUnitQuantityByNegativeAmount
            .Should()
            .Throw<NotPositiveAmountException>();
    }
}