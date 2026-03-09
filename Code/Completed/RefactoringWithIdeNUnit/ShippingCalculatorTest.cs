namespace RefactoringWithIdeNUnit;

[TestFixture]
public class ShippingCalculatorTest
{
    private ShippingCalculator _calculator = null!;

    [SetUp]
    public void SetUp()
    {
        _calculator = new ShippingCalculator();
    }

    [Test]
    public void CalculateShippingCost_OrderAboveThreshold_ReturnsFreeShipping()
    {
        Assert.That(_calculator.CalculateShippingCost(150m, 3m), Is.EqualTo(0m));
    }

    [Test]
    public void CalculateShippingCost_OrderAtThreshold_ReturnsFreeShipping()
    {
        Assert.That(_calculator.CalculateShippingCost(100m, 10m), Is.EqualTo(0m));
    }

    [Test]
    public void CalculateShippingCost_LightPackageBelowThreshold_ReturnsBaseCost()
    {
        Assert.That(_calculator.CalculateShippingCost(50m, 3m), Is.EqualTo(4.99m));
    }

    [Test]
    public void CalculateShippingCost_HeavyPackageBelowThreshold_AddsWeightSurcharge()
    {
        // Base 4.99 + (8 - 5) * 1.50 = 4.99 + 4.50 = 9.49
        Assert.That(_calculator.CalculateShippingCost(50m, 8m), Is.EqualTo(9.49m));
    }

    [Test]
    public void CalculateShippingCost_CustomThreshold_UsesCustomValue()
    {
        Assert.That(
            _calculator.CalculateShippingCost(50m, 3m, freeShippingThreshold: 50m),
            Is.EqualTo(0m));
    }

    [Test]
    public void CalculateShippingCost_CustomRate_UsesCustomValue()
    {
        // Base 4.99 + (10 - 5) * 2.00 = 4.99 + 10.00 = 14.99
        Assert.That(
            _calculator.CalculateShippingCost(50m, 10m, ratePerKg: 2.00m),
            Is.EqualTo(14.99m));
    }
}
