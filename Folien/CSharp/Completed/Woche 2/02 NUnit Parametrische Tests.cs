// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>NUnit Parametrische Tests</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// ## Motivation: Schaltjahr-Funktion
//
// - Ein Schaltjahr ist etwas komplizierter als "durch 4 teilbar"
// - Genauer:
//   - Durch 4 teilbar: Schaltjahr
//   - Aber durch 100 teilbar: **kein** Schaltjahr
//   - Aber durch 400 teilbar: **doch** Schaltjahr

// %%
static bool IsLeapYear(int year) {
    return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
}

// %%
#r "nuget: NUnit, *"

// %%
#load "NunitTestRunner.cs"

// %%
using NUnit.Framework;

// %%
using static NunitTestRunner;

// %% [markdown]
//
// ## Problem: Wiederholter Test-Code
//
// - Mehrere Tests mit identischer Struktur
// - Nur die Testwerte unterscheiden sich
// - Viel syntaktischer Overhead

// %%
[TestFixture]
public class LeapYearTestsV1 {
    [Test]
    public void YearDivisibleBy4ButNot100IsLeapYear() {
        Assert.That(IsLeapYear(2004), Is.True);
    }

    [Test]
    public void YearDivisibleBy400IsLeapYear() {
        Assert.That(IsLeapYear(2000), Is.True);
    }

    [Test]
    public void YearsNotDivisibleBy4AreNotLeapYears() {
        Assert.That(IsLeapYear(2001), Is.False);
        Assert.That(IsLeapYear(2002), Is.False);
        Assert.That(IsLeapYear(2003), Is.False);
    }

    [Test]
    public void YearDivisibleBy100ButNot400IsNotLeapYear() {
        Assert.That(IsLeapYear(1900), Is.False);
    }
}

// %%
RunTests<LeapYearTestsV1>();

// %% [markdown]
//
// ## `[TestCase]`-Attribut
//
// - Ersetzt wiederholte `[Test]`-Methoden durch eine parametrisierte Methode
// - Jedes `[TestCase]`-Attribut liefert einen Satz von Testdaten
// - Der Test wird fuer jeden `[TestCase]` einzeln ausgefuehrt
// - Parameter der Methode muessen zum `[TestCase]` passen

// %%
[TestFixture]
public class LeapYearTestsV2 {
    [TestCase(2004)]
    [TestCase(2008)]
    [TestCase(2012)]
    public void YearDivisibleBy4ButNot100IsLeapYear(int year) {
        Assert.That(IsLeapYear(year), Is.True);
    }

    [TestCase(2000)]
    public void YearDivisibleBy400IsLeapYear(int year) {
        Assert.That(IsLeapYear(year), Is.True);
    }

    [TestCase(2001)]
    [TestCase(2002)]
    [TestCase(2003)]
    public void YearsNotDivisibleBy4AreNotLeapYears(int year) {
        Assert.That(IsLeapYear(year), Is.False);
    }

    [TestCase(1900)]
    public void YearDivisibleBy100ButNot400IsNotLeapYear(int year) {
        Assert.That(IsLeapYear(year), Is.False);
    }
}

// %%
RunTests<LeapYearTestsV2>();

// %% [markdown]
//
// ## Kompaktere Version
//
// - Alle Tests haben die gleiche Struktur
// - Wir koennen sie zu zwei parametrisierten Tests zusammenfassen
// - Vorteil: Kompakter Code
// - Nachteil: Weniger informative Testnamen

// %%
[TestFixture]
public class LeapYearTestsV3 {
    [TestCase(2001)]
    [TestCase(2002)]
    [TestCase(2003)]
    [TestCase(1900)]
    public void NonLeapYears(int year) {
        Assert.That(IsLeapYear(year), Is.False);
    }

    [TestCase(2004)]
    [TestCase(2000)]
    public void LeapYears(int year) {
        Assert.That(IsLeapYear(year), Is.True);
    }
}

// %%
RunTests<LeapYearTestsV3>();

// %% [markdown]
//
// ## `[TestCase]` mit mehreren Parametern
//
// - `[TestCase]` kann mehrere Argumente enthalten
// - Argumente werden den Methodenparametern der Reihe nach zugeordnet
// - Ermoeglicht Kombination von Eingabe und erwartetem Ergebnis

// %%
[TestFixture]
public class LeapYearTestsV4 {
    [TestCase(2004, true)]
    [TestCase(2008, true)]
    [TestCase(2000, true)]
    [TestCase(2001, false)]
    [TestCase(2002, false)]
    [TestCase(2003, false)]
    [TestCase(1900, false)]
    public void IsLeapYearReturnsCorrectResult(int year, bool expected) {
        Assert.That(IsLeapYear(year), Is.EqualTo(expected));
    }
}

// %%
RunTests<LeapYearTestsV4>();

// %% [markdown]
//
// ## `[TestCaseSource]`
//
// - `[TestCaseSource]` liefert Testdaten aus einer Methode oder Property
// - Methodenname wird mit `nameof()` uebergeben
// - Methode muss `static` sein
// - Gibt `IEnumerable` von Testdaten zurueck
// - Mehr Flexibilitaet als `[TestCase]`

// %%
using System;
using System.Collections.Generic;

// %% [markdown]
//
// ### `[TestCaseSource]` mit `object[]`
//
// - Factory-Methode gibt `IEnumerable<object[]>` zurueck
// - Jedes `object[]` enthaelt die Argumente fuer einen Testlauf
// - Aehnlich wie `[MemberData]` in xUnit

// %%
[TestFixture]
public class LeapYearTestsV5 {
    private static IEnumerable<object[]> LeapYearData() {
        yield return new object[] { 2004, true };
        yield return new object[] { 2008, true };
        yield return new object[] { 2000, true };
        yield return new object[] { 2001, false };
        yield return new object[] { 2002, false };
        yield return new object[] { 2003, false };
        yield return new object[] { 1900, false };
    }

    [TestCaseSource(nameof(LeapYearData))]
    public void IsLeapYearReturnsCorrectResult(int year, bool expected) {
        Assert.That(IsLeapYear(year), Is.EqualTo(expected));
    }
}

// %% [markdown]
//
// ### `[TestCaseSource]` mit `TestCaseData`
//
// - `TestCaseData` erlaubt mehr Kontrolle ueber Testfaelle
// - Testnamen koennen individuell vergeben werden
// - Erwartete Ergebnisse koennen mit `.Returns()` angegeben werden

// %%
[TestFixture]
public class LeapYearTestsV6 {
    private static IEnumerable<TestCaseData> LeapYearData() {
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
    public bool IsLeapYearFromSource(int year) {
        return IsLeapYear(year);
    }
}

// %% [markdown]
//
// ## Vergleich: xUnit vs. NUnit
//
// | Feature | xUnit | NUnit |
// |---------|-------|-------|
// | Einfacher Test | `[Fact]` | `[Test]` |
// | Inline-Daten | `[Theory]` + `[InlineData]` | `[TestCase]` |
// | Factory-Methode | `[Theory]` + `[MemberData]` | `[TestCaseSource]` |
// | Assertions | `Assert.True/False/Equal` | `Assert.That(..., Is.*)` |

// %% [markdown]
//
// ## Workshop: Parametrisierte Tests (NUnit)
//
// - Schreiben Sie parametrisierte Tests fuer die folgenden Funktionen.
// - Verwenden Sie dabei `[TestCase]` mindestens einmal.
// - Verwenden Sie das NUnit Constraint-Modell (`Assert.That(..., Is.*)`)
//   fuer Assertions.

// %%
static bool IsPrime(int n) {
    if (n <= 1) {
        return false;
    }
    for (int i = 2; i <= Math.Sqrt(n); i++) {
        if (n % i == 0) {
            return false;
        }
    }
    return true;
}

// %%
[TestFixture]
public class PrimeTests {
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
    public void TestNonPrimes(int n) {
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
    public void TestPrimes(int n) {
        Assert.That(IsPrime(n), Is.True);
    }
}

// %%
RunTests<PrimeTests>();

// %%
static bool IsPalindrome(string s) {
    return s.Equals(new string(s.Reverse().ToArray()));
}

// %%
[TestFixture]
public class PalindromeTests {
    [TestCase("")]
    [TestCase("a")]
    [TestCase("aa")]
    [TestCase("aba")]
    [TestCase("abba")]
    [TestCase("abcba")]
    public void TestPalindromes(string s) {
        Assert.That(IsPalindrome(s), Is.True);
    }

    [TestCase("ab")]
    [TestCase("abc")]
    [TestCase("abcd")]
    [TestCase("abcde")]
    public void TestNonPalindromes(string s) {
        Assert.That(IsPalindrome(s), Is.False);
    }
}

// %%
RunTests<PalindromeTests>();

// %%
static bool ContainsDigit(int n, int digit) {
    return n.ToString().Contains(digit.ToString());
}

// %%
[TestFixture]
public class ContainsDigitTests {
    [TestCase(123, 1, true)]
    [TestCase(123, 2, true)]
    [TestCase(123, 3, true)]
    [TestCase(123, 4, false)]
    [TestCase(123, 5, false)]
    [TestCase(123, 6, false)]
    public void TestContainsDigit(int n, int digit, bool expected) {
        Assert.That(ContainsDigit(n, digit), Is.EqualTo(expected));
    }
}

// %%
RunTests<ContainsDigitTests>();

// %%
static string SubstringFollowing(string s, string prefix) {
    int index = s.IndexOf(prefix);
    if (index == -1) {
        return s;
    }
    return s.Substring(index + prefix.Length);
}

// %%
[TestFixture]
public class SubstringFollowingTests {
    [TestCase("Hello", "He", "llo")]
    [TestCase("Hello", "Hel", "lo")]
    [TestCase("Hello", "Hello", "")]
    [TestCase("Hello", "ello", "Hello")]
    [TestCase("Hello", "o", "Hello")]
    public void TestSubstringFollowing(string s, string prefix, string expected) {
        Assert.That(SubstringFollowing(s, prefix), Is.EqualTo(expected));
    }
}

// %%
RunTests<SubstringFollowingTests>();
