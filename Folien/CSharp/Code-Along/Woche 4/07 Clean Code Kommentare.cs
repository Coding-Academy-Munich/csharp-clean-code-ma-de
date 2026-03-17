// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Clean Code: Kommentare</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
//
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>


// %% [markdown]
//
// ## Kommentare und Code-Qualität
//
// - Kommentare kompensieren unser Unvermögen, uns in Code auszudrücken
// - Wenn möglich: Drücken Sie sich in Code aus, nicht in Kommentaren!
// - Prüfen Sie immer erst, ob Sie es nicht besser im Code tun können

// %% [markdown]
//
// ## Schlechte Kommentare
//
// - Redundante Kommentare (wiederholen den Code)
// - Irreführende Kommentare
// - Auskommentierter Code
// - Verpflichtende Kommentare ("jede Methode braucht einen Kommentar")
// - Journal-Kommentare (dafür gibt es Versionskontrolle)
// - Positions-Marker (`// ======= Section =======`)

// %% [markdown]
//
// Schlecht: redundanter Kommentar

// %%
// Increment counter by one
int counter = 0;
counter++;

// %% [markdown]
//
// Schlecht: irreführender Kommentar

// %%
// Returns true if the user is valid
bool CheckUser(string name)
{
    // Actually deletes invalid users!
    return true;
}

// %% [markdown]
//
// Schlecht: auskommentierter Code

// %%
// int oldValue = 42;
// string oldName = "test";
// DoSomethingOld(oldValue, oldName);
int newValue = 100;

// %% [markdown]
//
// ## Gute Kommentare
//
// - Erklärung der Absicht ("warum", nicht "was")
// - Warnung vor Konsequenzen
// - TODO-Kommentare (temporär)
// - Rechtliche Hinweise
// - Erklärung von regulären Ausdrücken oder komplexen Algorithmen

// %% [markdown]
//
// Gut: erklärt das "Warum", nicht das "Was"

// %%
// We use a 30-second timeout because the external API
// sometimes takes up to 25 seconds under heavy load
int timeoutMs = 30000;

// %% [markdown]
//
// Gut: Warnung vor Konsequenzen

// %%
// WARNING: This test takes 10 minutes to run.
// Only run it in CI, not locally.

// %% [markdown]
//
// Gut: TODO für zukünftige Verbesserung

// %%
// TODO: Replace linear search with binary search when
// the list grows beyond 10,000 elements

// %% [markdown]
//
// ## Besser: Code statt Kommentare
//
// - Verwenden Sie erklärende Variablen und gute Namen statt Kommentare
// - Extrahieren Sie Methoden mit aussagekräftigen Namen
// - Verwenden Sie Enums oder Konstanten statt Magic Numbers
// - Drücken Sie Bedingungen als benannte Prädikate aus

// %%
class Employee
{
    public int YearsOfService { get; set; }
    public int Rating { get; set; }
    public bool HasWarnings { get; set; }
    public double Salary { get; set; }
}

// %%

// %% [markdown]
//
// Mit Kommentaren:

// %%
// Check if employee is eligible for bonus
if (employee.YearsOfService > 5 && employee.Rating >= 4 && !employee.HasWarnings)
{
    // Calculate bonus as 10% of salary
    double bonus = employee.Salary * 0.10;
}

// %% [markdown]
//
// Besser: selbstdokumentierender Code

// %%
bool IsEligibleForBonus(Employee e)
    => e.YearsOfService > 5 && e.Rating >= 4 && !e.HasWarnings;

double CalculateBonus(Employee e)
    => e.Salary * BonusPercentage;

const double BonusPercentage = 0.10;

// %% [markdown]
//
// ## XML-Dokumentation in C#
//
// - C# verwendet `///` XML-Dokumentationskommentare
// - Nützlich für öffentliche APIs und Bibliotheken
// - IDE zeigt sie als Tooltips an
// - Nicht für interne Implementierungsdetails missbrauchen

// %%
/// <summary>
/// Calculates the total price including tax.
/// </summary>
/// <param name="basePrice">The pre-tax price.</param>
/// <param name="taxRate">The tax rate as a decimal (e.g. 0.19).</param>
/// <returns>The total price including tax.</returns>
/// <exception cref="ArgumentException">
/// Thrown when basePrice is negative.
/// </exception>
double CalculateTotalPrice(double basePrice, double taxRate)
{
    if (basePrice < 0) throw new ArgumentException("Price cannot be negative");
    return basePrice * (1 + taxRate);
}

// %% [markdown]
//
// ## Wann XML-Dokumentation?
//
// - Öffentliche API-Methoden und -Klassen
// - Bibliotheken, die von anderen Teams verwendet werden
// - Komplexe Methoden, deren Verhalten nicht offensichtlich ist
// - **Nicht** für triviale Getter/Setter oder interne Methoden

// %% [markdown]
//
// ## Workshop: Kommentare bewerten
//
// - Bewerten Sie die Kommentare im folgenden Code
// - Entscheiden Sie für jeden Kommentar, ob er nützlich, redundant oder
//   irreführend ist
// - Prüfen Sie, ob er durch besseren Code ersetzt werden kann
// - Schreiben Sie den Code um, sodass die Kommentare unnötig werden

// %%
using System;
using System.Collections.Generic;
using System.Linq;

// %%
class DataProcessor
{
    // The list of data
    private List<double> data;
    // The result
    private double result;
    // Flag
    private bool done;

    // Constructor
    public DataProcessor(List<double> d)
    {
        data = d;
        result = 0;
        done = false;
    }

    // Process the data
    public double Process()
    {
        // Filter out negatives
        var filtered = data.Where(x => x >= 0).ToList();
        // Calculate sum
        double sum = filtered.Sum();
        // Divide by count to get average
        result = filtered.Count > 0 ? sum / filtered.Count : 0;
        // Mark as done
        done = true;
        // Return result
        return result;
    }
}

// %% [markdown]
// *Antwort:* 

// %%
