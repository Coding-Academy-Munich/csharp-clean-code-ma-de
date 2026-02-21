// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Refactoring und Tests</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// - Zum Refactoring brauchen wir Tests
//   - Sonst können wir nicht wissen, ob wir das Verhalten geändert haben
// - Aber: manche Tests erschweren das Refactoring

// %% [markdown]
//
// ## Tests für Refactoring
//
// - Schreiben Sie Tests, die das öffentliche Verhalten testen
// - Vermeiden Sie alle Tests, die sich auf Implementierungs-Details beziehen
//   - Auch für Unit-Tests
// - Dazu testen sich oft Methoden gegenseitig
// - Das ist OK!

// %%
using System.Collections.Generic;
using System.Linq;

// %%
public static void Check(bool result, string expression)
{
    if (result)
    {
        Console.Write("Passed");
    }
    else
    {
        Console.Write("FAILED");
    }
    Console.WriteLine($" test for '{expression}'");
}

// %% [markdown]
//
// **Testen Sie NICHT so!**

// %%
public class StackTester<T> : Stack<T>
{
    // Access the protected data member of the stack
    public object[] GetData()
    {
        return ToArray().Cast<object>().ToArray();
    }

    // Access the actual number of elements in the stack
    public int GetActualSize()
    {
        return Count;
    }
}

// %%
StackTester<int> unit = new StackTester<int>();
unit.Push(1);

Check(unit.GetActualSize() == 1, "stack size is 1");
Check(unit.GetData()[0].Equals(1), "first element is 1");

int result = unit.Pop();

Check(result == 1, "pop result is 1");
Check(unit.GetActualSize() == 0, "size after popping is 0");

// %% [markdown]
//
// - Für `System.Collections.Generic.Stack<T>` ist in der Spezifikation festgelegt, wie er implementiert ist
// - Aber in "normalem" Code könnte sich die Implementierung jederzeit ändern
// - Testen Sie statt dessen so:

// %%
using System.Collections.Generic;

// %%
Stack<int> unit = new Stack<int>();
unit.Push(1);

Check(unit.Count == 1, "stack size is 1");
Check(unit.Peek() == 1, "top element is 1");

unit.Pop();
Check(unit.Count == 0, "stack is empty after popping");

// %% [markdown]
//
// - Diese Tests testen das öffentliche Verhalten
// - Das öffentliche Interface muss geeignet sein, diese Tests zu schreiben
//   - Versuchen Sie **nicht**, das durch Getter und Setter für jeden
//     Daten-Member zu erreichen
//   - Stattdessen sollten Sie Abfragen oder einen "abstrakten Zustand"
//     öffentlich machen
//   - Für den Stack sind das z.B. die `Peek()` und `Count`-Eigenschaften
// - Meistens macht das auch die normale Benutzung der Klasse einfacher
// - TDD ist ein Weg um das zu erreichen
//   - Aber: Schreiben Sie auch in TDD Tests für Feature-Inkremente, nicht für
//     Implementierungs-Inkremente

// %% [markdown]
//
// ## Workshop: Vorrangwarteschlange (Priority Queue)
//
// In diesem Workshop sollen Sie eine Vorrangwarteschlange testen, ohne sich auf
// ihre internen Implementierungsdetails zu verlassen.
//
// ### Hintergrund
//
// Eine Vorrangwarteschlange ist eine Datenstruktur, die Elemente mit
// zugehörigen Prioritäten speichert. Sie unterstützt zwei Hauptoperationen:
// - Enqueue: Füge ein Element mit einer gegebenen Priorität hinzu
// - Dequeue: Entferne und gib das Element mit der höchsten Priorität zurück
//
// Die Herausforderung besteht darin, zu überprüfen, dass die Vorrangwarteschlange
// die Reihenfolge der Elemente korrekt beibehält, ohne direkt auf ihre interne
// Struktur zuzugreifen.

// %% [markdown]
//
// Die folgende `PriorityQueue<T>`-Klasse implementiert die übliche Schnittstelle
// für eine Vorrangwarteschlange mit einer einfachen Listen-basierten
// Repräsentation:
//
// - `void Enqueue(T item, int priority)`: Füge ein Element mit der gegebenen
//   Priorität hinzu
// - `T Dequeue()`: Entferne und gib das Element mit der höchsten Priorität zurück
// - `bool IsEmpty`: Gib true zurück, wenn die Warteschlange leer ist, sonst
//   false

// %%
using System;
using System.Collections.Generic;
using System.Linq;

// %%
public class PriorityQueue<T>
{
    private List<PriorityItem<T>> items;

    public PriorityQueue()
    {
        this.items = new List<PriorityItem<T>>();
    }

    public void Enqueue(T item, int priority)
    {
        items.Add(new PriorityItem<T>(item, priority));
        items = items.OrderByDescending(x => x.Priority).ToList();
    }

    public T Dequeue()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Queue is empty");
        }
        var item = items[0];
        items.RemoveAt(0);
        return item.Item;
    }

    public bool IsEmpty => items.Count == 0;

    private class PriorityItem<T>
    {
        public T Item { get; }
        public int Priority { get; }

        public PriorityItem(T item, int priority)
        {
            Item = item;
            Priority = priority;
        }
    }
}

// %% [markdown]
//
// Schreiben Sie einen Tests, die das korrekte Verhalten der
// Vorrangwarteschlange über ihre öffentliche Schnittstelle verifizieren. Ihre
// Tests sollten folgendes abdecken:
//
// 1. Grundlegende Funktionalität (`Enqueue`, `Dequeue`, `IsEmpty`)
// 2. Korrekte Prioritäten-Reihenfolge beim Herausnehmen von Elementen mit `Dequeue`
// 3. Umgang mit Elementen mit gleichen Prioritäten
// 4. Randfälle (leere Warteschlange, einzelnes Element)
//
// Denken Sie daran, dass Sie in Ihren Tests nicht auf die interne Struktur der
// Warteschlange zugreifen können!
//
// - Welche Strategien haben Sie verwendet, um die Prioritäten-Reihenfolge zu
//   testen, ohne auf die interne Struktur zuzugreifen?
// - Wie sicher sind Sie, dass Ihre Tests das Verhalten der Vorrangwarteschlange
//   vollständig verifizieren?

// %%
#r "nuget: NUnit, *"

// %%
using NUnit.Framework;

// %%
#load "NunitTestRunner.cs"

// %%
using static NunitTestRunner;

// %%
[TestFixture]
public class PriorityQueueTest
{
    private PriorityQueue<string> queue;

    [SetUp]
    public void SetUp()
    {
        queue = new PriorityQueue<string>();
    }

    [Test]
    public void TestIsEmptyOnNewQueue()
    {
        Assert.That(queue.IsEmpty, Is.True);
    }

    [Test]
    public void TestIsNotEmptyAfterEnqueue()
    {
        queue.Enqueue("Item", 1);
        Assert.That(queue.IsEmpty, Is.False);
    }

    [Test]
    public void TestIsEmptyAfterDequeueLastItem()
    {
        queue.Enqueue("Item", 1);
        queue.Dequeue();
        Assert.That(queue.IsEmpty, Is.True);
    }

    [Test]
    public void TestSingleItemEnqueueDequeue()
    {
        queue.Enqueue("Item", 1);
        Assert.That(queue.Dequeue(), Is.EqualTo("Item"));
    }

    [Test]
    public void TestPriorityOrderWithTwoItems()
    {
        queue.Enqueue("Low", 1);
        queue.Enqueue("High", 2);
        Assert.That(queue.Dequeue(), Is.EqualTo("High"));
        Assert.That(queue.Dequeue(), Is.EqualTo("Low"));
    }

    [Test]
    public void TestPriorityOrderWithMultipleItems()
    {
        queue.Enqueue("Lowest", 1);
        queue.Enqueue("Highest", 4);
        queue.Enqueue("Medium", 2);
        queue.Enqueue("High", 3);

        Assert.That(queue.Dequeue(), Is.EqualTo("Highest"));
        Assert.That(queue.Dequeue(), Is.EqualTo("High"));
        Assert.That(queue.Dequeue(), Is.EqualTo("Medium"));
        Assert.That(queue.Dequeue(), Is.EqualTo("Lowest"));
    }

    [Test]
    public void TestEqualPriorities()
    {
        queue.Enqueue("First", 1);
        queue.Enqueue("Second", 1);

        string first = queue.Dequeue();
        string second = queue.Dequeue();

        Assert.That(
            (first == "First" && second == "Second") ||
            (first == "Second" && second == "First"),
            Is.True
        );
    }

    [Test]
    public void TestEnqueueDequeueMixedOperations()
    {
        queue.Enqueue("A", 1);
        queue.Enqueue("B", 3);
        Assert.That(queue.Dequeue(), Is.EqualTo("B"));
        queue.Enqueue("C", 2);
        Assert.That(queue.Dequeue(), Is.EqualTo("C"));
        Assert.That(queue.Dequeue(), Is.EqualTo("A"));
    }

    [Test]
    public void TestDequeueOnEmptyQueue()
    {
        Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
    }

    [Test]
    public void TestEnqueueDequeueEnqueueMaintainsCorrectOrder()
    {
        queue.Enqueue("Low", 1);
        queue.Enqueue("High", 2);
        Assert.That(queue.Dequeue(), Is.EqualTo("High"));
        queue.Enqueue("Medium", 1);
        Assert.That(queue.Dequeue(), Is.EqualTo("Low"));
        Assert.That(queue.Dequeue(), Is.EqualTo("Medium"));
    }
}

// %%
RunTests<PriorityQueueTest>();

// %% [markdown]
//
// ### Heap-basierte Implementierung
//
// Die Listen-basierte Implementierung der Vorrangwarteschlange hat eine
// Komplexität von O(n) für das Einfügen und Entfernen von Elementen. Eine
// effizientere Implementierung verwendet einen Heap, um die Operationen in O(log
// n) durchzuführen.
//
// Hier ist eine einfache Heap-basierte Implementierung der Vorrangwarteschlange:

// %%
using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    private List<PriorityItem<T>> heap;
    private readonly Comparison<PriorityItem<T>> comparison;

    public PriorityQueue()
    {
        this.heap = new List<PriorityItem<T>>();
        this.comparison = (a, b) => b.Priority.CompareTo(a.Priority);
    }

    public void Enqueue(T item, int priority)
    {
        heap.Add(new PriorityItem<T>(item, priority));
        SiftUp(heap.Count - 1);
    }

    public T Dequeue()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Queue is empty");
        }
        T item = heap[0].Item;
        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);
        if (!IsEmpty)
        {
            SiftDown(0);
        }
        return item;
    }

    public bool IsEmpty => heap.Count == 0;

    private void SiftUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;
            if (comparison(heap[index], heap[parentIndex]) >= 0)
            {
                break;
            }
            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void SiftDown(int index)
    {
        int size = heap.Count;
        while (true)
        {
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;
            int highest = index;

            if (leftChild < size && comparison(heap[leftChild], heap[highest]) < 0)
            {
                highest = leftChild;
            }
            if (rightChild < size && comparison(heap[rightChild], heap[highest]) < 0)
            {
                highest = rightChild;
            }

            if (highest == index)
            {
                break;
            }

            Swap(index, highest);
            index = highest;
        }
    }

    private void Swap(int i, int j)
    {
        PriorityItem<T> temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }

    private class PriorityItem<T>
    {
        public T Item { get; }
        public int Priority { get; }

        public PriorityItem(T item, int priority)
        {
            Item = item;
            Priority = priority;
        }
    }
}

// %% [markdown]
//
// Funktionieren Ihre Tests für die Listen-basierte Implementierung auch für die
// Heap-basierte Implementierung?
//
// *Hinweis:* Sie können im Notebook einfach die Zelle mit der neuen
// Implementierung der Vorrangwarteschlange ausführen und dann die Zellen mit
// Ihren Tests erneut ausführen.

// %% [markdown]
//
// Die Klasse `HeapPriorityQueueTest` ist eine exakte Kopie der Klasse
// `PriorityQueueTest`, aber sie ist hier nochmal angegeben, um das Testen der
// Heap-basierten Implementierung zu erleichtern.

// %%
[TestFixture]
public class HeapPriorityQueueTest
{
    private PriorityQueue<string> queue;

    [SetUp]
    public void SetUp()
    {
        queue = new PriorityQueue<string>();
    }

    [Test]
    public void TestIsEmptyOnNewQueue()
    {
        Assert.That(queue.IsEmpty, Is.True);
    }

    [Test]
    public void TestIsNotEmptyAfterEnqueue()
    {
        queue.Enqueue("Item", 1);
        Assert.That(queue.IsEmpty, Is.False);
    }

    [Test]
    public void TestIsEmptyAfterDequeueLastItem()
    {
        queue.Enqueue("Item", 1);
        queue.Dequeue();
        Assert.That(queue.IsEmpty, Is.True);
    }

    [Test]
    public void TestSingleItemEnqueueDequeue()
    {
        queue.Enqueue("Item", 1);
        Assert.That(queue.Dequeue(), Is.EqualTo("Item"));
    }

    [Test]
    public void TestPriorityOrderWithTwoItems()
    {
        queue.Enqueue("Low", 1);
        queue.Enqueue("High", 2);
        Assert.That(queue.Dequeue(), Is.EqualTo("High"));
        Assert.That(queue.Dequeue(), Is.EqualTo("Low"));
    }

    [Test]
    public void TestPriorityOrderWithMultipleItems()
    {
        queue.Enqueue("Lowest", 1);
        queue.Enqueue("Highest", 4);
        queue.Enqueue("Medium", 2);
        queue.Enqueue("High", 3);

        Assert.That(queue.Dequeue(), Is.EqualTo("Highest"));
        Assert.That(queue.Dequeue(), Is.EqualTo("High"));
        Assert.That(queue.Dequeue(), Is.EqualTo("Medium"));
        Assert.That(queue.Dequeue(), Is.EqualTo("Lowest"));
    }

    [Test]
    public void TestEqualPriorities()
    {
        queue.Enqueue("First", 1);
        queue.Enqueue("Second", 1);

        string first = queue.Dequeue();
        string second = queue.Dequeue();

        Assert.That(
            (first == "First" && second == "Second") ||
            (first == "Second" && second == "First"),
            Is.True
        );
    }

    [Test]
    public void TestEnqueueDequeueMixedOperations()
    {
        queue.Enqueue("A", 1);
        queue.Enqueue("B", 3);
        Assert.That(queue.Dequeue(), Is.EqualTo("B"));
        queue.Enqueue("C", 2);
        Assert.That(queue.Dequeue(), Is.EqualTo("C"));
        Assert.That(queue.Dequeue(), Is.EqualTo("A"));
    }

    [Test]
    public void TestDequeueOnEmptyQueue()
    {
        Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
    }

    [Test]
    public void TestEnqueueDequeueEnqueueMaintainsCorrectOrder()
    {
        queue.Enqueue("Low", 1);
        queue.Enqueue("High", 2);
        Assert.That(queue.Dequeue(), Is.EqualTo("High"));
        queue.Enqueue("Medium", 1);
        Assert.That(queue.Dequeue(), Is.EqualTo("Low"));
        Assert.That(queue.Dequeue(), Is.EqualTo("Medium"));
    }
}

// %%
RunTests<HeapPriorityQueueTest>();
