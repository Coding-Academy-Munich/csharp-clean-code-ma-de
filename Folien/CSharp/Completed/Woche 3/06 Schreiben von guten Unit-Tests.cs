// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Schreiben von guten Unit-Tests</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// ## Mechanik von Unit-Tests
//
// Unit-Tests sollen
// - automatisiert sein: keine manuelle Interaktion
// - selbsttestend sein: Pass/Fail
// - feingranular sein
// - schnell sein
// - isoliert sein
// - zu jedem Zeitpunkt erfolgreich ausführbar sein

// %% [markdown]
//
// ## Einfache Struktur!
//
// <ul>
//   <li>Einfache, standardisierte Struktur<br>&nbsp;<br>
//     <table style="display:inline;margin:20px 20px;">
//     <tr><td style="text-align:left;width:60px;padding-left:15px;">Arrange</td>
//         <td style="text-align:left;width:60px;padding-left:15px;border-left:1px solid
//         black;">Given</td> <td
//         style="text-align:left;width:800px;padding-left:15px;border-left:1px solid
//         black;">
//           Bereite das Test-Environment vor</td></tr>
//     <tr><td style="text-align:left;padding-left:15px;">Act</td>
//         <td style="text-align:left;width:60px;padding-left:15px;border-left:1px solid
//         black;">
//            When</td>
//         <td style="text-align:left;width:800px;padding-left:15px;border-left:1px
//         solid black;">
//            Führe die getestete Aktion aus (falls vorhanden)</td></tr>
//     <tr><td style="text-align:left;padding-left:15px;">Assert</td>
//         <td style="text-align:left;width:60px;padding-left:15px;border-left:1px solid
//         black;">
//            Then</td>
//         <td style="text-align:left;width:800px;padding-left:15px;border-left:1px
//         solid black;">
//            Überprüfe die Ergebnisse</td></tr>
//     </table>
//     <br>&nbsp;
//   </li>
//   <li>Wenig Code
//     <ul>
//       <li>Wenig Boilerplate</li>
//       <li>Factories, etc. für Tests</li>
//     </ul>
//   </li>
// </ul>

// %%
#r "nuget: NUnit, *"

// %%
#load "NUnitTestRunner.cs"

// %%
using NUnit.Framework;
using static NUnitTestRunner;

// %%
using System;
using System.Collections.Generic;

// %%
[TestFixture]
public class ListExtensionTests
{
    [Test]
    public void TestTheAddRangeMethodOfTheBuiltInListType()
    {
        // Arrange/Given
        var x = new List<int> { 1, 2, 3 };
        var y = new List<int> { 10, 20 };

        // Act/When
        x.AddRange(y);

        // Assert
        Assert.That(x, Is.EqualTo(new List<int> { 1, 2, 3, 10, 20 }));
    }
}

// %%
RunTests<ListExtensionTests>();

// %% [markdown]
//
// - Wie viele Tests wollen wir haben?
// - Wie viele Werte wollen wir überprüfen?

// %% [markdown]
//
// ## Versuch: Erschöpfendes Testen
//
// - Wir schreiben erschöpfende Tests, d.h. Tests, die alle möglichen Eingaben eines
//   Programms abdecken

// %% [markdown]
//
// - Erschöpfendes Testen ist nicht möglich
// - Beispiel Passworteingabe:
//   - Angenommen, Passwörter mit maximal 20 Zeichen sind zulässig,
//     80 Eingabezeichen sind erlaubt (große und kleine Buchstaben, Sonderzeichen)
//   - Das ergibt $80^{20}$ = 115.292.150.460.684.697.600.000.000.000.000.000.000
//     mögliche Eingaben
//   - Bei 10ns für einen Test würde man ca. $10^{24}$ Jahre brauchen, um alle Eingaben
//     zu testen
//   - Das Universum ist ungefähr $1.4 \times 10^{10}$ Jahre alt

// %% [markdown]
//
// ## Effektivität und Effizienz von Tests
//
// - Unit-Tests sollen effektiv und effizient sein
//   - Effektiv: Die Tests sollen so viele Fehler wie möglich finden
//   - Effizient: Wir wollen die größte Anzahl an Fehlern mit der geringsten Anzahl
//     an möglichst einfachen Tests finden
// - Effizienz ist wichtig, da Tests selbst Code sind, der gewartet werden muss und
//   Fehler enthalten kann

// %% [markdown]
//
// ## Wie schreibt man gute Unit-Tests?
//
// - Teste beobachtbares Verhalten, nicht Implementierung
// - Bevorzuge Tests von Werten gegenüber Tests von Zuständen
// - Bevorzuge Tests von Zuständen gegenüber Tests von Interaktion
// - Verwende Test-Doubles dann (aber auch nur dann), wenn eine Abhängigkeit
//   "eine Rakete abfeuert"
// - (Diese Regeln setzen voraus, dass der Code solche Tests erlaubt)

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
        // This test is good because it uses the public interface (Count and Pop).
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

// %% [markdown]
//
// ## Werte > Zustand > Interaktion
//
// - Verständlicher
// - Leichter zu testen
// - Oft stabiler gegenüber Refactorings
//
// Ausnahme: Testen von Protokollen

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
public class AdderSpy : IAbstractAdder {
    public List<(int, int)> Calls { get; } = [];

    public void Add(int x, int y) {
        Calls.Add((x, y));
    }
}

// %%
[TestFixture]
class InteractionAdderTests {
    [Test]
    public void TestAdderWithSpy()
    {
        AdderSpy spy = new AdderSpy();
        InteractionAdder adder = new InteractionAdder(spy);

        adder.Add(2, 3);

        Assert.That(spy.Calls.Count, Is.EqualTo(1));
        Assert.That(spy.Calls[0].Item1, Is.EqualTo(2));
        Assert.That(spy.Calls[0].Item2, Is.EqualTo(3));
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

// %% [markdown]
//
// ## Wie schreibt man testbaren Code?
//
// - Umwandeln von weniger testbarem in besser testbaren Stil
//   - Beobachtbarkeit
//   - Keine globalen oder statischen Daten
//   - Funktionale (unveränderliche) Datenstrukturen
// - Gutes objektorientiertes Design
//   - Hohe Kohäsion
//   - Geringe Kopplung, Management von Abhängigkeiten
// - Etc.

// %% [markdown]
//
// ## Prozess
//
// - Iteratives Vorgehen
//   - Kleine Schritte mit Tests
// - Test-Driven Development (TDD)
//   - Schreiben von Tests vor Code

// %% [markdown]
//
// ## Workshop: Bessere Testbarkeit
//
// - Wie können Sie Tests für die folgenden Funktionen/Klassen schreiben?
// - Wie können Sie die folgenden Funktionen/Klassen verbessern, um sie besser
//   testbar zu machen?
// - Was für Nachteile ergeben sich dadurch?

// %%
using System;
using System.Collections.Generic;

// %%
public class Counter {
    private static int c = 0;
    public static int Count() {
        return c++;
    }
}

// %%
for (int i = 0; i < 3; ++i) {
    System.Console.WriteLine(Counter.Count());
}

// %%
public class Counter2 {
    private int c = 0;
    public int Invoke() => c++;
}

// %%
Counter2 counter = new Counter2();

// %%
for (int i = 0; i < 3; ++i) {
    System.Console.WriteLine(counter.Invoke());
}

// %%
public enum State {
    Off,
    On
}

// %%
public class Switch {
    private State state = State.Off;

    public void Toggle() {
        state = state == State.Off ? State.On : State.Off;
        System.Console.WriteLine("Switch is " + (state == State.Off ? "OFF" : "ON"));
    }
}

// %%
Switch s = new Switch();

// %%
for (int i = 0; i < 3; ++i) {
    s.Toggle();
}

// %%
public class SwitchWithGetter {
    private State state = State.Off;

    public void Toggle() {
        state = state == State.Off ? State.On : State.Off;
        System.Console.WriteLine("Switch is " + (state == State.Off ? "OFF" : "ON"));
    }

    public State GetState() => state;
}

// %%
SwitchWithGetter sg = new SwitchWithGetter();

// %%
for (int i = 0; i < 3; ++i) {
    sg.Toggle();
}

// %%
System.Console.WriteLine("Switch is " + (sg.GetState() == State.Off ? "OFF" : "ON"));

// %%
public class ObservableSwitch
{
    private State _state = State.Off;
    public event Action<State> StateChanged;

    public void Toggle()
    {
        _state = (_state == State.Off) ? State.On : State.Off;
        Notify(_state);
    }

    private void Notify(State s)
    {
        StateChanged?.Invoke(s);
    }
}

// %%
var os = new ObservableSwitch();

// %%
os.StateChanged += s => Console.WriteLine($"Switch is {(s == State.Off ? "OFF" : "ON")}");

// %%
for (int i = 0; i < 3; i++)
{
    os.Toggle();
}

// %%
public static void PrintFib(int n) {
    int a = 0;
    int b = 1;
    for (int i = 0; i < n; ++i) {
        System.Console.WriteLine("fib(" + i + ") = " + b);
        int tmp = a;
        a = b;
        b = tmp + b;
    }
}

// %%
PrintFib(5);

// %%
public static int Fib1(int n) {
    int a = 0;
    int b = 1;
    for (int i = 0; i < n; ++i) {
        int tmp = a;
        a = b;
        b = tmp + b;
    }
    return b;
}

// %%
public static void PrintFib1(int n) {
    for (int i = 0; i < n; ++i) {
        System.Console.WriteLine("fib(" + i + ") = " + Fib1(i));
    }
}

// %%
PrintFib1(5);

// %%
using System;

public static void FibGen(int n, Action<int, int> f) {
    int a = 0;
    int b = 1;
    for (int i = 0; i < n; ++i) {
        f(i, b);
        int tmp = a;
        a = b;
        b = tmp + b;
    }
}

// %%
public static void PrintFib2(int n) {
    FibGen(n, (i, x) =>
        System.Console.WriteLine("fib(" + i + ") = " + x));
}

// %%
PrintFib2(5);

// %%
