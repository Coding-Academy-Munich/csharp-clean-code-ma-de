namespace RefactoringWithIdeNUnitSk;

[TestFixture]
public class MgrTest
{
    private Mgr _mgr = null!;

    [SetUp]
    public void SetUp()
    {
        _mgr = new Mgr();
    }

    [Test]
    public void Proc_NoEmployees_ReturnsZero()
    {
        Assert.That(_mgr.Proc(), Is.EqualTo(0));
    }

    [Test]
    public void Proc_EmployeeWithTooFewYears_ReturnsZero()
    {
        _mgr.Add("Alice", 50000, 1);

        Assert.That(_mgr.Proc(), Is.EqualTo(0));
    }

    [Test]
    public void Proc_EmployeeWithSalaryTooLow_ReturnsZero()
    {
        _mgr.Add("Bob", 30000, 5);

        Assert.That(_mgr.Proc(), Is.EqualTo(0));
    }

    [Test]
    public void Proc_EmployeeWithSalaryTooHigh_ReturnsZero()
    {
        _mgr.Add("Charlie", 200000, 5);

        Assert.That(_mgr.Proc(), Is.EqualTo(0));
    }

    [Test]
    public void Proc_EligibleEmployeeUpTo5Years_Returns5PercentBonus()
    {
        _mgr.Add("Dana", 60000, 3);

        Assert.That(_mgr.Proc(), Is.EqualTo(3000));
    }

    [Test]
    public void Proc_EligibleEmployee6To10Years_Returns10PercentBonus()
    {
        _mgr.Add("Eve", 80000, 7);

        Assert.That(_mgr.Proc(), Is.EqualTo(8000));
    }

    [Test]
    public void Proc_EligibleEmployeeOver10Years_Returns15PercentBonus()
    {
        _mgr.Add("Frank", 100000, 15);

        Assert.That(_mgr.Proc(), Is.EqualTo(15000));
    }

    [Test]
    public void Proc_MixedEligibility_AveragesOnlyEligible()
    {
        _mgr.Add("Eligible1", 60000, 3);   // 5% = 3000
        _mgr.Add("Ineligible", 50000, 1);  // not eligible
        _mgr.Add("Eligible2", 80000, 7);   // 10% = 8000

        Assert.That(_mgr.Proc(), Is.EqualTo(5500));
    }
}
