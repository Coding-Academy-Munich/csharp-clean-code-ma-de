namespace RefactoringWithIdeNUnit;

[TestFixture]
public class BonusCalculatorTest
{
    private BonusCalculator _calculator = null!;

    [SetUp]
    public void SetUp()
    {
        _calculator = new BonusCalculator();
    }

    [Test]
    public void CalculateAverageBonus_NoEmployees_ReturnsZero()
    {
        Assert.That(_calculator.CalculateAverageBonus(), Is.EqualTo(0));
    }

    [Test]
    public void CalculateAverageBonus_EmployeeWithTooFewYears_ReturnsZero()
    {
        _calculator.AddEmployee("Alice", 50000, 1);

        Assert.That(_calculator.CalculateAverageBonus(), Is.EqualTo(0));
    }

    [Test]
    public void CalculateAverageBonus_EmployeeWithSalaryTooLow_ReturnsZero()
    {
        _calculator.AddEmployee("Bob", 30000, 5);

        Assert.That(_calculator.CalculateAverageBonus(), Is.EqualTo(0));
    }

    [Test]
    public void CalculateAverageBonus_EmployeeWithSalaryTooHigh_ReturnsZero()
    {
        _calculator.AddEmployee("Charlie", 200000, 5);

        Assert.That(_calculator.CalculateAverageBonus(), Is.EqualTo(0));
    }

    [Test]
    public void CalculateAverageBonus_EligibleEmployeeUpTo5Years_Returns5PercentBonus()
    {
        _calculator.AddEmployee("Dana", 60000, 3);

        Assert.That(_calculator.CalculateAverageBonus(), Is.EqualTo(3000));
    }

    [Test]
    public void CalculateAverageBonus_EligibleEmployee6To10Years_Returns10PercentBonus()
    {
        _calculator.AddEmployee("Eve", 80000, 7);

        Assert.That(_calculator.CalculateAverageBonus(), Is.EqualTo(8000));
    }

    [Test]
    public void CalculateAverageBonus_EligibleEmployeeOver10Years_Returns15PercentBonus()
    {
        _calculator.AddEmployee("Frank", 100000, 15);

        Assert.That(_calculator.CalculateAverageBonus(), Is.EqualTo(15000));
    }

    [Test]
    public void CalculateAverageBonus_MixedEligibility_AveragesOnlyEligible()
    {
        _calculator.AddEmployee("Eligible1", 60000, 3);   // 5% = 3000
        _calculator.AddEmployee("Ineligible", 50000, 1);  // not eligible
        _calculator.AddEmployee("Eligible2", 80000, 7);   // 10% = 8000

        Assert.That(_calculator.CalculateAverageBonus(), Is.EqualTo(5500));
    }
}
