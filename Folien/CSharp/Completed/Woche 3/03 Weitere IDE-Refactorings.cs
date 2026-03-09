// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Weitere IDE-Refactorings</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
//
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>


// %% [markdown]
//
// ## Weitere nützliche IDE-Features
//
// - **Safe Delete**: Prüft ob ein Symbol noch verwendet wird
// - **Change Signature**: Ändert Parameter einer Methode überall
// - **Move**: Verschiebt Klassen/Methoden in andere Dateien/Namespaces
// - **Introduce Parameter**: Macht lokale Variable zum Parameter
// - **Code Analysis**: Zeigt Verbesserungsvorschläge (Warnungen, Hints)

// %% [markdown]
//
// ### Change Signature
//
// - Ändert die Signatur einer Methode
// - Fügt Parameter hinzu, entfernt oder benennt sie um
// - Passt alle Aufrufer automatisch an

// %%
using System;

// %%
class Conv
{
    public double C2f(double v)
    {
        return Math.Round(v * 9.0 / 5.0 + 32, 2);
    }

    public double F2c(double v)
    {
        return Math.Round((v - 32) * 5.0 / 9.0, 2);
    }

    public double C2k(double v)
    {
        return Math.Round(v + 273.15, 2);
    }
}

// %% [markdown]
//
// Nach Rename und Change Signature:
//
// ```csharp
// class TemperatureConverter
// {
//     public double CelsiusToFahrenheit(double celsius, int decimalPlaces = 2)
//     {
//         double result = celsius * 9.0 / 5.0 + 32;
//         return Math.Round(result, decimalPlaces);
//     }
//
//     public double FahrenheitToCelsius(double fahrenheit, int decimalPlaces = 2)
//     {
//         double result = (fahrenheit - 32) * 5.0 / 9.0;
//         return Math.Round(result, decimalPlaces);
//     }
//
//     public double CelsiusToKelvin(double celsius, int decimalPlaces = 2)
//     {
//         double result = celsius + 273.15;
//         return Math.Round(result, decimalPlaces);
//     }
// }
// ```

// %% [markdown]
//
// ### Introduce Parameter
//
// - Ersetzt einen hardcodierten Wert durch einen Parameter
// - Fügt automatisch einen Standardwert hinzu
// - Alle Aufrufer werden angepasst

// %%
class Ship
{
    public decimal Calc(decimal tot, decimal w)
    {
        if (tot >= 100m)
            return 0m;

        decimal b = 4.99m;
        decimal ws = w > 5 ? (w - 5) * 1.50m : 0m;
        return b + ws;
    }
}

// %% [markdown]
//
// Nach Rename und Introduce Parameter:
//
// ```csharp
// class ShippingCalculator
// {
//     public decimal CalculateShippingCost(
//         decimal orderTotal, decimal weightKg,
//         decimal freeShippingThreshold = 100m, decimal ratePerKg = 1.50m)
//     {
//         if (orderTotal >= freeShippingThreshold)
//             return 0m;
//
//         decimal baseCost = 4.99m;
//         decimal weightSurcharge = weightKg > 5
//             ? (weightKg - 5) * ratePerKg : 0m;
//         return baseCost + weightSurcharge;
//     }
// }
// ```

// %% [markdown]
//
// ## Tipps für IDE-Refactoring
//
// - Lernen Sie die Tastenkürzel Ihrer IDE!
// - Verwenden Sie die Vorschau vor dem Anwenden
// - Führen Sie nach jedem Refactoring die Tests aus
// - Machen Sie kleine Schritte
// - Nutzen Sie die "Undo"-Funktion wenn etwas schiefgeht

// %% [markdown]
//
// ## Workshop: Change Signature & Introduce Parameter
//
// Öffnen Sie `Dis.cs` im Projekt `RefactoringWithIdeNUnitSk`.
//
// Verwenden Sie IDE-Refactoring-Tools, um den Code zu verbessern:
//
// 1. Benennen Sie schlecht benannte Elemente um (Rename)
// 2. Verwenden Sie Introduce Parameter für hardcodierte Werte
// 3. Verwenden Sie Change Signature für zusätzliche Parameter
// 4. Führen Sie nach jedem Schritt die Tests aus

// %%
using System;

// %%
class Dis
{
    public decimal Calc(decimal p, int q)
    {
        decimal t = p * q;
        if (t > 100m)
            t -= 10m;
        return Math.Round(t, 2);
    }
}

// %%
class DiscountCalculator
{
    public decimal CalculateTotal(decimal unitPrice, int quantity,
        decimal discountThreshold = 100m, decimal discountAmount = 10m,
        int decimalPlaces = 2)
    {
        decimal total = unitPrice * quantity;
        if (total > discountThreshold)
            total -= discountAmount;
        return Math.Round(total, decimalPlaces);
    }
}
