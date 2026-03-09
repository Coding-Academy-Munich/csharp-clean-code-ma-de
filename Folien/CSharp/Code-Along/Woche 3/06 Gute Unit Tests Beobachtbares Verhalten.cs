// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Gute Unit Tests: Beobachtbares Verhalten</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
//
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>


// %% [markdown]
//
// ### Warum Tests von beobachtbarem Verhalten, nicht Implementierung?
//
// Beobachtbares Verhalten
// - ist leichter zu verstehen
// - ist stabiler als Implementierung
// - entspricht eher dem Kundennutzen

// %% [markdown]
//
// ## Teste beobachtbares Verhalten, nicht Implementierung
//
// - Abstrahiere so weit wie möglich von Implementierungsdetails
//   - Auch auf Unit-Test Ebene
// - Oft testen sich verschiedene Methoden gegenseitig
// - Dies erfordert manchmal die Einführung von zusätzlichen Methoden
//     - Diese Methoden sollen für Anwender sinnvoll sein, nicht nur für Tests
//     - Oft "abstrakter Zustand" von Objekten
//     - **Nicht:** konkreten Zustand öffentlich machen

// %%
using System.Collections.Generic;

// %%
#r "nuget: NUnit, *"

// %%
#load "NUnitTestRunner.cs"

// %%
using NUnit.Framework;
using static NUnitTestRunner;

// %%
public class Stack_Bad
{
    public List<object> Items { get; } = new();

    public void Push(object newItem)
    {
        Items.Add(newItem);
    }

    public object Pop()
    {
        if (Items.Count == 0)
        {
            throw new InvalidOperationException("Cannot pop from an empty stack.");
        }
        var item = Items[^1];
        Items.RemoveAt(Items.Count - 1);
        return item;
    }
}

// %% [markdown]
//
// Dieser Test ist schlecht, weil er auf Implementierungsdetails (die `Items`-Property) zugreift.

// %%
[TestFixture]
public class StackTests_Bad
{
    [Test]
    public void TestStack()
    {
        var myStack = new Stack_Bad();
        myStack.Push(5);
        Assert.That(myStack.Items, Is.EqualTo(new List<object> { 5 }));
    }
}

// %%
RunTests<StackTests_Bad>();

// %%
public class Stack
{
    private List<object> Items { get; } = new();

    public int Count => Items.Count;

    public void Push(object newItem)
    {
        Items.Add(newItem);
    }

    public object Pop()
    {
        if (Items.Count == 0)
        {
            throw new InvalidOperationException("Cannot pop from an empty stack.");
        }
        var item = Items[^1];
        Items.RemoveAt(Items.Count - 1);
        return item;
    }
}

// %%
[TestFixture]
public class StackTests_Good
{
    [Test]
    public void TestStack()
    {
        var myStack = new Stack();
        myStack.Push(5);
        Assert.That(myStack.Count, Is.EqualTo(1));
        Assert.That(myStack.Pop(), Is.EqualTo(5));
    }
}

// %%
RunTests<StackTests_Good>();

// %% [markdown]
//
// ### Tests, wenn nur `Push()` und `Pop()` verfügbar sind

// %%
[TestFixture]
public class StackTests_PushPopOnly
{
    [Test]
    public void TestPushAndPopReturnsCorrectValue()
    {
        var myStack = new Stack();
        myStack.Push(5);
        Assert.That(myStack.Pop(), Is.EqualTo(5));
    }

    [Test]
    public void TestPushAndPopWithMultipleElements()
    {
        var myStack = new Stack();
        myStack.Push(5);
        myStack.Push(10);
        Assert.That(myStack.Pop(), Is.EqualTo(10));
        Assert.That(myStack.Pop(), Is.EqualTo(5));
    }

    [Test]
    public void TestPopWithEmptyStack()
    {
        var myStack = new Stack();
        Assert.Throws<InvalidOperationException>(() => myStack.Pop());
    }

    [Test]
    public void TestPushPushesOnlyASingleElement()
    {
        var myStack = new Stack();
        myStack.Push(5);
        Assert.That(myStack.Pop(), Is.EqualTo(5));
        Assert.Throws<InvalidOperationException>(() => myStack.Pop());
    }
}


// %%
RunTests<StackTests_PushPopOnly>();
