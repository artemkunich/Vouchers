using FluentAssertions;
using Vouchers.Core.Domain.Exceptions;

namespace Vouchers.Core.Domain.UnitTests;

public class UnitTypeQuantityTests
{
    private readonly Account _issuerAccount;
    private readonly UnitType _unitType;
    private readonly UnitTypeQuantity _unitTypeQuantity;
    
    public UnitTypeQuantityTests()
    {
        var issuerAccountId = Guid.NewGuid();
        _issuerAccount = Account.Create(issuerAccountId, DateTime.Now);

        var unitTypeId = Guid.NewGuid();
        _unitType = UnitType.Create(unitTypeId, _issuerAccount);
        
        _unitTypeQuantity = UnitTypeQuantity.Create(0, _unitType);
    }

    [Fact]
    public void Create_WithNegativeAmount_ThrowsNegativeAmountException()
    {
        var createUnitQuantityWithNegativeAmount = () => UnitTypeQuantity.Create(-1, _unitType);
        createUnitQuantityWithNegativeAmount
            .Should()
            .Throw<NegativeAmountException>();
    }

    [Fact]
    public void Add_UnitQuantityWithAnotherUnitType_ThrowsDifferentUnitTypesException()
    {
        var anotherUnitTypeId = Guid.NewGuid();
        var anotherUnitType = UnitType.Create(anotherUnitTypeId, _issuerAccount);

        var anotherUnitValidFrom = DateTime.Now;
        var anotherUnitValidTo = anotherUnitValidFrom.AddHours(1);
        var anotherUnitId = Guid.NewGuid();
        var anotherUnit = Unit.Create(anotherUnitId, anotherUnitValidFrom, anotherUnitValidTo, DateTime.Now, true, anotherUnitType);
        var anotherUnitQuantity = UnitQuantity.Create(1, anotherUnit);

        var addWithDifferentUnitType = () => _unitTypeQuantity.Add(anotherUnitQuantity);
        addWithDifferentUnitType
            .Should()
            .Throw<DifferentUnitTypesException>();
    }
    
    [Fact]
    public void Add_UnitQuantity_ReturnsUnitTypeQuantityWithIncreasedAmount()
    {
        var unitValidFrom = DateTime.Now;
        var unitValidTo = unitValidFrom.AddHours(1);
        var unitId = Guid.NewGuid();
        var unit = Unit.Create(unitId, unitValidFrom, unitValidTo, DateTime.Now, true, _unitType);
        var unitQuantity = UnitQuantity.Create(1, unit);

        var expectedNewUnitTypeQuantityAmount = _unitTypeQuantity.Amount + unitQuantity.Amount;
        
        var newUnitTypeQuantity = _unitTypeQuantity.Add(unitQuantity);

        newUnitTypeQuantity.Amount
            .Should()
            .Be(expectedNewUnitTypeQuantityAmount);
        
    }
}