// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Gute Unit Tests: Werte, Zustand und Interaktion</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
//
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>


// %% [markdown]
//
// ## Werte > Zustand > Interaktion
//
// - Verständlicher
// - Leichter zu testen
// - Oft stabiler gegenüber Refactorings
//
// Ausnahme: Testen von Protokollen

// %%
#r "nuget: Moq, 4.17"
#r "nuget: NUnit, *"

// %%
#load "NUnitTestRunner.cs"

// %%
using Moq;
using NUnit.Framework;
using static NUnitTestRunner;

// %% [markdown]
//
// ### Funktionen/Werte

// %%
public static int Add(int x, int y)
{
    return x + y;
}

// %%
[TestFixture]
class AddFunctionTests
{
    [Test]
    public void TestAdd()
    {
        Assert.That(Add(2, 3), Is.EqualTo(5));
    }
}

// %%
RunTests<AddFunctionTests>();

// %% [markdown]
//
// ### Zustand

// %%
public class Adder {
    public int X { get; set; }
    public int Y { get; set; }
    public int Result { get; private set; }
    public void Add() => Result = X + Y;
}

// %%
[TestFixture]
class AdderTests {
    [Test]
    public void TestAdder() {
        Adder adder = new Adder() { X = 2, Y = 3 };

        adder.Add();

        Assert.That(adder.Result, Is.EqualTo(5));
    }
}

// %%
RunTests<AdderTests>();

// %% [markdown]
//
// ### Seiteneffekt/Interaktion
//
// - Mit Interface

// %%
public interface IAbstractAdder {
    void Add(int x, int y);
}

// %%
public class InteractionAdder {
    private readonly IAbstractAdder adder;

    public InteractionAdder(IAbstractAdder adder) {
        this.adder = adder;
    }

    public void Add(int x, int y) {
        adder.Add(x, y);
    }
}

// %% [markdown]
//
// Test benötigt Mock/Spy

// %%
[TestFixture]
class InteractionAdderTests {
    [Test]
    public void TestAdderWithMock()
    {
        var mock = new Mock<IAbstractAdder>();
        InteractionAdder adder = new InteractionAdder(mock.Object);

        adder.Add(2, 3);

        mock.Verify(m => m.Add(2, 3), Times.Once());
    }
}

// %%
RunTests<InteractionAdderTests>();

// %% [markdown]
//
// ### Seiteneffekt/Interaktion
//
// - Ohne Interface

// %%
public class SideEffectAdder
{
    public static void AddAndPrint(int x, int y)
    {
        Console.WriteLine("Result: " + (x + y));
    }
}

// %%
SideEffectAdder.AddAndPrint(1, 2);

// %%
using System.IO;

// %%
public class ConsoleOutput : IDisposable
{
    private readonly StringWriter _stringWriter;
    private readonly TextWriter _originalOutput;

    public ConsoleOutput()
    {
        _stringWriter = new StringWriter();
        _originalOutput = Console.Out;
        Console.SetOut(_stringWriter);
    }

    public string GetOutput()
    {
        return _stringWriter.ToString();
    }

    public void Dispose()
    {
        Console.SetOut(_originalOutput);
        _stringWriter.Dispose();
    }
}

// %%
[TestFixture]
class SideEffectAdderTests {
    [Test]
    public void TestAddAndPrint() {
        using (var consoleOutput = new ConsoleOutput()) {
            SideEffectAdder.AddAndPrint(2, 3);
            Assert.That(consoleOutput.GetOutput().Trim(), Is.EqualTo("Result: 5"));
        }
    }
}

// %%
RunTests<SideEffectAdderTests>();
