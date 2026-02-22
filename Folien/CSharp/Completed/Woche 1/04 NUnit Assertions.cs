// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>NUnit: Assertions</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// ## Was ist NUnit?
//
// - NUnit ist ein modernes Test-Framework für C#
// - Open-Source
// - Einfach in Projekte zu integrieren
// - Viele Features, einschließlich des Constraint-Modells für Assertions
// - Eines der ältesten und am weitesten verbreiteten .NET Test-Frameworks

// %% [markdown]
//
// ## Features von NUnit
//
// - Verwaltung von Tests und Test-Suites
// - Assertion-Bibliothek mit Constraint-Modell
// - Ausführung von Tests (Test Runner)
// - Versetzen des SuT in einen definierten Zustand (Test Fixtures)
// - Unterstützung für parametrisierte Tests (TestCase, TestCaseSource)

// %% [markdown]
//
// ## Assertions in NUnit: Das Constraint-Modell
//
// - `Assert.That(condition)` um Bedingungen zu prüfen
// - `Assert.That(actual, Is.EqualTo(expected))` um Werte zu prüfen
// - `Assert.That(actual, Is.SameAs(expected))` um Referenzen zu prüfen
// - `Assert.That(actual, Is.Null)` um auf `null` zu prüfen
// - `Assert.Throws<T>(...)` um Exceptions zu prüfen

// %%
#r "nuget: NUnit, *"

// %%
using NUnit.Framework;

// %% [markdown]
//
// ### Boolesche Assertions

// %%
Assert.That(5 > 3, Is.True);

// %%
Assert.That(2 > 5, Is.False);

// %%
Assert.That(5 > 3);

// %%
// Assert.That(1 > 4);

// %% [markdown]
//
// ### Gleichheits-Assertions

// %%
Assert.That(2 + 2, Is.EqualTo(4));

// %%
Assert.That(2 + 2, Is.Not.EqualTo(5));

// %%
string str1 = new string("Hello");
string str2 = new string("Hello");

// %%
Assert.That(str1, Is.EqualTo(str2));

// %%
Assert.That(str1.Equals(str2));

// %%
// Assert.That("Hello", Is.EqualTo("World"));

// %% [markdown]
//
// #### Vergleich von Gleitkommazahlen

// %%
// Assert.That(0.1 + 0.2, Is.EqualTo(0.3));

// %%
Assert.That(0.1 + 0.2, Is.EqualTo(0.3).Within(0.00001));

// %%
Assert.That(0.1 + 0.2, Is.EqualTo(0.3).Within(1).Percent);

// %%
// Assert.That(0.1, Is.EqualTo(0.11).Within(0.001));

// %%
Assert.That(0.1, Is.EqualTo(0.11).Within(0.1));

// %% [markdown]
//
// ### Identitäts-Assertions

// %%
Assert.That("Hello", Is.SameAs("Hello"));

// %%
// Assert.That(str1, Is.SameAs(str2));

// %%
Assert.That(str1, Is.SameAs(str1));

// %%
// Assert.That(str1, Is.Not.SameAs(str1));

// %% [markdown]
//
// ### Null-Assertions

// %%
Assert.That(null, Is.Null);

// %%
// Assert.That(0, Is.Null);

// %%
Assert.That(123, Is.Not.Null);

// %% [markdown]
//
// ### Einschub: Lambda-Ausdrücke
//
// - Lambda-Ausdrücke sind eine Möglichkeit, Funktionen als Argumente zu
//   übergeben.
// - Sie können in Delegat-Typen konvertiert werden.

// %% [markdown]
//
// #### Ausdrucks-Lambdas

// %%
Func<int, int, int> add = (x, y) => x + y;

// %%
add(2, 3)

// %% [markdown]
//
// #### Anweisungs-Lambdas

// %%
Action<string> print = s => {
    Console.Write("Hello, ");
    Console.WriteLine(s);
};

// %%
print("World!");

// %% [markdown]
//
// ### Exception-Assertions

// %%
int n = 0;

// %%
Assert.Throws<DivideByZeroException>(() => { var x = 1 / n; });

// %%
// Assert.Throws<DivideByZeroException>(() => { var x = 1 / 1; });

// %%
Assert.Throws<DivideByZeroException>(() => { var x = 1 / n; });

// %%
Assert.Catch<ArithmeticException>(() => { var x = 1 / n; });

// %% [markdown]
//
// ### Weitere nützliche Constraints
//
// - `Is.GreaterThan`, `Is.LessThan`, `Is.InRange`
// - `Does.Contain`, `Does.StartWith`, `Does.EndWith` (für Strings)
// - `Has.Count.EqualTo`, `Has.Member`, `Is.Empty` (für Collections)
// - `Is.TypeOf<T>`, `Is.InstanceOf<T>` (für Typ-Prüfungen)

// %%
Assert.That(5, Is.GreaterThan(3));

// %%
Assert.That(2, Is.LessThan(5));

// %%
Assert.That(5, Is.InRange(1, 10));

// %%
Assert.That("Hello World", Does.Contain("World"));

// %%
Assert.That("Hello World", Does.StartWith("Hello"));

// %%
var numbers = new List<int> { 1, 2, 3 };
Assert.That(numbers, Has.Count.EqualTo(3));

// %%
Assert.That(numbers, Has.Member(2));

// %%
Assert.That(new List<int>(), Is.Empty);

// %% [markdown]
//
// ## Mini-Workshop: NUnit Assertions
//
// Hier ist eine einfache Klasse `StringUtils`. Schreiben Sie Assertions, um
// die Methoden dieser Klasse zu überprüfen. Verwenden Sie dabei verschiedene
// NUnit Constraints.

// %%
public class StringUtils
{
    public string Reverse(string s)
    {
        char[] chars = s.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }

    public bool IsPalindrome(string s)
    {
        string lower = s.ToLower();
        return lower == Reverse(lower);
    }

    public string Capitalize(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        return char.ToUpper(s[0]) + s.Substring(1).ToLower();
    }

    public int CountWords(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return 0;
        return s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
    }
}

// %% [markdown]
//
// Schreiben Sie Assertions für die folgenden Fälle:

// %% [markdown]
//
// `Reverse` gibt den String umgekehrt zurück:

// %%
var utils = new StringUtils();
Assert.That(utils.Reverse("Hello"), Is.EqualTo("olleH"));

// %% [markdown]
//
// `IsPalindrome` erkennt Palindrome (unabhängig von Groß-/Kleinschreibung):

// %%
Assert.That(utils.IsPalindrome("Racecar"), Is.True);
Assert.That(utils.IsPalindrome("Hello"), Is.False);

// %% [markdown]
//
// `Capitalize` formatiert den String korrekt:

// %%
Assert.That(utils.Capitalize("hELLO"), Is.EqualTo("Hello"));
Assert.That(utils.Capitalize("hELLO"), Does.StartWith("H"));

// %% [markdown]
//
// `CountWords` zählt die Wörter korrekt:

// %%
Assert.That(utils.CountWords("Hello World"), Is.EqualTo(2));
Assert.That(utils.CountWords(""), Is.EqualTo(0));
Assert.That(utils.CountWords("  spaced  out  "), Is.EqualTo(2));

// %% [markdown]
//
// `Capitalize` wirft bei `null` keine Exception:

// %%
Assert.That(utils.Capitalize(null), Is.Null);
