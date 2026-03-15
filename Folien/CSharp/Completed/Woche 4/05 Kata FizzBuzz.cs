// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Kata: FizzBuzz</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
//
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>


// %% [markdown]
//
// ## Kata: FizzBuzz
//
// Erstellen Sie eine Klasse `FizzBuzz` mit einer öffentlichen statischen Methode:
// ```csharp
// public static void PrintFizzBuzz(int n)
// ```
// Diese Methode soll die Zahlen von 1 bis `n` auf dem Bildschirm ausgeben, aber dabei
//
// - jede Zahl, die durch 3 teilbar ist, durch `Fizz` ersetzt
// - jede Zahl, die durch 5 teilbar ist, durch `Buzz` ersetzt
// - jede Zahl, die durch 3 und 5 teilbar ist, durch `FizzBuzz` ersetzt

// %% [markdown]
//
// Zum Beispiel soll `FizzBuzz.PrintFizzBuzz(16)` die folgende Ausgabe erzeugen:
//
// ```plaintext
// 1
// 2
// Fizz
// 4
// Buzz
// Fizz
// 7
// 8
// Fizz
// Buzz
// 11
// Fizz
// 13
// 14
// FizzBuzz
// 16
// ```

// %% [markdown]
//
// ## Einfache Lösung
//
// - Direkte Ausgabe auf die Konsole
// - Einfach zu schreiben, aber schwer zu testen

// %%
using System.Collections.Generic;


// %%
#r "nuget: NUnit, *"
using NUnit.Framework;

// %%
#load "NUnitTestRunner.cs"
using static NUnitTestRunner;

// %%
public class FizzBuzzSimple {
    public static void PrintFizzBuzz(int n) {
        for (int i = 1; i <= n; i++) {
            if (i % 3 == 0 && i % 5 == 0) {
                Console.WriteLine("FizzBuzz");
            } else if (i % 3 == 0) {
                Console.WriteLine("Fizz");
            } else if (i % 5 == 0) {
                Console.WriteLine("Buzz");
            } else {
                Console.WriteLine(i);
            }
        }
    }

    public static void Run(string[] args) {
        PrintFizzBuzz(16);
    }
}

// %%
FizzBuzzSimple.Run([]);

// %% [markdown]
//
// ## Testbare Lösung
//
// - Trennung von Logik und Ausgabe
// - `GenerateFizzBuzz()` gibt eine Liste zurück (leicht testbar)
// - `PrintFizzBuzz()` nimmt einen `TextWriter` entgegen (Dependency Injection)

// %%
using System.IO;
using System.Collections.Generic;

// %%
public class FizzBuzz {
    public static List<string> GenerateFizzBuzz(int n) {
        if (n < 0)
            throw new ArgumentException("Input must be non-negative");

        List<string> result = new List<string>();
        for (int i = 1; i <= n; i++) {
            if (i % 3 == 0 && i % 5 == 0)
                result.Add("FizzBuzz");
            else if (i % 3 == 0)
                result.Add("Fizz");
            else if (i % 5 == 0)
                result.Add("Buzz");
            else
                result.Add(i.ToString());
        }
        return result;
    }

    public static void PrintFizzBuzz(int n, TextWriter output) {
        List<string> fizzBuzzList = GenerateFizzBuzz(n);
        foreach (string item in fizzBuzzList) {
            output.WriteLine(item);
        }
    }

    public static void Run(string[] args) {
        PrintFizzBuzz(16, Console.Out);
    }
}

// %%
FizzBuzz.Run([]);

// %% [markdown]
//
// ## Tests
//
// - Testen der Logik und der Ausgabe
// - Parametrisierte Tests für verschiedene Eingaben

// %%
using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// %%
[TestFixture]
public class FizzBuzzTest {
    [Test]
    public void GenerateFizzBuzz_ReturnsCorrectSequenceFor15() {
        List<string> expected = new List<string> {
            "1", "2", "Fizz", "4", "Buzz", "Fizz", "7", "8", "Fizz", "Buzz", "11", "Fizz", "13", "14", "FizzBuzz"
        };
        List<string> result = FizzBuzz.GenerateFizzBuzz(15);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase(1, "1")]
    [TestCase(3, "Fizz")]
    [TestCase(5, "Buzz")]
    [TestCase(15, "FizzBuzz")]
    public void GenerateFizzBuzz_ReturnsCorrectValueForSpecificNumbers(int number, string expected) {
        List<string> result = FizzBuzz.GenerateFizzBuzz(number);
        Assert.That(result.Last(), Is.EqualTo(expected));
    }

    [Test]
    public void GenerateFizzBuzz_ReturnsEmptyListForZero() {
        List<string> result = FizzBuzz.GenerateFizzBuzz(0);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void GenerateFizzBuzz_ThrowsArgumentExceptionForNegativeNumber() {
        Assert.That(() => FizzBuzz.GenerateFizzBuzz(-1), Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void GenerateFizzBuzz_ReturnsCorrectSequenceFor100() {
        List<string> result = FizzBuzz.GenerateFizzBuzz(100);
        Assert.That(result.Count, Is.EqualTo(100));
        Assert.That(result[0], Is.EqualTo("1"));
        Assert.That(result[1], Is.EqualTo("2"));
        Assert.That(result[2], Is.EqualTo("Fizz"));
        Assert.That(result[3], Is.EqualTo("4"));
        Assert.That(result[4], Is.EqualTo("Buzz"));
        Assert.That(result[5], Is.EqualTo("Fizz"));
        Assert.That(result[14], Is.EqualTo("FizzBuzz"));
        Assert.That(result[29], Is.EqualTo("FizzBuzz"));
        Assert.That(result[44], Is.EqualTo("FizzBuzz"));
        Assert.That(result[99], Is.EqualTo("Buzz"));
    }

    [Test]
    public void PrintFizzBuzz_WritesToProvidedTextWriter() {
        StringWriter stringWriter = new StringWriter();
        FizzBuzz.PrintFizzBuzz(5, stringWriter);
        string[] result = stringWriter.ToString().Trim().Split(Environment.NewLine);
        Assert.That(result, Is.EqualTo(new string[] { "1", "2", "Fizz", "4", "Buzz" }));
    }
}

// %%
NUnitTestRunner.RunTests<FizzBuzzTest>();
