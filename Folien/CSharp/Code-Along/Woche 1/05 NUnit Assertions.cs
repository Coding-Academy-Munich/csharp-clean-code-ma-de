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
