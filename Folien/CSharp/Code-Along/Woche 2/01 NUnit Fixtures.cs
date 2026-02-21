// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>NUnit Fixtures</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// ## Probleme mit einfachen Tests
//
// - Kompliziertes Setup (und Teardown)
//   - Wiederholung von Code
//   - Schwer zu warten und verstehen
// - Viele aehnliche Tests

// %%
public class Dependency1
{
    public Dependency1(int i, int j)
    {
        this.i = i;
        this.j = j;
    }

    public int Value()
    {
        return i + 2 * j;
    }

    private int i;
    private int j;
}

// %%
public class Dependency2
{
    public Dependency2(Dependency1 d1, int k)
    {
        this.d1 = d1;
        this.k = k + d1.Value();
    }

    public int Value()
    {
        return d1.Value() + 3 * k;
    }

    private Dependency1 d1;
    public int k;
}

// %%
public class MyClass
{
    public MyClass(Dependency2 d2, int m) { this.d2 = d2; this.m = m; }
    public int Value()
    {
        if (m < - 10) { return d2.Value() + 2 * m; }
        else if (m < 0) { return d2.Value() - 3 * m; }
        else if (m < 10) { return d2.Value() + 4 * m; }
        else { return d2.Value() - 5 * m; }
    }
    public void ReleaseResources() { d2 = null;  }

    private Dependency2 d2;
    private int m;
}

// %% [markdown]
//
// ## Tests ohne Fixtures

// %%
#r "nuget: NUnit, *"

// %%
using NUnit.Framework;

// %%
[TestFixture]
public class MyClassTest1
{
    [Test]
    public void TestValue1()
    {
        Dependency1 d1 = new Dependency1(1, 2);
        Dependency2 d2 = new Dependency2(d1, 3);
        MyClass unit = new MyClass(d2, -11);

        Assert.That(unit.Value(), Is.EqualTo(7));

        unit.ReleaseResources();
    }

    [Test]
    public void TestValue2()
    {
        Dependency1 d1 = new Dependency1(1, 2);
        Dependency2 d2 = new Dependency2(d1, 4);
        MyClass unit = new MyClass(d2, -1);

        Assert.That(unit.Value(), Is.EqualTo(35));

        unit.ReleaseResources();
    }

    [Test]
    public void TestValue3()
    {
        Dependency1 d1 = new Dependency1(1, 2);
        Dependency2 d2 = new Dependency2(d1, 3);
        MyClass unit = new MyClass(d2, 1);

        Assert.That(unit.Value(), Is.EqualTo(33));

        unit.ReleaseResources();
    }

    [Test]
    public void TestValue4()
    {
        Dependency1 d1 = new Dependency1(1, 2);
        Dependency2 d2 = new Dependency2(d1, 3);
        MyClass unit = new MyClass(d2, 11);

        Assert.That(unit.Value(), Is.EqualTo(-26));

        unit.ReleaseResources();
    }
}

// %%
#load "NunitTestRunner.cs"

// %%
using static NunitTestRunner;

// %%
RunTests<MyClassTest1>();

// %% [markdown]
//
// ## Manuelle Setup-Methode
//
// - Wir können eine Setup-Methode definieren und sie in jedem Test aufrufen
// - Das reduziert die Code-Duplikation
// - Aber wir muessen daran denken, sie in jedem Test aufzurufen

// %%
[TestFixture]
public class MyClassTest2
{
    private void SetupDependencies()
    {
        Dependency1 d1 = new Dependency1(1, 2);
        d2 = new Dependency2(d1, 3);
    }

    private Dependency2 d2;

    [Test]
    public void TestValue1()
    {
        SetupDependencies();
        MyClass unit = new MyClass(d2, -11);

        Assert.That(unit.Value(), Is.EqualTo(7));

        unit.ReleaseResources();
    }

    [Test]
    public void TestValue2()
    {
        SetupDependencies();
        MyClass unit = new MyClass(d2, -1);

        Assert.That(unit.Value(), Is.EqualTo(32));

        unit.ReleaseResources();
    }

    [Test]
    public void TestValue3()
    {
        SetupDependencies();
        MyClass unit = new MyClass(d2, 1);

        Assert.That(unit.Value(), Is.EqualTo(33));

        unit.ReleaseResources();
    }

    [Test]
    public void TestValue4()
    {
        SetupDependencies();
        MyClass unit = new MyClass(d2, 11);

        Assert.That(unit.Value(), Is.EqualTo(-26));

        unit.ReleaseResources();
    }
}

// %%
RunTests<MyClassTest2>();

// %% [markdown]
//
// ## `[SetUp]` und `[TearDown]`
//
// - NUnit bietet die Attribute `[SetUp]` und `[TearDown]`
// - `[SetUp]`: Methode wird **vor jedem Test** automatisch aufgerufen
// - `[TearDown]`: Methode wird **nach jedem Test** automatisch aufgerufen
// - NUnit erzeugt fuer jeden Test eine neue Instanz der Testklasse

// %%
[TestFixture]
public class MyClassTestWithSetUp
{
    private Dependency2 _d2;

    [SetUp]
    public void SetUp()
    {
        Dependency1 d1 = new Dependency1(1, 2);
        _d2 = new Dependency2(d1, 3);
    }

    [Test]
    public void TestValue1()
    {
        MyClass unit = new MyClass(_d2, -11);
        Assert.That(unit.Value(), Is.EqualTo(7));
    }

    [Test]
    public void TestValue2()
    {
        MyClass unit = new MyClass(_d2, -1);
        Assert.That(unit.Value(), Is.EqualTo(32));
    }

    [Test]
    public void TestValue3()
    {
        MyClass unit = new MyClass(_d2, 1);
        Assert.That(unit.Value(), Is.EqualTo(33));
    }

    [Test]
    public void TestValue4()
    {
        MyClass unit = new MyClass(_d2, 11);
        Assert.That(unit.Value(), Is.EqualTo(-26));
    }
}

// %%
RunTests<MyClassTestWithSetUp>();

// %% [markdown]
//
// ## `[SetUp]` und `[TearDown]` zusammen
//
// - `[TearDown]` eignet sich ideal fuer Cleanup-Aufgaben
// - Wird auch aufgerufen, wenn ein Test fehlschlaegt

// %%
[TestFixture]
public class MyClassTestWithSetUpAndTearDown
{
    private MyClass _unit;
    private Dependency2 _d2;

    [SetUp]
    public void SetUp()
    {
        Console.WriteLine($"  [SetUp] Running setup");
        Dependency1 d1 = new Dependency1(1, 2);
        _d2 = new Dependency2(d1, 3);
    }

    [TearDown]
    public void TearDown()
    {
        Console.WriteLine($"  [TearDown] Running teardown");
        if (_unit != null)
        {
            _unit.ReleaseResources();
            _unit = null;
        }
    }

    [Test]
    public void TestValue1()
    {
        _unit = new MyClass(_d2, -11);
        Assert.That(_unit.Value(), Is.EqualTo(7));
    }

    [Test]
    public void TestValue2()
    {
        _unit = new MyClass(_d2, 1);
        Assert.That(_unit.Value(), Is.EqualTo(33));
    }
}

// %%
RunTests<MyClassTestWithSetUpAndTearDown>();

// %% [markdown]
//
// ## `[OneTimeSetUp]` und `[OneTimeTearDown]`
//
// - Manchmal ist das Setup teuer (z.B. Datenbankverbindung)
// - `[OneTimeSetUp]`: Wird **einmal pro Testklasse** ausgefuehrt
// - `[OneTimeTearDown]`: Wird **einmal nach allen Tests** ausgefuehrt
// - Zustand wird zwischen allen Tests der Klasse geteilt

// %%
[TestFixture]
public class MyClassTestWithOneTimeSetUp
{
    private Dependency2 _d2;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Console.WriteLine("  [OneTimeSetUp] Creating shared dependencies");
        Dependency1 d1 = new Dependency1(1, 2);
        _d2 = new Dependency2(d1, 3);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Console.WriteLine("  [OneTimeTearDown] Cleaning up shared dependencies");
    }

    [Test]
    public void TestValue1()
    {
        MyClass unit = new MyClass(_d2, -11);
        Assert.That(unit.Value(), Is.EqualTo(7));
    }

    [Test]
    public void TestValue2()
    {
        MyClass unit = new MyClass(_d2, -1);
        Assert.That(unit.Value(), Is.EqualTo(32));
    }

    [Test]
    public void TestValue3()
    {
        MyClass unit = new MyClass(_d2, 1);
        Assert.That(unit.Value(), Is.EqualTo(33));
    }

    [Test]
    public void TestValue4()
    {
        MyClass unit = new MyClass(_d2, 11);
        Assert.That(unit.Value(), Is.EqualTo(-26));
    }
}

// %%
RunTests<MyClassTestWithOneTimeSetUp>();

// %% [markdown]
//
// ## Kombination von `[SetUp]` und `[OneTimeSetUp]`
//
// - Beide Arten von Setup können in derselben Testklasse verwendet werden
// - `[OneTimeSetUp]` fuer teure, einmalige Initialisierung
// - `[SetUp]` fuer leichtgewichtiges, pro-Test Setup
// - Ausfuehrungsreihenfolge:
//   1. `[OneTimeSetUp]` (einmal)
//   2. `[SetUp]` (vor jedem Test)
//   3. Test
//   4. `[TearDown]` (nach jedem Test)
//   5. `[OneTimeTearDown]` (einmal am Ende)

// %%
[TestFixture]
public class CombinedSetUpTest
{
    private Dependency2 _d2;
    private MyClass _unit;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Console.WriteLine("  [OneTimeSetUp] Creating shared dependencies");
        Dependency1 d1 = new Dependency1(1, 2);
        _d2 = new Dependency2(d1, 3);
    }

    [SetUp]
    public void SetUp()
    {
        Console.WriteLine("  [SetUp] Running per-test setup");
    }

    [TearDown]
    public void TearDown()
    {
        Console.WriteLine("  [TearDown] Running per-test teardown");
        if (_unit != null)
        {
            _unit.ReleaseResources();
            _unit = null;
        }
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Console.WriteLine("  [OneTimeTearDown] Cleaning up shared dependencies");
    }

    [Test]
    public void TestValue1()
    {
        _unit = new MyClass(_d2, -11);
        Assert.That(_unit.Value(), Is.EqualTo(7));
    }

    [Test]
    public void TestValue2()
    {
        _unit = new MyClass(_d2, 1);
        Assert.That(_unit.Value(), Is.EqualTo(33));
    }
}

// %%
RunTests<CombinedSetUpTest>();

// %% [markdown]
//
// ## Vergleich: xUnit vs. NUnit Fixtures
//
// | Konzept | xUnit | NUnit |
// |---------|-------|-------|
// | Pro-Test Setup | Konstruktor | `[SetUp]` |
// | Pro-Test Teardown | `IDisposable.Dispose()` | `[TearDown]` |
// | Einmaliges Setup | `IClassFixture<T>` | `[OneTimeSetUp]` |
// | Einmaliges Teardown | Fixture `IDisposable` | `[OneTimeTearDown]` |
// | Test-Attribut | `[Fact]` | `[Test]` |

// %% [markdown]
//
// ## Workshop: NUnit Fixtures fuer einen Musik-Streaming-Dienst
//
// In diesem Workshop werden wir Tests fuer ein einfaches Musik-Streaming-System
// mit NUnit implementieren.
//
// In diesem System haben wir die Klassen `User`, `Song`, `PlaylistEntry` und
// `Playlist`.
// - Die Klasse `User` repraesentiert einen Benutzer des Streaming-Dienstes.
// - Ein `Song` repraesentiert ein Musikstueck, das im Streaming-Dienst
//   verfuegbar ist.
// - Ein `PlaylistEntry` repraesentiert einen Eintrag in einer Playlist, also
//   ein Musikstueck und die Anzahl der Wiedergaben.
// - Eine `Playlist` repraesentiert eine Sammlung von Musikstuecken, also eine
//   Liste von `PlaylistEntry`-Objekten.

// %%
public class User
{
    public string Username { get; }
    public string Email { get; }
    public bool IsPremium { get; }

    public User(string username, string email, bool isPremium)
    {
        Username = username;
        Email = email;
        IsPremium = isPremium;
    }
}

// %%
public class Song
{
    public string Title { get; }
    public string Artist { get; }
    public int DurationInSeconds { get; }

    public Song(string title, string artist, int durationInSeconds)
    {
        Title = title;
        Artist = artist;
        DurationInSeconds = durationInSeconds;
    }
}

// %%
public class PlaylistEntry
{
    public Song Song { get; }
    public int PlayCount { get; private set; }

    public PlaylistEntry(Song song)
    {
        Song = song;
        PlayCount = 0;
    }

    public void IncrementPlayCount()
    {
        PlayCount++;
    }
}

// %%
public class Playlist
{
    private List<PlaylistEntry> entries = new List<PlaylistEntry>();
    private User owner;
    public string Name { get; }

    public Playlist(User owner, string name)
    {
        this.owner = owner;
        Name = name;
    }

    public void AddSong(Song song)
    {
        entries.Add(new PlaylistEntry(song));
    }

    public int GetTotalDuration()
    {
        return entries.Sum(entry => entry.Song.DurationInSeconds);
    }

    public int GetTotalPlayCount()
    {
        return entries.Sum(entry => entry.PlayCount);
    }

    public bool CanAddMoreSongs()
    {
        if (owner.IsPremium)
        {
            return true;
        }
        return entries.Count < 100;
    }

    public List<PlaylistEntry> GetEntries()
    {
        return new List<PlaylistEntry>(entries);
    }
}

// %% [markdown]
//
// Implementieren Sie Tests fuer dieses System mit NUnit. Verwenden Sie dabei
// `[SetUp]` und `[OneTimeSetUp]`, um die Tests zu strukturieren und
// Code-Duplikation zu vermeiden.
//
// Beachten Sie bei der Implementierung der Tests die folgenden Aspekte:
// - Grundlegende Funktionen wie das Hinzufuegen von Songs zu einer Playlist
// - Berechnung der Gesamtdauer einer Playlist
// - Unterschiedliche Benutzerrechte (Premium vs. Nicht-Premium)
// - Begrenzung der Playlist-Groesse fuer Nicht-Premium-Benutzer
// - Zaehlung der Wiedergaben von Songs

// %%

// %%

// %%
