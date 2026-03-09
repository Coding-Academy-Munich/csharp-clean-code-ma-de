// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Code Smells</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
//
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>


// %% [markdown]
//
// ## Was sind Code Smells?
//
// - Hinweise darauf, dass Code verbessert werden sollte
// - Keine Bugs, aber Anzeichen für tiefere Probleme
// - Zeigen an, wo Refactoring sinnvoll wäre

// %% [markdown]
//
// ## Smells aus den Workshops (1)
//
// - **Mysteriöser Name**: Namen verschleiern statt zu klären
//   - IDE-Workshop: `Mgr`, `Calc`, `n`, `s`, `y`
// - **Lange Funktion**: Funktion macht zu viele Dinge
//   - KI-Workshop: `Report.Generate()`
// - **Feature-Neid**: Logik gehört in eine andere Klasse
//   - KI-Workshop: Formatierungslogik in `Report`

// %% [markdown]
//
// ## Smells aus den Workshops (2)
//
// - **Primitive Obsession**: Eingebaute Typen statt sinnvoller Klassen
//   - KI-Workshop: `type` als String statt Enum/Polymorphismus
// - **Switch Statements**: if/else-Ketten über die gleichen Daten
//   - KI-Workshop: if/else-Kette auf String-Typ in `Report`
// - **Veränderliche Daten**: Unkontrollierter Zugriff auf veränderliche Daten
//   - KI-Workshop: `sent`-Flag als öffentliches Feld

// %% [markdown]
//
// ## Weitere wichtige Smells (1)
//
// - **Duplizierter Code**: Identischer oder ähnlicher Code an mehreren Stellen
//   - Refactoring: Extrahiere Funktion, Extrahiere Klasse
// - **Lange Parameterliste**: Funktion braucht zu viele Parameter
//   - Refactoring: Introduce Parameter Object, Preserve Whole Object
// - **Globale Daten**: Daten, auf die jeder zugreifen kann
//   - Refactoring: Encapsulate Variable

// %% [markdown]
//
// ## Weitere wichtige Smells (2)
//
// - **Datenklumpen**: Gleiche Daten erscheinen immer zusammen
//   - Refactoring: Introduce Parameter Object, Extract Class
// - **Divergente Änderung**: Ein Modul ändert sich bei vielen verschiedenen
//   Anforderungen
//   - Refactoring: Extract Class, Split Phase
// - **Schrotflinten-Operation**: Einfache Änderungen betreffen viele Module
//   - Refactoring: Move Function, Combine Functions into Class

// %% [markdown]
//
// ## Smells in der Klassenstruktur (1)
//
// - **Faules Element**: Programmelemente, die keinen Mehrwert haben
//   - Refactoring: Inline Function, Collapse Hierarchy
// - **Spekulative Allgemeinheit**: Flexibilität, die momentan nicht benötigt
//   wird (YAGNI)
//   - Refactoring: Collapse Hierarchy, Remove Dead Code
// - **Große Klasse**: Klasse hat zu viele Verantwortlichkeiten
//   - Refactoring: Extract Class, Extract Subclass

// %% [markdown]
//
// ## Smells in der Klassenstruktur (2)
//
// - **Datenklasse**: Klasse hat nur Getter und Setter, keine Logik
//   - Refactoring: Move Function (Logik in die Klasse verschieben)
// - **Verweigerte Erbschaft**: Unterklassen erben Funktionalität, die sie
//   nicht brauchen
//   - Refactoring: Replace Inheritance with Delegation, Strategy Pattern

// %% [markdown]
//
// ## Weitere Smells (1)
//
// - **Temporäres Feld**: Attribute, die nur zeitweise gültig sind
// - **Ketten von Nachrichten**: Lange Ketten von Getter-Aufrufen
// - **Mittelsmann**: Klasse delegiert nur an eine andere Klasse
// - **Insider Trading**: Klassen kennen zu viele Interna anderer Klassen

// %% [markdown]
//
// ## Weitere Smells (2)
//
// - **Alternative Klassen mit unterschiedlichen Interfaces**: Klassen, die
//   austauschbar sein sollten, aber nicht sind
// - **Schleifen**: Können oft durch LINQ ersetzt werden
// - **Kommentare**: Falls sie nur da sind, weil der Code schlecht ist

// %% [markdown]
//
// ## Workshop: Code Smells erkennen
//
// Welche Code Smells finden Sie im folgenden Code?

// %%
public static class App
{
    public static List<string> data = new List<string>();

    public static string Process(string a, string b, int c, bool d, string e)
    {
        string result = "";
        if (c == 1)
        {
            result = a + " " + b;
        }
        else if (c == 2)
        {
            result = a + ", " + b;
        }
        else if (c == 3)
        {
            result = b + ": " + a;
        }
        data.Add(result);
        return result;
    }
}

// %% [markdown]
// *Antwort:* 
// - **Mysteriöser Name**: `App`, `Process`, `a`, `b`, `c`, `d`, `e`, `data`
// - **Lange Parameterliste**: 5 Parameter
// - **Primitive Obsession**: `c` als int statt Enum/Polymorphismus
// - **Switch Statements**: if/else-Kette auf `c`
// - **Globale Daten**: `data` als öffentliches statisches Feld
// - **Veränderliche Daten**: `data` kann von überall geändert werden
// - **Kommentare**: Keine Kommentare, aber der Code bräuchte welche (oder
//   bessere Namen)
