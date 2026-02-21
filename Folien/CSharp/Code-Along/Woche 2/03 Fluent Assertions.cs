// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Fluent Assertions</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// ## Was ist FluentAssertions?
//
// - Bibliothek fuer lesbare Test-Assertions in C#
// - Verwendet eine fluente API mit `Should()`-Syntax
// - Bessere Fehlermeldungen als Standard-NUnit-Assertions
// - Unterstuetzt Strings, Zahlen, Collections, Exceptions, ...
// - NuGet-Paket: `FluentAssertions`

// %%
#r "nuget: NUnit, *"
#r "nuget: FluentAssertions, 6.12.0"

// %%
using NUnit.Framework;
using FluentAssertions;

// %%
#load "NunitTestRunner.cs"

// %% [markdown]
//
// ## Vergleich: NUnit vs. FluentAssertions

// %% [markdown]
//
// ### NUnit-Style

// %%
Assert.That(42, Is.EqualTo(42));
Assert.That(42, Is.Not.EqualTo(0));
Assert.That(10 > 5, Is.True);

// %% [markdown]
//
// ### FluentAssertions-Style

// %%
42.Should().Be(42);
42.Should().NotBe(0);
(10 > 5).Should().BeTrue();

// %% [markdown]
//
// ## Numerische Assertions
//
// - `Be()`, `NotBe()` -- Gleichheit
// - `BeGreaterThan()`, `BeLessThan()` -- Vergleiche
// - `BeInRange()` -- Wertebereich
// - `BeApproximately()` -- Gleitkommazahlen

// %%
int value = 42;

value.Should().BeGreaterThan(0);
value.Should().BeLessThanOrEqualTo(100);
value.Should().BeInRange(1, 50);

// %%
double pi = 3.14159;

pi.Should().BeApproximately(3.14, 0.01);

// %% [markdown]
//
// ## String-Assertions
//
// - `Be()`, `NotBe()` -- Gleichheit
// - `Contain()`, `StartWith()`, `EndWith()` -- Teilstrings
// - `BeEmpty()`, `NotBeNullOrEmpty()` -- Leer/Null
// - `HaveLength()`, `MatchRegex()` -- Laenge und Pattern

// %%
string greeting = "Hello, World!";

greeting.Should().StartWith("Hello");
greeting.Should().EndWith("!");
greeting.Should().Contain("World");
greeting.Should().HaveLength(13);

// %%
string name = "Alice";

name.Should().NotBeNullOrEmpty();
name.Should().NotBe("Bob");

// %% [markdown]
//
// ## Collection-Assertions
//
// - `HaveCount()` -- Anzahl der Elemente
// - `Contain()`, `NotContain()` -- Elemente pruefen
// - `BeInAscendingOrder()` -- Sortierung
// - `OnlyContain()` -- Bedingung fuer alle Elemente
// - `BeEquivalentTo()` -- gleiche Elemente, beliebige Reihenfolge

// %%
using System.Collections.Generic;

// %%
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

numbers.Should().HaveCount(5);
numbers.Should().Contain(3);
numbers.Should().NotContain(42);
numbers.Should().BeInAscendingOrder();
numbers.Should().OnlyContain(n => n > 0);

// %%
List<string> fruits = new List<string> { "Apple", "Banana", "Cherry" };

fruits.Should().HaveCount(3);
fruits.Should().Contain("Banana");
fruits.Should().BeEquivalentTo(new[] { "Cherry", "Apple", "Banana" });

// %% [markdown]
//
// ## Exception-Assertions
//
// - `action.Should().Throw<T>()` -- Exception erwartet
// - `action.Should().NotThrow()` -- keine Exception erwartet
// - `.WithMessage()` -- Fehlermeldung pruefen

// %%
using System;

// %%
Action act = () => throw new InvalidOperationException("Something went wrong");

act.Should().Throw<InvalidOperationException>()
   .WithMessage("*went wrong*");

// %%
Action safeAct = () => { int x = 1 + 1; };

safeAct.Should().NotThrow();

// %% [markdown]
//
// ## Fehlermeldungen
//
// Ein grosser Vorteil von FluentAssertions: bessere Fehlermeldungen

// %% [markdown]
//
// **NUnit:**
// ```
// Expected: 5
// But was:  3
// ```
//
// **FluentAssertions:**
// ```
// Expected numbers to contain 5 items, but found 3.
// ```

// %% [markdown]
//
// ## Test-Beispiel

// %%
public class FluentAssertionsBasicsTest
{
    [Test]
    public void TestStringOperations()
    {
        string result = "Hello" + " " + "World";
        result.Should().Be("Hello World");
        result.Should().StartWith("Hello");
        result.Should().HaveLength(11);
    }

    [Test]
    public void TestCollectionOperations()
    {
        var numbers = new List<int> { 3, 1, 4, 1, 5 };
        numbers.Should().HaveCount(5);
        numbers.Should().Contain(4);
        numbers.Should().ContainInOrder(new[] { 3, 4, 5 });
    }

    [Test]
    public void TestNumericComparisons()
    {
        double result = 0.1 + 0.2;
        result.Should().BeApproximately(0.3, 0.001);
    }
}

// %%
NunitTestRunner.RunTests(typeof(FluentAssertionsBasicsTest));

// %% [markdown]
//
// ## Workshop: Fluent Assertions
//
// Schreiben Sie die folgenden NUnit-Assertions als FluentAssertions um:

// %%
public class NunitStyleTests
{
    [Test]
    public void TestEquality()
    {
        Assert.That(5 + 5, Is.EqualTo(10));
    }

    [Test]
    public void TestStringContains()
    {
        string text = "The quick brown fox";
        Assert.That(text, Does.Contain("brown"));
    }

    [Test]
    public void TestListCount()
    {
        var items = new List<string> { "A", "B", "C" };
        Assert.That(items, Has.Count.EqualTo(3));
        Assert.That(items, Does.Contain("B"));
    }

    [Test]
    public void TestException()
    {
        Assert.Throws<ArgumentNullException>(() => {
            string s = null;
            if (s == null) throw new ArgumentNullException(nameof(s));
        });
    }
}

// %%
NunitTestRunner.RunTests(typeof(NunitStyleTests));

// %%

// %%
NunitTestRunner.RunTests(typeof(FluentStyleTests));
