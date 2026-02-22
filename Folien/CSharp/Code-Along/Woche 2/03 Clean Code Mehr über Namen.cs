// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Clean Code: Mehr über Namen</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

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

// %% [markdown]
//
// ## Disinformation vermeiden
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

// %%
// Inconsistent:
string myPath = Path.Combine("my-data");
string yourDir = Path.Combine("your-data");
string fileLoc = Path.Combine("file-loc");

// %%
// Consistent:
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

// %%
int d = 86400;
string s = "admin";
bool b = true;
List<string> theList = new List<string> { "read", "write", "execute" };

// %%
