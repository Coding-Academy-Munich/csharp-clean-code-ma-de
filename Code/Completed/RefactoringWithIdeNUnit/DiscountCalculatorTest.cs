namespace RefactoringWithIdeNUnit;

[TestFixture]
public class DiscountCalculatorTest
{
    private DiscountCalculator _calculator = null!;

    [SetUp]
    public void SetUp()
    {
        _calculator = new DiscountCalculator();
    }

    [Test]
    public void CalculateTotal_SmallOrder_NoDiscount()
    {
        Assert.That(_calculator.CalculateTotal(10m, 5), Is.EqualTo(50m));
    }

    [Test]
    public void CalculateTotal_ExactThreshold_NoDiscount()
    {
        Assert.That(_calculator.CalculateTotal(50m, 2), Is.EqualTo(100m));
    }

    [Test]
    public void CalculateTotal_AboveThreshold_AppliesDiscount()
    {
        Assert.That(_calculator.CalculateTotal(50m, 3), Is.EqualTo(140m));
    }

    [Test]
    public void CalculateTotal_LargeOrder_AppliesDiscount()
    {
        Assert.That(_calculator.CalculateTotal(100m, 2), Is.EqualTo(190m));
    }

    [Test]
    public void CalculateTotal_SingleItem_NoDiscount()
    {
        Assert.That(_calculator.CalculateTotal(99m, 1), Is.EqualTo(99m));
    }

    [Test]
    public void CalculateTotal_FractionalPrice_RoundsToTwoDecimals()
    {
        Assert.That(_calculator.CalculateTotal(33.33m, 1), Is.EqualTo(33.33m));
    }

    [Test]
    public void CalculateTotal_FractionalAboveThreshold_RoundsToTwoDecimals()
    {
        Assert.That(_calculator.CalculateTotal(33.337m, 4), Is.EqualTo(123.35m));
    }

    [Test]
    public void CalculateTotal_CustomThreshold_AppliesDiscount()
    {
        Assert.That(
            _calculator.CalculateTotal(25m, 3, discountThreshold: 50m),
            Is.EqualTo(65m));
    }

    [Test]
    public void CalculateTotal_CustomDecimalPlaces_RoundsCorrectly()
    {
        Assert.That(
            _calculator.CalculateTotal(33.337m, 4, decimalPlaces: 1),
            Is.EqualTo(123.3m));
    }
}
