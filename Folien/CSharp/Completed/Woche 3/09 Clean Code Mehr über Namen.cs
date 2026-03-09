// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Clean Code: Mehr über Namen</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
//
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>


// %% [markdown]
//
// ## Namensregeln im Überblick
//
// Gute Namen
//
// - sind selbsterklärend
// - sind aussprechbar
// - beschreiben das Problem, nicht die Implementierung
// - vermeiden Kodierungen und Füllwörter
// - verwenden die richtige Wortart
// - vermeiden Disinformation und benennen sinnvolle Unterscheidungen
// - verwenden die Regeln für Umfang und Länge

// %% [markdown]
//
// ## Selbsterklärende Namen
//
// - Der Name sollte alles sagen, was man wissen muss
// - Kein Kommentar nötig, um den Zweck zu verstehen

// %%
int d = 0;

// %%
int elapsedTimeInDays = 0;

// %% [markdown]
//
// ## Aussprechbare Namen
//
// - Aussprechbare Namen sind leichter zu merken und zu diskutieren
// - Oft sind sie auch besser zu suchen

// %%
List<int> hwCrsrPxy = new List<int> { 0, 0 };

// %%
List<int> hardwareCursorPosition = new List<int> { 0, 0 };

// %% [markdown]
//
// ## Beschreibe Problem, nicht Implementierung
//
// Vermeide Namen, die sich auf Implementierungsdetails beziehen:
// - Die Kommunikation der Intention hat höchste Priorität!

// %%
int AddElements(List<int> lst)
{
    return lst.Sum();
}

// %%
int ComputeTotalInventory(List<int> warehouseStockCounts)
{
    return warehouseStockCounts.Sum();
}

// %% [markdown]
//
// ## Vermeide Kodierungen
//
// - Keine ungarische Notation: `i_days`, `str_name`
// - Keine Member-Präfixe: `m_X`, `m_Y`
// - C#-Ausnahme: Interfaces beginnen mit `I` (z.B. `IEnumerable`)

// %%
public class PointBad
{
    public int m_X;
    public int m_Y;
}

// %%
public class PointGood
{
    public int X { get; set; }
    public int Y { get; set; }
}

// %% [markdown]
//
// ### C#-Namenskonventionen
//
// - PascalCase für Klassen, Methoden, Properties
// - camelCase für lokale Variablen und Parameter
// - Interfaces beginnen mit `I`
// - Siehe: [C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

// %% [markdown]
//
// ## Verwende die richtige Wortart
//
// - Klassen und Variablen: Substantive (`ServerConnection`)
// - Methoden: Verben (`Connect()`, `ProcessInput()`)
// - Enums: oft Adjektive
// - Boolesche Werte: Prädikate (`IsAvailable`, `HasPermission`)

// %%
public class GoToTheServer
{
    public void Connection() { }
    public bool ServerAvailability() { return true; }
}

// %%
public class ServerConnection
{
    public void Connect() { }
    public bool IsServerAvailable() { return true; }
}

// %% [markdown]
//
// ## Vermeide Füllwörter
//
// Vermeide bedeutungslose Wörter: Manager, Processor, Data, Info

// %%
class DataInfoManager
{
    private string strName;
    private int iAge;
}

// %%
class Person
{
    private string Name;
    private int Age;
}

// %% [markdown]
//
// ## Vermeide Disinformation
//
// - Der Name darf nicht etwas anderes implizieren als der Code tut
// - Keinen Typ in den Namen aufnehmen, wenn er nicht stimmt
// - Vorsicht bei Namen, die sich nur minimal unterscheiden
// - Konsistent sein bei der Namensgebung

// %%
bool verifyConfiguration = false;
if (verifyConfiguration)
{
    Console.WriteLine("Deleting configuration files...");
}

// %%
int vectorOfCards = 0;    // Not a vector!
List<int> cardArray = new List<int>();  // Not an array!

// %%
List<int> cards = new List<int>();  // Better

// %% [markdown]
//
// ## Sinnvolle Unterscheidungen
//
// - Verwende Namen, die die Bedeutung klar ausdrücken
// - Verwende denselben Namen für dasselbe Konzept

// %%
string a1 = "Fluffy";
string a2 = "Garfield";

// %%
string myDog = "Fluffy";
string jonsCat = "Garfield";

// %%
using System.IO;

// %% [markdown]
//
// Inkonsistent:

// %%
string myPath = Path.Combine("my-data");
string yourDir = Path.Combine("your-data");
string fileLoc = Path.Combine("file-loc");

// %% [markdown]
//
// Konsistent:

// %%
string myPath = Path.Combine("my-data");
string yourPath = Path.Combine("your-data");
string filePath = Path.Combine("file-loc");

// %% [markdown]
//
// ## Regeln für Umfang und Länge
//
// - Langer Geltungsbereich = langer Name
// - Kurzer Geltungsbereich = kurzer Name ist OK
//
// **Faustregel:** Verwende lange, beschreibende Namen für lange Geltungsbereiche

// %%
for (int theLoopCounterOfThisForLoop = 0; theLoopCounterOfThisForLoop < 10; theLoopCounterOfThisForLoop++)
{
    Console.WriteLine($"{theLoopCounterOfThisForLoop}^2 = {theLoopCounterOfThisForLoop * theLoopCounterOfThisForLoop}");
}

// %%
for (int i = 0; i < 10; i++)
{
    Console.WriteLine($"{i}^2 = {i * i}");
}

// %%
class P
{
    public double X { get; set; }
    public double Y { get; set; }
}

// %%
class Point
{
    public double X { get; set; }
    public double Y { get; set; }
}

// %%
class FixedSizeOrderedCollectionIndexedByInts { }

// %%
class Array { }

// %% [markdown]
//
// ## Workshop: Namensverbesserung
//
// Verbessern Sie die Namen im folgenden Code. Identifizieren Sie,
// welche Namensregeln verletzt werden, und schreiben Sie den Code um.

// %%
class dtprcssor
{
    private List<int> lst;
    private bool flag;

    public dtprcssor(List<int> d)
    {
        lst = d;
        flag = false;
    }

    public int Calc()
    {
        int r = 0;
        foreach (var i in lst) { r += i; }
        flag = true;
        return r;
    }

    public bool getflag() { return flag; }
}

// %%
class SalesReportCalculator
{
    private List<int> monthlySales;
    private bool hasBeenCalculated;

    public SalesReportCalculator(List<int> monthlySales)
    {
        this.monthlySales = monthlySales;
        hasBeenCalculated = false;
    }

    public int ComputeTotal()
    {
        int total = 0;
        foreach (var amount in monthlySales) { total += amount; }
        hasBeenCalculated = true;
        return total;
    }

    public bool HasBeenCalculated => hasBeenCalculated;
}

// %% [markdown]
//
// - Klasse: Substantiv, aussprechbar, beschreibt den Zweck
// - Attribute: beschreiben ihren Inhalt, Bool als Prädikat
// - Methode: Verb, beschreibt was sie tut
// - Property statt Getter-Methode (C#-Konvention)

// %%
int computeSecondsInADay = 24 * 60 * 60;

// %%
int secondsPerDay = 24 * 60 * 60;

// %% [markdown]
//
// Falsche Wortart: `computeSecondsInADay` klingt wie eine Methode, aber es ist
// eine Variable

// %%
string s = "admin"; // Default role for new users

// %%
string defaultUserRole = "admin";

// %% [markdown]
//
// Kurzer Name wird in einem großen Geltungsbereich verwendet, beschreibt aber
// nicht, was er repräsentiert

// %%
bool b_FtrFlg = true; // Feature flag: If true, the feature is enabled

// %%
bool isFeatureEnabled = true;

// %% [markdown]
//
// Ungarische Notation (`b_`), unverständlicher Name (`FtrFlg`), und der
// Kommentar ist nötig, um zu verstehen, was die Variable bedeutet

// %%
List<string> theList = new List<string> { "read", "write", "execute" };

// %%
List<string> permissions = new List<string> { "read", "write", "execute" };

// %% [markdown]
//
// `theList` sagt nichts über den Inhalt aus — was ist das für eine Liste?
