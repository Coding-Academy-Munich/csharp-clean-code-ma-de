// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Refactoring mit KI-Werkzeugen</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// ## KI-gestütztes Refactoring
//
// - Moderne KI-Werkzeuge können beim Refactoring helfen
// - Beispiele: GitHub Copilot, Claude, ChatGPT, Cursor
// - KI kann Code-Probleme erkennen und Verbesserungen vorschlagen
// - Aber: KI ist ein Werkzeug, kein Ersatz für Verständnis!

// %% [markdown]
//
// ## Einsatzgebiete
//
// - **Code Smells erkennen**: "Welche Probleme hat dieser Code?"
// - **Refactoring vorschlagen**: "Wie kann ich diesen Code verbessern?"
// - **Code transformieren**: "Refaktorisiere diese Methode"
// - **Tests generieren**: "Schreibe Tests für diese Klasse"
// - **Code erklären**: "Was macht dieser Code?"

// %% [markdown]
//
// ## Der Workflow: KI + Tests
//
// 1. **Tests vorhanden?** Wenn nicht: erst Tests schreiben (KI kann helfen)
// 2. **KI fragen**: Code analysieren lassen, Verbesserungen vorschlagen
// 3. **Vorschlag prüfen**: Verstehen, was die KI vorschlägt und warum
// 4. **Änderung anwenden**: KI-Vorschlag übernehmen oder anpassen
// 5. **Tests ausführen**: Sicherstellen, dass alles noch funktioniert
// 6. **Committen**: Kleine, getestete Schritte

// %% [markdown]
//
// ## Kritisch: Tests sind unverzichtbar!
//
// - KI-generierter Code kann subtile Fehler enthalten
// - Die KI "versteht" den Kontext nicht vollständig
// - Tests sind das Sicherheitsnetz
// - **Ohne Tests ist KI-gestütztes Refactoring gefährlich!**

// %% [markdown]
//
// ## Beispiel: Code Smells erkennen
//
// Prompt: "Welche Code Smells hat dieser Code?"

// %%
using System;
using System.Collections.Generic;

// %%
class Report
{
    public string type;
    public List<string> data;
    public bool sent;

    public string Generate()
    {
        string result = "";
        if (type == "html")
        {
            result = "<html><body>";
            foreach (var d in data)
                result += "<p>" + d + "</p>";
            result += "</body></html>";
        }
        else if (type == "csv")
        {
            foreach (var d in data)
                result += d + ",";
            result = result.TrimEnd(',');
        }
        else if (type == "json")
        {
            result = "[";
            foreach (var d in data)
                result += "\"" + d + "\",";
            result = result.TrimEnd(',') + "]";
        }
        sent = false;
        return result;
    }
}

// %% [markdown]
//
// KI-Analyse identifiziert:
//
// - **Long Method**: `Generate()` zu lang, macht zu viele Dinge
// - **Switch Statements**: if/else-Kette auf String-Typ
// - **Primitive Obsession**: `type` als String statt Enum/Polymorphismus
// - **Feature Envy**: Formatierungslogik gehört nicht in `Report`
// - **Mutable State**: `sent`-Flag als öffentliches Feld

// %% [markdown]
//
// ## Beispiel: KI-vorgeschlagenes Refactoring

// %%
interface IReportFormatter
{
    string Format(List<string> data);
}

class HtmlFormatter : IReportFormatter
{
    public string Format(List<string> data)
        => $"<html><body>{string.Join("", data.Select(d => $"<p>{d}</p>"))}</body></html>";
}

class CsvFormatter : IReportFormatter
{
    public string Format(List<string> data)
        => string.Join(",", data);
}

class JsonFormatter : IReportFormatter
{
    public string Format(List<string> data)
        => $"[{string.Join(",", data.Select(d => $"\"{d}\""))}]";
}

// %%
class Report
{
    private readonly List<string> data;
    private readonly IReportFormatter formatter;
    public bool HasBeenSent { get; private set; }

    public Report(List<string> data, IReportFormatter formatter)
    {
        this.data = data;
        this.formatter = formatter;
        HasBeenSent = false;
    }

    public string Generate() => formatter.Format(data);
}

// %% [markdown]
//
// ## Grenzen von KI-Refactoring
//
// - KI kann den Geschäftskontext nicht vollständig verstehen
// - Vorschläge können Over-Engineering sein
// - KI kann subtile Verhaltensänderungen einführen
// - Nicht blind vertrauen: immer den Vorschlag verstehen!
// - Am besten für: Routine-Refactorings, Namensfindung, Patterns

// %% [markdown]
//
// ## Best Practices
//
// - Immer Tests haben, bevor man KI-Refactoring anwendet
// - KI-Vorschläge als Inspiration verwenden, nicht als finale Lösung
// - Kleine Schritte: ein Refactoring auf einmal
// - Code-Review für KI-generierte Änderungen
// - KI für Routine-Aufgaben nutzen, kreative Entscheidungen selbst treffen

// %% [markdown]
//
// ## Workshop: KI-gestütztes Refactoring
//
// Verwenden Sie ein KI-Werkzeug Ihrer Wahl, um den folgenden Code
// zu verbessern:
//
// 1. Bitten Sie die KI, Code Smells zu identifizieren
// 2. Lassen Sie sich Refactoring-Vorschläge machen
// 3. Bewerten Sie die Vorschläge kritisch
// 4. Wenden Sie die besten Vorschläge an
// 5. Stellen Sie sicher, dass der Code weiterhin korrekt funktioniert
//
// *Diskutieren Sie: Welche Vorschläge waren hilfreich? Welche nicht?*

// %%
using System;
using System.Collections.Generic;

// %%
class Shop
{
    public List<(string name, double price, int qty, string cat)> items = new();
    public double disc = 0;

    public double CalcTotal()
    {
        double t = 0;
        for (int i = 0; i < items.Count; i++)
        {
            double p = items[i].price * items[i].qty;
            if (items[i].cat == "food")
                p = p * 1.07;
            else if (items[i].cat == "electronics")
                p = p * 1.19;
            else
                p = p * 1.19;
            t += p;
        }
        if (t > 100) disc = 0.1;
        else if (t > 50) disc = 0.05;
        else disc = 0;
        t = t * (1 - disc);
        Console.WriteLine($"Total: {t:C}");
        return t;
    }
}

// %% [markdown]
//
// Mögliche Verbesserungen (die eine KI vorschlagen könnte):

// %%
record ShopItem(string Name, double Price, int Quantity, string Category);

class ShoppingCart
{
    private readonly List<ShopItem> items = new();
    private static readonly Dictionary<string, double> TaxRates = new()
    {
        ["food"] = 0.07,
        ["electronics"] = 0.19
    };
    private const double DefaultTaxRate = 0.19;

    public void AddItem(ShopItem item) => items.Add(item);

    public double CalculateTotal()
    {
        double subtotalWithTax = items.Sum(item => CalculateItemTotal(item));
        double discountRate = DetermineDiscountRate(subtotalWithTax);
        return subtotalWithTax * (1 - discountRate);
    }

    private double CalculateItemTotal(ShopItem item)
    {
        double taxRate = TaxRates.GetValueOrDefault(item.Category, DefaultTaxRate);
        return item.Price * item.Quantity * (1 + taxRate);
    }

    private double DetermineDiscountRate(double total)
        => total > 100 ? 0.10 : total > 50 ? 0.05 : 0;
}
