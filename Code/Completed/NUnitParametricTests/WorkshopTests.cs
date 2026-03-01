namespace NUnitParametricTests;

[TestFixture]
public class PrimeTests
{
    [TestCase(1)]
    [TestCase(4)]
    [TestCase(6)]
    [TestCase(8)]
    [TestCase(9)]
    [TestCase(10)]
    [TestCase(12)]
    [TestCase(14)]
    [TestCase(15)]
    [TestCase(16)]
    [TestCase(18)]
    [TestCase(20)]
    public void TestNonPrimes(int n)
    {
        Assert.That(IsPrime(n), Is.False);
    }

    [TestCase(2)]
    [TestCase(3)]
    [TestCase(5)]
    [TestCase(7)]
    [TestCase(11)]
    [TestCase(13)]
    [TestCase(17)]
    [TestCase(19)]
    public void TestPrimes(int n)
    {
        Assert.That(IsPrime(n), Is.True);
    }
}

[TestFixture]
public class PalindromeTests
{
    [TestCase("")]
    [TestCase("a")]
    [TestCase("aa")]
    [TestCase("aba")]
    [TestCase("abba")]
    [TestCase("abcba")]
    public void TestPalindromes(string s)
    {
        Assert.That(IsPalindrome(s), Is.True);
    }

    [TestCase("ab")]
    [TestCase("abc")]
    [TestCase("abcd")]
    [TestCase("abcde")]
    public void TestNonPalindromes(string s)
    {
        Assert.That(IsPalindrome(s), Is.False);
    }
}

[TestFixture]
public class ContainsDigitTests
{
    [TestCase(123, 1, true)]
    [TestCase(123, 2, true)]
    [TestCase(123, 3, true)]
    [TestCase(123, 4, false)]
    [TestCase(123, 5, false)]
    [TestCase(123, 6, false)]
    public void TestContainsDigit(int n, int digit, bool expected)
    {
        Assert.That(ContainsDigit(n, digit), Is.EqualTo(expected));
    }
}

[TestFixture]
public class SubstringFollowingTests
{
    [TestCase("Hello", "He", "llo")]
    [TestCase("Hello", "Hel", "lo")]
    [TestCase("Hello", "Hello", "")]
    [TestCase("Hello", "el", "lo")]
    [TestCase("Hello", "ello", "")]
    [TestCase("Hello", "o", "")]
    [TestCase("Hello", "xyz", "Hello")]
    public void TestSubstringFollowing(string s, string prefix, string expected)
    {
        Assert.That(SubstringFollowing(s, prefix), Is.EqualTo(expected));
    }
}
