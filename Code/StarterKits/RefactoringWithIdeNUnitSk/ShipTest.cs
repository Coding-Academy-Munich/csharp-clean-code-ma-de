namespace RefactoringWithIdeNUnitSk;

[TestFixture]
public class ShipTest
{
    private Ship _ship = null!;

    [SetUp]
    public void SetUp()
    {
        _ship = new Ship();
    }

    [Test]
    public void Calc_OrderAboveThreshold_ReturnsFreeShipping()
    {
        Assert.That(_ship.Calc(150m, 3m), Is.EqualTo(0m));
    }

    [Test]
    public void Calc_OrderAtThreshold_ReturnsFreeShipping()
    {
        Assert.That(_ship.Calc(100m, 10m), Is.EqualTo(0m));
    }

    [Test]
    public void Calc_LightPackageBelowThreshold_ReturnsBaseCost()
    {
        Assert.That(_ship.Calc(50m, 3m), Is.EqualTo(4.99m));
    }

    [Test]
    public void Calc_HeavyPackageBelowThreshold_AddsWeightSurcharge()
    {
        Assert.That(_ship.Calc(50m, 8m), Is.EqualTo(9.49m));
    }
}
