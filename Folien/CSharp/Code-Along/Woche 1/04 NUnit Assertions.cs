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

// %%

// %%

// %%

// %% [markdown]
//
// ### Gleichheits-Assertions

// %%

// %%

// %%
string str1 = new string("Hello");
string str2 = new string("Hello");

// %%

// %%

// %%

// %% [markdown]
//
// #### Vergleich von Gleitkommazahlen

// %%

// %%

// %%

// %%

// %%

// %% [markdown]
//
// ### Identitäts-Assertions

// %%

// %%

// %%

// %%

// %% [markdown]
//
// ### Null-Assertions

// %%

// %%

// %%

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

// %%

// %% [markdown]
//
// #### Anweisungs-Lambdas

// %%

// %%

// %% [markdown]
//
// ### Exception-Assertions

// %%
int n = 0;

// %%

// %%

// %%

// %%

// %% [markdown]
//
// ### Weitere nützliche Constraints
//
// - `Is.GreaterThan`, `Is.LessThan`, `Is.InRange`
// - `Does.Contain`, `Does.StartWith`, `Does.EndWith` (für Strings)
// - `Has.Count.EqualTo`, `Has.Member`, `Is.Empty` (für Collections)
// - `Is.TypeOf<T>`, `Is.InstanceOf<T>` (für Typ-Prüfungen)

// %%

// %%

// %%

// %%

// %%

// %%

// %%

// %%

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
// Assert that Reverse("Hello") returns "olleH"

// %% [markdown]
//
// `IsPalindrome` erkennt Palindrome (unabhängig von Groß-/Kleinschreibung):

// %%
// Assert that "Racecar" is a palindrome
// Assert that "Hello" is not a palindrome

// %% [markdown]
//
// `Capitalize` formatiert den String korrekt:

// %%
// Assert that Capitalize("hELLO") returns "Hello"
// Assert that the result starts with "H"

// %% [markdown]
//
// `CountWords` zählt die Wörter korrekt:

// %%
// Assert that "Hello World" has 2 words
// Assert that an empty string has 0 words
// Assert that "  spaced  out  " has 2 words

// %% [markdown]
//
// `Capitalize` wirft bei `null` keine Exception:

// %%
// Assert that Capitalize(null) returns null
