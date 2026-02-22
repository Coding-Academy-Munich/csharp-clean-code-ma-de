// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Clean Code: Funktionen</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// ## Funktionen als Strukturierungsmittel
//
// Fasse logisch zusammengehörige Operationen als sorgfältig benannte
// Funktionen zusammen:
//
// - Besser lesbar
// - Einfacher zu testen
// - Weniger fehleranfällig
// - Eher wiederverwendbar

// %% [markdown]
//
// ## Funktionen sollten kurz sein
//
// - Funktionen sollten auf einen Bildschirm passen
// - 1 bis 5 Zeilen sind normal
// - Große Funktionen in kleinere, zusammenhängende Funktionen aufteilen
// - Nicht dogmatisch sein: 20-30 Zeilen sind manchmal OK

// %% [markdown]
//
// ## "Do One Thing"
//
// - Funktionen sollten eine Aufgabe erfüllen
// - Sie sollten diese Aufgabe gut erfüllen
// - Sie sollten nur diese Aufgabe erfüllen

// %%
using System;
using System.Collections.Generic;

// %%
void DoStuff(int a, int b, List<int> results)
{
    int measurement = a + b;
    int newResult = measurement + 1;
    if (newResult > 0)
    {
        results.Add(newResult);
    }
    foreach (int result in results)
    {
        Console.WriteLine(result);
    }
}

// %% [markdown]
//
// Besser: Aufteilen in einzelne Funktionen

// %%
int GetMeasurement(int a, int b) => a + b;

int ComputeResult(int measurement) => measurement + 1;

bool IsValidResult(int result) => result > 0;

void SaveResult(int result, List<int> results) => results.Add(result);

void PrintResults(List<int> results)
{
    foreach (int result in results)
        Console.WriteLine(result);
}

// %%
void ProcessMeasurement(int a, int b, List<int> results)
{
    int measurement = GetMeasurement(a, b);
    int newResult = ComputeResult(measurement);
    if (IsValidResult(newResult))
        SaveResult(newResult, results);
    PrintResults(results);
}

// %% [markdown]
//
// ## Abstraktionsebenen
//
// - Alle Anweisungen in einer Funktion sollten auf der gleichen
//   Abstraktionsebene sein
// - High-Level-Funktionen rufen Low-Level-Funktionen auf
// - Nicht mischen!

// %%
class Order { }
void ValidateOrder(Order o) { }
decimal CalculateTotal(Order o) => 0m;
void ApplyDiscount(Order o, decimal t) { }
void SendConfirmation(Order o) { }

// %%
void ProcessOrder(Order order)
{
    ValidateOrder(order);
    decimal total = CalculateTotal(order);
    ApplyDiscount(order, total);
    SendConfirmation(order);
}

// %% [markdown]
//
// ## Funktionsparameter minimieren
//
// - Idealerweise: 0 Parameter (niladische Funktion)
// - 1 Parameter (monadisch) ist gut
// - 2 Parameter (dyadisch) sind akzeptabel
// - 3+ Parameter vermeiden
// - Viele Parameter → Parameterobjekt einführen

// %%
void CreateUser(string firstName, string lastName, string email,
    string phone, string street, string city, string zip)
{
    // Too many parameters!
}

// %%
record UserData(string FirstName, string LastName, string Email,
    string Phone, string Street, string City, string Zip);

void CreateUser(UserData userData)
{
    // Much better: one parameter object
}

// %% [markdown]
//
// ## Vermeide versteckte Seiteneffekte
//
// - Der Name einer Funktion soll alles beschreiben, was sie tut
// - Keine versteckten Zustandsänderungen
// - Keine überraschenden Nebeneffekte

// %%
class User
{
    public bool IsValidPassword(string p) => true;
    public void InitializeNewSession() { }
}
User FindUser(string name) => new User();

// %%
bool CheckPassword(string userName, string password)
{
    var user = FindUser(userName);
    if (user != null && user.IsValidPassword(password))
    {
        user.InitializeNewSession();  // Hidden side effect!
        return true;
    }
    return false;
}

// %% [markdown]
//
// Besser: Seiteneffekt im Namen sichtbar machen oder trennen

// %%
bool ValidatePassword(string userName, string password)
{
    var user = FindUser(userName);
    return user != null && user.IsValidPassword(password);
}

void StartSession(string userName)
{
    var user = FindUser(userName);
    user?.InitializeNewSession();
}

// %% [markdown]
//
// ## Command-Query Separation
//
// - Funktionen sollten entweder etwas **tun** (Command)
//   oder etwas **zurückgeben** (Query)
// - Nicht beides!

// %%
int defaultValue = -1;

// %%
bool HasDefaultValue()
{
    if (defaultValue >= 0)
        return true;
    defaultValue = 123;   // Side effect! Violates CQS
    return false;
}

// %%
// Better: separate command and query
bool HasDefaultValue() => defaultValue >= 0;

void SetDefaultValue(int value) { defaultValue = value; }

// %% [markdown]
//
// ## Zusammenfassung
//
// - Funktionen sollten kurz sein und eine Aufgabe erfüllen
// - Gleiche Abstraktionsebene innerhalb einer Funktion
// - Parameter minimieren (max. 2-3)
// - Keine versteckten Seiteneffekte
// - Command-Query Separation beachten

// %% [markdown]
//
// ## Workshop: Funktionen verbessern
//
// Refaktorisieren Sie die folgende Funktion. Wenden Sie die Clean Code
// Regeln für Funktionen an:
// - Aufteilen in kleinere Funktionen
// - Jede Funktion erfüllt eine Aufgabe
// - Beschreibende Namen
// - Seiteneffekte eliminieren

// %%
using System;
using System.Collections.Generic;
using System.Linq;

// %%
static double ProcessData(List<double> data, bool print)
{
    // Remove invalid values
    List<double> clean = new List<double>();
    foreach (var d in data)
    {
        if (d >= 0 && d <= 1000)
            clean.Add(d);
    }
    // Calculate average
    double sum = 0;
    foreach (var d in clean)
        sum += d;
    double avg = clean.Count > 0 ? sum / clean.Count : 0;
    // Print if requested
    if (print)
    {
        Console.WriteLine($"Processed {clean.Count} values, average: {avg:F2}");
    }
    // Side effect: modify input list
    data.Clear();
    data.AddRange(clean);
    return avg;
}

// %%
var testData = new List<double> { 10, -5, 200, 1500, 42, 88 };
ProcessData(testData, true)

// %%

// %%
