namespace RefactoringWithIdeNUnitSk;

[TestFixture]
public class DisTest
{
    private Dis _dis = null!;

    [SetUp]
    public void SetUp()
    {
        _dis = new Dis();
    }

    [Test]
    public void Calc_SmallOrder_NoDiscount()
    {
        Assert.That(_dis.Calc(10m, 5), Is.EqualTo(50m));
    }

    [Test]
    public void Calc_ExactThreshold_NoDiscount()
    {
        Assert.That(_dis.Calc(50m, 2), Is.EqualTo(100m));
    }

    [Test]
    public void Calc_AboveThreshold_AppliesDiscount()
    {
        Assert.That(_dis.Calc(50m, 3), Is.EqualTo(140m));
    }

    [Test]
    public void Calc_LargeOrder_AppliesDiscount()
    {
        Assert.That(_dis.Calc(100m, 2), Is.EqualTo(190m));
    }

    [Test]
    public void Calc_SingleItem_NoDiscount()
    {
        Assert.That(_dis.Calc(99m, 1), Is.EqualTo(99m));
    }

    [Test]
    public void Calc_FractionalPrice_RoundsToTwoDecimals()
    {
        Assert.That(_dis.Calc(33.33m, 1), Is.EqualTo(33.33m));
    }

    [Test]
    public void Calc_FractionalAboveThreshold_RoundsToTwoDecimals()
    {
        Assert.That(_dis.Calc(33.337m, 4), Is.EqualTo(123.35m));
    }
}
