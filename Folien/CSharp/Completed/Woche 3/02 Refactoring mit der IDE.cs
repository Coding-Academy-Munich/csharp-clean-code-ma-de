// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Refactoring mit der IDE</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
//
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>


// %% [markdown]
//
// ## Warum IDE-Refactoring?
//
// - IDEs können viele Refactorings automatisch durchführen
// - Sicherer als manuelles Refactoring
//   - Findet alle Verwendungsstellen
//   - Berücksichtigt Namensräume und Typen
//   - Erkennt Konflikte vor der Änderung
// - Schneller als manuelles Suchen und Ersetzen
// - Vorschau-Funktion zeigt Änderungen vor dem Anwenden

// %% [markdown]
//
// ## Verfügbare IDEs für C#
//
// - **JetBrains Rider**: Umfangreichste Refactoring-Unterstützung
// - **Visual Studio**: Gute Refactoring-Tools, besonders mit ReSharper
// - **VS Code**: Grundlegende Refactorings mit C# Dev Kit
//
// Die Konzepte sind in allen IDEs gleich, nur die Tastenkürzel und Anzahl der
// Refactorings unterscheiden sich.

// %% [markdown]
//
// ## Die wichtigsten IDE-Refactorings
//
// | Refactoring          | Rider            | Visual Studio  | VS + Resharper |
// |----------------------|------------------|----------------|----------------|
// | Umbenennen           | Shift+F6         | Ctrl+R, R       | Ctrl+R, R     |
// | Funktion extrahieren | Ctrl+Alt+M       | Ctrl+R, M       | Ctrl+R, M     |
// | Variable extrahieren | Ctrl+Alt+V       | ❌             | Ctrl+R, V     |
// | Inline               | Ctrl+Alt+N       | ❌             | Ctrl+R, I     |
// | Methode verschieben  | F6               | ❌             | Ctrl+R, O     |
// | Quick Actions        | Alt+Enter        | Ctrl+.         | Alt+Enter      |
// | Refactor This        | Ctrl+Alt+Shift+T | ❌             | Ctrl+Shift+R  |

// %% [markdown]
//
// ## Beispielprojekte
//
// - Startcode: `RefactoringWithIdeNUnitSk`
//   - Pfad: `code/StarterKits/RefactoringWithIdeNUnitSk/`
// - Fertiger Code: `RefactoringWithIdeNUnit`
//   - Pfad: `code/Completed/RefactoringWithIdeNUnit/`

// %% [markdown]
//
// ## Rename (Umbenennen)
//
// - Das am häufigsten verwendete Refactoring
// - Benennt Symbol an allen Verwendungsstellen um
// - Funktioniert für: Variablen, Methoden, Klassen, Namespaces, Properties
// - Erkennt automatisch Referenzen in Strings und Kommentaren

// %%
using System;
using System.Collections.Generic;
using System.Linq;

// %%
class Calc
{
    public double Calc1(List<double> d)
    {
        double s = 0;
        foreach (var x in d) s += x;
        return d.Count > 0 ? s / d.Count : 0;
    }

    public double Calc2(List<double> d)
    {
        double s = 0;
        foreach (var x in d)
            s += (x - Calc1(d)) * (x - Calc1(d));
        return d.Count > 1 ? s / (d.Count - 1) : 0;
    }
}

// %% [markdown]
//
// Nach Rename-Refactoring (Shift+F6 in Rider):
//
// ```csharp
// class StatisticsCalculator
// {
//     public double CalculateMean(List<double> values)
//     {
//         double sum = 0;
//         foreach (var value in values) sum += value;
//         return values.Count > 0 ? sum / values.Count : 0;
//     }
//
//     public double CalculateVariance(List<double> values)
//     {
//         double mean = CalculateMean(values);
//         double sumOfSquares = 0;
//         foreach (var value in values)
//             sumOfSquares += (value - mean) * (value - mean);
//         return values.Count > 1 ? sumOfSquares / (values.Count - 1) : 0;
//     }
// }
// ```


// %% [markdown]
//
// ## Extract Method (Methode extrahieren)
//
// - Wähle Code-Block aus → IDE erstellt neue Methode
// - IDE erkennt automatisch:
//   - Benötigte Parameter
//   - Rückgabewert
//   - Lokale Variablen
// - Ersetzt den Code-Block durch den Methodenaufruf

// %%
class OrderItem
{
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

class Order
{
    public List<OrderItem> Items { get; set; } = new();
    public decimal Total { get; set; }
}

// %%
void ProcessOrders(List<Order> orders)
{
    foreach (var order in orders)
    {
        // This block can be extracted
        decimal subtotal = 0;
        foreach (var item in order.Items)
        {
            subtotal += item.Price * item.Quantity;
        }
        decimal tax = subtotal * 0.19m;
        decimal total = subtotal + tax;
        order.Total = total;
    }
}

// %% [markdown]
//
// Nach Extract Method:
//
// ```csharp
// void ProcessOrders(List<Order> orders)
// {
//     foreach (var order in orders)
//     {
//         order.Total = CalculateOrderTotal(order);
//     }
// }
//
// decimal CalculateOrderTotal(Order order)
// {
//     decimal subtotal = order.Items.Sum(i => i.Price * i.Quantity);
//     decimal tax = subtotal * 0.19m;
//     return subtotal + tax;
// }
// ```

// %% [markdown]
//
// ## Extract Variable (Variable extrahieren)
//
// - Ersetzt einen Ausdruck durch eine benannte Variable
// - Macht komplexe Bedingungen lesbarer
// - Alle Vorkommen des gleichen Ausdrucks werden ersetzt

// %%
var employee = new { Age = 60, YearsOfService = 35 };

// %%
if (employee.Age >= 65 || (employee.YearsOfService >= 30 && employee.Age >= 55))
{
    // eligible for retirement
}

// %% [markdown]
//
// Nach Extract Variable:
//
// ```csharp
// bool isRegularRetirement = employee.Age >= 65;
// bool isEarlyRetirement = employee.YearsOfService >= 30 && employee.Age >= 55;
//
// if (isRegularRetirement || isEarlyRetirement)
// {
//     // eligible for retirement
// }
// ```

// %% [markdown]
//
// ## Inline (Inlining)
//
// - Das Gegenteil von Extract
// - Ersetzt einen Methodenaufruf durch den Methodeninhalt
// - Ersetzt eine Variable durch ihren Wert
// - Nützlich wenn eine Abstraktion nicht hilfreich ist

// %%
class Fmt
{
    private decimal GetTr() { return 0.19m; }

    private string GetLbl(int t)
    {
        return t == 1 ? "Subtotal" : t == 2 ? "Tax" : "Total";
    }

    public string Fmt1(string d, decimal p, int q)
    {
        decimal lt = p * q;
        return $"{d,-30} {q,5} x {p,10:C} = {lt,12:C}";
    }

    public string Fmt2(List<(string d, decimal p, int q)> items)
    {
        decimal t = 0;
        foreach (var i in items) t += i.p * i.q;
        decimal tx = t * GetTr();
        return $"{GetLbl(1)}: {t:C}\n{GetLbl(2)} ({GetTr() * 100}%): {tx:C}\n{GetLbl(3)}: {t + tx:C}";
    }
}

// %% [markdown]
//
// Nach Rename und Inline:
//
// ```csharp
// class InvoiceFormatter
// {
//     public string FormatInvoiceLine(string description, decimal unitPrice,
//         int quantity)
//     {
//         decimal lineTotal = unitPrice * quantity;
//         return $"{description,-30} {quantity,5} x {unitPrice,10:C} = {lineTotal,12:C}";
//     }
//
//     public string FormatTotal(
//         List<(string Description, decimal UnitPrice, int Quantity)> items)
//     {
//         decimal total = items.Sum(item => item.UnitPrice * item.Quantity);
//         decimal tax = total * 0.19m;
//         return $"Subtotal: {total:C}\nTax (19%): {tax:C}\nTotal: {total + tax:C}";
//     }
// }
// ```

// %% [markdown]
//
// ## Workshop: IDE-Refactoring
//
// Öffnen Sie das Projekt `RefactoringWithIdeNUnitSk` in Ihrer IDE.
//
// Verwenden Sie nur IDE-Refactoring-Tools (keine manuellen Änderungen!),
// um den Code in `Mgr.cs` zu verbessern:
//
// 1. Benennen Sie schlecht benannte Elemente um (Rename)
// 2. Extrahieren Sie Methoden für wiederholte Logik (Extract Method)
// 3. Extrahieren Sie Variablen für komplexe Ausdrücke (Extract Variable)
// 4. Führen Sie nach jedem Schritt die Tests aus

// %%
using System;
using System.Collections.Generic;
using System.Linq;

// %%
class Mgr
{
    private List<(string n, double s, int y)> emps = new();

    public void Add(string n, double s, int y) => emps.Add((n, s, y));

    public double Proc()
    {
        double t = 0;
        int c = 0;
        foreach (var e in emps)
        {
            if (e.y >= 2 && e.s > 30000 && e.s < 200000)
            {
                double b = e.s * (e.y > 10 ? 0.15 : e.y > 5 ? 0.10 : 0.05);
                t += b;
                c++;
            }
        }
        return c > 0 ? t / c : 0;
    }
}

// %%
class BonusCalculator
{
    private List<(string Name, double Salary, int YearsOfService)> employees = new();

    public void AddEmployee(string name, double salary, int yearsOfService)
        => employees.Add((name, salary, yearsOfService));

    public double CalculateAverageBonus()
    {
        var eligibleEmployees = employees
            .Where(e => IsEligibleForBonus(e.YearsOfService, e.Salary));

        if (!eligibleEmployees.Any()) return 0;

        return eligibleEmployees
            .Select(e => CalculateBonus(e.Salary, e.YearsOfService))
            .Average();
    }

    private bool IsEligibleForBonus(int yearsOfService, double salary)
        => yearsOfService >= 2 && salary > 30000 && salary < 200000;

    private double CalculateBonus(double salary, int yearsOfService)
    {
        double bonusRate = yearsOfService > 10 ? 0.15
                         : yearsOfService > 5 ? 0.10
                         : 0.05;
        return salary * bonusRate;
    }
}
