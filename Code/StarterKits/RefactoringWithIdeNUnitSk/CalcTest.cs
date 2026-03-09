namespace RefactoringWithIdeNUnitSk;

[TestFixture]
public class CalcTest
{
    private Calc _calc = null!;

    [SetUp]
    public void SetUp()
    {
        _calc = new Calc();
    }

    [Test]
    public void Calc1_EmptyList_ReturnsZero()
    {
        Assert.That(_calc.Calc1([]), Is.EqualTo(0));
    }

    [Test]
    public void Calc1_SingleValue_ReturnsThatValue()
    {
        Assert.That(_calc.Calc1([5.0]), Is.EqualTo(5.0));
    }

    [Test]
    public void Calc1_MultipleValues_ReturnsAverage()
    {
        Assert.That(_calc.Calc1([2.0, 4.0, 6.0]), Is.EqualTo(4.0));
    }

    [Test]
    public void Calc2_EmptyList_ReturnsZero()
    {
        Assert.That(_calc.Calc2([]), Is.EqualTo(0));
    }

    [Test]
    public void Calc2_SingleValue_ReturnsZero()
    {
        Assert.That(_calc.Calc2([5.0]), Is.EqualTo(0));
    }

    [Test]
    public void Calc2_IdenticalValues_ReturnsZero()
    {
        Assert.That(_calc.Calc2([3.0, 3.0, 3.0]), Is.EqualTo(0));
    }

    [Test]
    public void Calc2_KnownValues_ReturnsCorrectResult()
    {
        Assert.That(_calc.Calc2([2.0, 4.0, 6.0]), Is.EqualTo(4.0));
    }
}
