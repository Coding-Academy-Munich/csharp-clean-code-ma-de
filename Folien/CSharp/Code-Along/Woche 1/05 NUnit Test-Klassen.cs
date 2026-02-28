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

// %%

// %%

// %%

// %%
#load "NUnitTestRunner.cs"

// %%

// %%

// %%

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
    public void TestTest()
    {
        Assert.That(true, Is.True);
    }
}

// %% [markdown]
//
// - Mit dem folgenden Code können Sie die Tests ausführen.

// %%
RunTests(typeof(SimpleMathTest));
