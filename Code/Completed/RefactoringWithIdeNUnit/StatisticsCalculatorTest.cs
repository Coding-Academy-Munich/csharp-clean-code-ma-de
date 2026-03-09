namespace RefactoringWithIdeNUnit;

[TestFixture]
public class StatisticsCalculatorTest
{
    private StatisticsCalculator _calculator = null!;

    [SetUp]
    public void SetUp()
    {
        _calculator = new StatisticsCalculator();
    }

    [Test]
    public void CalculateMean_EmptyList_ReturnsZero()
    {
        Assert.That(_calculator.CalculateMean([]), Is.EqualTo(0));
    }

    [Test]
    public void CalculateMean_SingleValue_ReturnsThatValue()
    {
        Assert.That(_calculator.CalculateMean([5.0]), Is.EqualTo(5.0));
    }

    [Test]
    public void CalculateMean_MultipleValues_ReturnsAverage()
    {
        Assert.That(_calculator.CalculateMean([2.0, 4.0, 6.0]), Is.EqualTo(4.0));
    }

    [Test]
    public void CalculateVariance_EmptyList_ReturnsZero()
    {
        Assert.That(_calculator.CalculateVariance([]), Is.EqualTo(0));
    }

    [Test]
    public void CalculateVariance_SingleValue_ReturnsZero()
    {
        Assert.That(_calculator.CalculateVariance([5.0]), Is.EqualTo(0));
    }

    [Test]
    public void CalculateVariance_IdenticalValues_ReturnsZero()
    {
        Assert.That(_calculator.CalculateVariance([3.0, 3.0, 3.0]), Is.EqualTo(0));
    }

    [Test]
    public void CalculateVariance_KnownValues_ReturnsCorrectVariance()
    {
        // Values: 2, 4, 6. Mean = 4. Variance = ((4+0+4) / 2) = 4
        Assert.That(_calculator.CalculateVariance([2.0, 4.0, 6.0]), Is.EqualTo(4.0));
    }
}
