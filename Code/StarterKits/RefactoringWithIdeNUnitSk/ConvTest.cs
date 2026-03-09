namespace RefactoringWithIdeNUnitSk;

[TestFixture]
public class ConvTest
{
    private Conv _conv = null!;

    [SetUp]
    public void SetUp()
    {
        _conv = new Conv();
    }

    [Test]
    public void C2f_FreezingPoint_Returns32()
    {
        Assert.That(_conv.C2f(0), Is.EqualTo(32.0));
    }

    [Test]
    public void C2f_BoilingPoint_Returns212()
    {
        Assert.That(_conv.C2f(100), Is.EqualTo(212.0));
    }

    [Test]
    public void F2c_FreezingPoint_ReturnsZero()
    {
        Assert.That(_conv.F2c(32), Is.EqualTo(0.0));
    }

    [Test]
    public void F2c_BoilingPoint_Returns100()
    {
        Assert.That(_conv.F2c(212), Is.EqualTo(100.0));
    }

    [Test]
    public void F2c_BodyTemperature_RoundsToTwoDecimals()
    {
        Assert.That(_conv.F2c(98.6), Is.EqualTo(37.0));
    }

    [Test]
    public void C2k_AbsoluteZero_Returns0()
    {
        Assert.That(_conv.C2k(-273.15), Is.EqualTo(0.0));
    }

    [Test]
    public void C2k_FreezingPoint_Returns273Point15()
    {
        Assert.That(_conv.C2k(0), Is.EqualTo(273.15));
    }
}
