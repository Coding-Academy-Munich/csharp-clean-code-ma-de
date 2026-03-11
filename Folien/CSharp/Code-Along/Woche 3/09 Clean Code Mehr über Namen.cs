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

// %% [markdown]
//
// ## Aussprechbare Namen
//
// - Aussprechbare Namen sind leichter zu merken und zu diskutieren
// - Oft sind sie auch besser zu suchen

// %%
List<int> hwCrsrPxy = new List<int> { 0, 0 };

// %%

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

// %% [markdown]
//
// ## Vermeide Kodierungen
//
// - Keine ungarische Notation: `i_days`, `str_name`
// - Keine Member-Präfixe: `m_X`, `m_Y`
// - C#-Ausnahme: Interfaces beginnen mit `I` (z.B. `IEnumerable`)

// %%
public class Point
{
    private int m_X;
    private int m_Y;

    public int X { get => m_X; }
    public int Y { get => m_Y; }
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

// %% [markdown]
//
// ## Vermeide Füllwörter
//
// Vermeide bedeutungslose Wörter: Manager, Processor, Data, Info

// %%
class PersonDataInfoManager
{
    private string strName;
    private int iAge;
}

// %%

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
    // Delete configuration files
}

// %%
int vectorOfCards = 0;    // Not a vector!

// %%
List<int> cardArray = new List<int>();  // Not an array!

// %%

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

// %%
class P
{
    public double X { get; set; }
    public double Y { get; set; }
}

// %%

// %%
class FixedSizeOrderedCollectionIndexedByInts { }

// %%

// %% [markdown]
//
// ## Workshop: Namensverbesserung
//
// Verbessern Sie die Namen im folgenden Code. Identifizieren Sie,
// welche Namensregeln verletzt werden, und schreiben Sie den Code um.

// %%
class dtprcssor
{
    private List<int> lst; // List of monthly sales figures
    private bool flag;  // true if the total has been calculated

    public dtprcssor(List<int> d)
    {
        lst = d;
        flag = false;
    }

    // Calculates the total sales and sets the flag to true
    public int Calc()
    {
        int r = 0;
        foreach (var i in lst) { r += i; }
        flag = true;
        return r;
    }

    // Returns true if the total has been calculated, false otherwise
    public bool getflag() { return flag; }
}

// %%

// %%
int computeSecondsInADay = 24 * 60 * 60;

// %%

// %%
string s = "admin"; // Default role for new users

// %%

// %%
bool b_FtrFlg = true; // Feature flag: If true, the feature is enabled

// %%

// %%
// Permissions for the current user
List<string> theList = new List<string> { "read", "write", "execute" };

// %%
