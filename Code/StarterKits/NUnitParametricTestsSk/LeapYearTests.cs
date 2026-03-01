namespace NUnitParametricTestsSk;

[TestFixture]
public class LeapYearTestsV1
{
    [Test]
    public void YearDivisibleBy4ButNot100IsLeapYear()
    {
        Assert.That(IsLeapYear(2004), Is.True);
    }

    [Test]
    public void YearDivisibleBy400IsLeapYear()
    {
        Assert.That(IsLeapYear(2000), Is.True);
    }

    [Test]
    public void YearsNotDivisibleBy4AreNotLeapYears()
    {
        Assert.That(IsLeapYear(2001), Is.False);
        Assert.That(IsLeapYear(2002), Is.False);
        Assert.That(IsLeapYear(2003), Is.False);
    }

    [Test]
    public void YearDivisibleBy100ButNot400IsNotLeapYear()
    {
        Assert.That(IsLeapYear(1900), Is.False);
    }
}

[TestFixture]
public class LeapYearTestsV2
{
    [TestCase(2004)]
    [TestCase(2008)]
    [TestCase(2012)]
    public void YearDivisibleBy4ButNot100IsLeapYear(int year)
    {
        Assert.That(IsLeapYear(year), Is.True);
    }

    [TestCase(2000)]
    public void YearDivisibleBy400IsLeapYear(int year)
    {
        Assert.That(IsLeapYear(year), Is.True);
    }

    [TestCase(2001)]
    [TestCase(2002)]
    [TestCase(2003)]
    public void YearsNotDivisibleBy4AreNotLeapYears(int year)
    {
        Assert.That(IsLeapYear(year), Is.False);
    }

    [TestCase(1900)]
    public void YearDivisibleBy100ButNot400IsNotLeapYear(int year)
    {
        Assert.That(IsLeapYear(year), Is.False);
    }
}

[TestFixture]
public class LeapYearTestsV3
{
    [TestCase(2001)]
    [TestCase(2002)]
    [TestCase(2003)]
    [TestCase(1900)]
    public void NonLeapYears(int year)
    {
        Assert.That(IsLeapYear(year), Is.False);
    }

    [TestCase(2004)]
    [TestCase(2000)]
    public void LeapYears(int year)
    {
        Assert.That(IsLeapYear(year), Is.True);
    }
}

[TestFixture]
public class LeapYearTestsV4
{
    [TestCase(2004, true)]
    [TestCase(2008, true)]
    [TestCase(2000, true)]
    [TestCase(2001, false)]
    [TestCase(2002, false)]
    [TestCase(2003, false)]
    [TestCase(1900, false)]
    public void IsLeapYearReturnsCorrectResult(int year, bool expected)
    {
        Assert.That(IsLeapYear(year), Is.EqualTo(expected));
    }
}

[TestFixture]
public class LeapYearTestsV5
{
    private static IEnumerable<object[]> LeapYearData()
    {
        yield return new object[] { 2004, true };
        yield return new object[] { 2008, true };
        yield return new object[] { 2000, true };
        yield return new object[] { 2001, false };
        yield return new object[] { 2002, false };
        yield return new object[] { 2003, false };
        yield return new object[] { 1900, false };
    }

    [TestCaseSource(nameof(LeapYearData))]
    public void IsLeapYearReturnsCorrectResult(int year, bool expected)
    {
        Assert.That(IsLeapYear(year), Is.EqualTo(expected));
    }
}

[TestFixture]
public class LeapYearTestsV6
{
    private static IEnumerable<TestCaseData> LeapYearData()
    {
        yield return new TestCaseData(2004).Returns(true)
            .SetName("2004 is a leap year (divisible by 4)");
        yield return new TestCaseData(2000).Returns(true)
            .SetName("2000 is a leap year (divisible by 400)");
        yield return new TestCaseData(1900).Returns(false)
            .SetName("1900 is not a leap year (divisible by 100)");
        yield return new TestCaseData(2001).Returns(false)
            .SetName("2001 is not a leap year (not divisible by 4)");
    }

    [TestCaseSource(nameof(LeapYearData))]
    public bool IsLeapYearFromSource(int year)
    {
        return IsLeapYear(year);
    }
}
