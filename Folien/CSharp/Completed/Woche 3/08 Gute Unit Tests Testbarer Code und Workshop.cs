// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Gute Unit Tests: Testbarer Code und Workshop</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
//
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>


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
