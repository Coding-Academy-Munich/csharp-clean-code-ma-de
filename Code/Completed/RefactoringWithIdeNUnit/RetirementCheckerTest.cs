namespace RefactoringWithIdeNUnit;

[TestFixture]
public class RetirementCheckerTest
{
    [Test]
    public void IsEligibleForRetirement_RegularRetirementAge_ReturnsTrue()
    {
        Assert.That(RetirementChecker.IsEligibleForRetirement(65, 5), Is.True);
    }

    [Test]
    public void IsEligibleForRetirement_OlderThanRegularAge_ReturnsTrue()
    {
        Assert.That(RetirementChecker.IsEligibleForRetirement(70, 0), Is.True);
    }

    [Test]
    public void IsEligibleForRetirement_EarlyRetirement_ReturnsTrue()
    {
        Assert.That(RetirementChecker.IsEligibleForRetirement(55, 30), Is.True);
    }

    [Test]
    public void IsEligibleForRetirement_EarlyRetirementOlderWith30Years_ReturnsTrue()
    {
        Assert.That(RetirementChecker.IsEligibleForRetirement(60, 35), Is.True);
    }

    [Test]
    public void IsEligibleForRetirement_TooYoungNotEnoughService_ReturnsFalse()
    {
        Assert.That(RetirementChecker.IsEligibleForRetirement(50, 20), Is.False);
    }

    [Test]
    public void IsEligibleForRetirement_TooYoungEvenWith30Years_ReturnsFalse()
    {
        Assert.That(RetirementChecker.IsEligibleForRetirement(54, 30), Is.False);
    }

    [Test]
    public void IsEligibleForRetirement_OldEnoughButNotEnoughService_ReturnsFalse()
    {
        Assert.That(RetirementChecker.IsEligibleForRetirement(55, 29), Is.False);
    }
}
