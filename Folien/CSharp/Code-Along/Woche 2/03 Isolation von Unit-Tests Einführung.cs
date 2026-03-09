// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Isolation von Unit-Tests: Einführung</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
//
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>


// %% [markdown]
//
// ## Unit-Test
//
// - Testet einen kleinen Teil des Codes (eine **"Unit"**)
// - Hat kurze Laufzeit
// - **Ist isoliert**

// %% [markdown]
//
// ## Was ist eine "Unit"?
//
// - Ein Verhalten?
//   - *Unit of behavior*
//   - Teil eines Szenarios/Use-Cases/...
//   - Ursprüngliche Intention von Kent Beck
// - Ein Code-Bestandteil?
//   - *Unit of code*
//   - Oft Unit = Klasse
//   - In der Literatur weit verbreitete Ansicht

// %% [markdown]
//
// ## Was bedeutet "isolierter" Test?
//
// - Keine Interaktion zwischen Tests?
//   - Isolierte Testfälle
//   - Klassische Unit-Tests (Detroit School, Kent Beck)
// - Keine Interaktion zwischen getesteter Einheit und dem Rest des Systems?
//   - Abhängigkeiten werden durch einfache Simulationen ersetzt (Test-Doubles)
//   - London School

// %% [markdown]
//
// ## Isolierte Testfälle (Detroit School)
//
// - Jeder Testfall ist unabhängig von den anderen
// - Tests können in beliebiger Reihenfolge ausgeführt werden
// - Tests können parallel ausgeführt werden

// %% [markdown]
//
// ### Gegenbeispiel: Nicht isolierte Testfälle

// %%
using System;

// %%
#r "nuget: NUnit, *"
#load "NUnitTestRunner.cs"
using NUnit.Framework;
using static NUnitTestRunner;

// %%
public static class EventTrigger
{
    public static readonly TimeSpan Cooldown = TimeSpan.FromSeconds(1);
    public static DateTime LastTriggeredAt = DateTime.MinValue;

    public static bool CanTrigger()
    {
        return DateTime.Now >= LastTriggeredAt + Cooldown;
    }

    public static bool Trigger()
    {
        DateTime currentTime = DateTime.Now;
        DateTime nextAllowed = LastTriggeredAt + Cooldown;
        Console.WriteLine($"  Current time:       {currentTime:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine($"  Triggering allowed: {nextAllowed:yyyy-MM-dd HH:mm:ss}");
        if (!CanTrigger())
            return false;
        LastTriggeredAt = currentTime;
        return true;
    }
}

// %%

// %%

// %%

// %%
void RunTest(string name, Action test)
{
    try
    {
        test();
        Console.WriteLine($"{name}: passed");
    }
    catch (Exception)
    {
        Console.WriteLine($"{name}: FAILED");
    }
}

// %%

// %%

// %% [markdown]
//
// ## Gründe für nicht isolierte Testfälle
//
// - Veränderlicher globaler/statischer Zustand
// - Veränderliche externe Ressourcen (Dateien, Datenbanken, Netzwerk, ...)

// %% [markdown]
//
// ## Isolation der getesteten Unit (London School)
//
// - Die getestete Unit wird von allen anderen Units isoliert
// - Test-Doubles für alle Abhängigkeiten

// %% [markdown]
//
// ## Wann sind Test-Doubles sinnvoll?
//
// - Beide Schulen verwenden Test-Doubles für:
//   - Nicht-deterministische Abhängigkeiten (Zeit, Zufallszahlen)
//   - Langsame Abhängigkeiten (Datenbank, Netzwerk, Dateisystem)
//   - Externe Systeme (APIs, Services)
// - London School geht weiter:
//   - *Alle* veränderlichen Abhängigkeiten ersetzen

// %% [markdown]
//
// ## Wie geht es weiter?
//
// - Taxonomie der Test-Doubles (Dummies, Stubs, Fakes, Spies, Mocks)
// - Das Moq-Framework zum effizienten Erstellen von Test-Doubles
// - Dann: Zurück zur London School mit einem konkreten Beispiel
