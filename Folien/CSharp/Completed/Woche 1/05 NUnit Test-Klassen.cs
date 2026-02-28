// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>NUnit: Test-Klassen</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// ## Test-Klassen
//
// - Tests werden in Klassen organisiert
// - `[TestFixture]`-Attribut an der Klasse (optional, aber empfohlen)
// - `[Test]`-Attribut an Methoden um Tests zu definieren
// - Assertions wie oben besprochen

// %%
#r "nuget: NUnit, *"

// %%
using NUnit.Framework;

// %%
[TestFixture]
public class NUnitBasicsTest
{
    [Test]
    public void TestAddition()
    {
        Assert.That(2 + 2, Is.EqualTo(4));
    }

    [Test]
    public void TestFailure()
    {
        Assert.That(2, Is.EqualTo(1));
    }
}

// %%
var tests = new NUnitBasicsTest();

// %%
tests.TestAddition();

// %%
// tests.TestFailure();

// %%
#load "NUnitTestRunner.cs"

// %%
using static NUnitTestRunner;

// %%
RunTests<NUnitBasicsTest>();

// %%
RunTests(typeof(NUnitBasicsTest));

// %% [markdown]
//
// ## Workshop: NUnit Basics im Notebook
//
// In diesem Workshop sollen Sie eine einfache Testklasse schreiben und die
// Tests mit NUnit ausführen.
//
// Hier ist der Code, den Sie testen sollen:

// %%
public class SimpleMath
{
    public int Add(int a, int b)
    {
        return a + b;
    }

    public int Subtract(int a, int b)
    {
        return a - b;
    }

    public int Multiply(int a, int b)
    {
        return a * b;
    }

    public int Divide(int a, int b)
    {
        return a / b;
    }
}

// %% [markdown]
//
// - Schreiben Sie Tests, die die Methoden der Klasse `SimpleMath` überprüfen.
// - Sie können dabei die folgende Klasse `SimpleMathTest` erweitern.

// %%
[TestFixture]
public class SimpleMathTest
{
    [Test]
    public void TestAddition()
    {
        SimpleMath math = new SimpleMath();
        Assert.That(math.Add(2, 2), Is.EqualTo(4));
    }

    [Test]
    public void TestSubtraction()
    {
        SimpleMath math = new SimpleMath();
        Assert.That(math.Subtract(2, 2), Is.EqualTo(0));
    }

    [Test]
    public void TestMultiplication()
    {
        SimpleMath math = new SimpleMath();
        Assert.That(math.Multiply(2, 3), Is.EqualTo(6));
    }

    [Test]
    public void TestDivision()
    {
        SimpleMath math = new SimpleMath();
        Assert.That(math.Divide(6, 3), Is.EqualTo(2));
    }

    [Test]
    public void TestDivisionByZero()
    {
        SimpleMath math = new SimpleMath();
        Assert.Throws<DivideByZeroException>(() => math.Divide(1, 0));
    }
}

// %% [markdown]
//
// - Mit dem folgenden Code können Sie die Tests ausführen.

// %%
RunTests(typeof(SimpleMathTest));
