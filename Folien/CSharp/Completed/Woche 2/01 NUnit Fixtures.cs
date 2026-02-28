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
public class Supplier
{
    public Supplier(int baseCost, int qualityRating)
    {
        this.baseCost = baseCost;
        this.qualityRating = qualityRating;
    }

    public int UnitCost()
    {
        return baseCost + qualityRating * 2;
    }

    private int baseCost;
    private int qualityRating;
}

// %%
public class Product
{
    public Product(Supplier supplier, int margin)
    {
        this.supplier = supplier;
        this.margin = margin;
    }

    public int Price()
    {
        return supplier.UnitCost() + margin;
    }

    private Supplier supplier;
    public int margin;
}

// %%
public class OrderLine
{
    public OrderLine(Product product, int quantity) { this.product = product; this.quantity = quantity; }
    public int Total()
    {
        int baseTotal = product.Price() * quantity;
        if (quantity < 10) { return baseTotal; }
        else if (quantity < 50) { return baseTotal * 90 / 100; }
        else if (quantity < 100) { return baseTotal * 80 / 100; }
        else { return baseTotal * 70 / 100; }
    }
    public void ReleaseReservation() { product = null; }

    private Product product;
    private int quantity;
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
public class OrderLineTest1
{
    [Test]
    public void TestTotal1()
    {
        Supplier supplier = new Supplier(3, 1);
        Product product = new Product(supplier, 5);
        OrderLine unit = new OrderLine(product, 5);

        Assert.That(unit.Total(), Is.EqualTo(50));

        unit.ReleaseReservation();
    }

    [Test]
    public void TestTotal2()
    {
        Supplier supplier = new Supplier(3, 1);
        Product product = new Product(supplier, 6);
        OrderLine unit = new OrderLine(product, 20);

        Assert.That(unit.Total(), Is.EqualTo(198));

        unit.ReleaseReservation();
    }

    [Test]
    public void TestTotal3()
    {
        Supplier supplier = new Supplier(3, 1);
        Product product = new Product(supplier, 5);
        OrderLine unit = new OrderLine(product, 75);

        Assert.That(unit.Total(), Is.EqualTo(600));

        unit.ReleaseReservation();
    }

    [Test]
    public void TestTotal4()
    {
        Supplier supplier = new Supplier(3, 1);
        Product product = new Product(supplier, 5);
        OrderLine unit = new OrderLine(product, 150);

        Assert.That(unit.Total(), Is.EqualTo(1050));

        unit.ReleaseReservation();
    }
}

// %%
#load "NunitTestRunner.cs"

// %%
using static NunitTestRunner;

// %%
RunTests<OrderLineTest1>();

// %% [markdown]
//
// ## Manuelle Setup-Methode
//
// - Wir können eine Setup-Methode definieren und sie in jedem Test aufrufen
// - Das reduziert die Code-Duplikation
// - Aber wir muessen daran denken, sie in jedem Test aufzurufen

// %%
[TestFixture]
public class OrderLineTest2
{
    private void SetupDependencies()
    {
        Supplier supplier = new Supplier(3, 1);
        product = new Product(supplier, 5);
    }

    private Product product;

    [Test]
    public void TestTotal1()
    {
        SetupDependencies();
        OrderLine unit = new OrderLine(product, 5);

        Assert.That(unit.Total(), Is.EqualTo(50));

        unit.ReleaseReservation();
    }

    [Test]
    public void TestTotal2()
    {
        SetupDependencies();
        OrderLine unit = new OrderLine(product, 20);

        Assert.That(unit.Total(), Is.EqualTo(180));

        unit.ReleaseReservation();
    }

    [Test]
    public void TestTotal3()
    {
        SetupDependencies();
        OrderLine unit = new OrderLine(product, 75);

        Assert.That(unit.Total(), Is.EqualTo(600));

        unit.ReleaseReservation();
    }

    [Test]
    public void TestTotal4()
    {
        SetupDependencies();
        OrderLine unit = new OrderLine(product, 150);

        Assert.That(unit.Total(), Is.EqualTo(1050));

        unit.ReleaseReservation();
    }
}

// %%
RunTests<OrderLineTest2>();

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
public class OrderLineTestWithSetUp
{
    private Product _product;

    [SetUp]
    public void SetUp()
    {
        Supplier supplier = new Supplier(3, 1);
        _product = new Product(supplier, 5);
    }

    [Test]
    public void TestTotal1()
    {
        OrderLine unit = new OrderLine(_product, 5);
        Assert.That(unit.Total(), Is.EqualTo(50));
    }

    [Test]
    public void TestTotal2()
    {
        OrderLine unit = new OrderLine(_product, 20);
        Assert.That(unit.Total(), Is.EqualTo(180));
    }

    [Test]
    public void TestTotal3()
    {
        OrderLine unit = new OrderLine(_product, 75);
        Assert.That(unit.Total(), Is.EqualTo(600));
    }

    [Test]
    public void TestTotal4()
    {
        OrderLine unit = new OrderLine(_product, 150);
        Assert.That(unit.Total(), Is.EqualTo(1050));
    }
}

// %%
RunTests<OrderLineTestWithSetUp>();

// %% [markdown]
//
// ## `[SetUp]` und `[TearDown]` zusammen
//
// - `[TearDown]` eignet sich ideal fuer Cleanup-Aufgaben
// - Wird auch aufgerufen, wenn ein Test fehlschlaegt

// %%
[TestFixture]
public class OrderLineTestWithSetUpAndTearDown
{
    private OrderLine _unit;
    private Product _product;

    [SetUp]
    public void SetUp()
    {
        Console.WriteLine($"  [SetUp] Running setup");
        Supplier supplier = new Supplier(3, 1);
        _product = new Product(supplier, 5);
    }

    [TearDown]
    public void TearDown()
    {
        Console.WriteLine($"  [TearDown] Running teardown");
        if (_unit != null)
        {
            _unit.ReleaseReservation();
            _unit = null;
        }
    }

    [Test]
    public void TestTotal1()
    {
        _unit = new OrderLine(_product, 5);
        Assert.That(_unit.Total(), Is.EqualTo(50));
    }

    [Test]
    public void TestTotal2()
    {
        _unit = new OrderLine(_product, 75);
        Assert.That(_unit.Total(), Is.EqualTo(600));
    }
}

// %%
RunTests<OrderLineTestWithSetUpAndTearDown>();

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
public class OrderLineTestWithOneTimeSetUp
{
    private Product _product;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Console.WriteLine("  [OneTimeSetUp] Creating shared dependencies");
        Supplier supplier = new Supplier(3, 1);
        _product = new Product(supplier, 5);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Console.WriteLine("  [OneTimeTearDown] Cleaning up shared dependencies");
    }

    [Test]
    public void TestTotal1()
    {
        OrderLine unit = new OrderLine(_product, 5);
        Assert.That(unit.Total(), Is.EqualTo(50));
    }

    [Test]
    public void TestTotal2()
    {
        OrderLine unit = new OrderLine(_product, 20);
        Assert.That(unit.Total(), Is.EqualTo(180));
    }

    [Test]
    public void TestTotal3()
    {
        OrderLine unit = new OrderLine(_product, 75);
        Assert.That(unit.Total(), Is.EqualTo(600));
    }

    [Test]
    public void TestTotal4()
    {
        OrderLine unit = new OrderLine(_product, 150);
        Assert.That(unit.Total(), Is.EqualTo(1050));
    }
}

// %%
RunTests<OrderLineTestWithOneTimeSetUp>();

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
    private Product _product;
    private OrderLine _unit;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Console.WriteLine("  [OneTimeSetUp] Creating shared dependencies");
        Supplier supplier = new Supplier(3, 1);
        _product = new Product(supplier, 5);
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
            _unit.ReleaseReservation();
            _unit = null;
        }
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Console.WriteLine("  [OneTimeTearDown] Cleaning up shared dependencies");
    }

    [Test]
    public void TestTotal1()
    {
        _unit = new OrderLine(_product, 5);
        Assert.That(_unit.Total(), Is.EqualTo(50));
    }

    [Test]
    public void TestTotal2()
    {
        _unit = new OrderLine(_product, 75);
        Assert.That(_unit.Total(), Is.EqualTo(600));
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
[TestFixture]
public class PlaylistEntryTest
{
    private Song _song;
    private PlaylistEntry _entry;

    [SetUp]
    public void SetUp()
    {
        _song = new Song("Test Song", "Test Artist", 180);
        _entry = new PlaylistEntry(_song);
    }

    [Test]
    public void TestInitialPlayCount()
    {
        Assert.That(_entry.PlayCount, Is.EqualTo(0));
    }

    [Test]
    public void TestIncrementPlayCount()
    {
        _entry.IncrementPlayCount();
        _entry.IncrementPlayCount();
        Assert.That(_entry.PlayCount, Is.EqualTo(2));
    }
}

// %%
RunTests(typeof(PlaylistEntryTest));

// %%
[TestFixture]
public class PlaylistBasicsTest
{
    private User _user;
    private Song _song1;
    private Song _song2;
    private Playlist _playlist;

    [SetUp]
    public void SetUp()
    {
        _user = new User("Alice", "alice@example.com", false);
        _song1 = new Song("Song 1", "Artist 1", 180);
        _song2 = new Song("Song 2", "Artist 2", 240);
        _playlist = new Playlist(_user, "My Playlist");
    }

    [Test]
    public void TestPlaylistInitiallyEmpty()
    {
        Assert.That(_playlist.GetEntries(), Is.Empty);
    }

    [Test]
    public void TestAddSong()
    {
        _playlist.AddSong(_song1);
        _playlist.AddSong(_song2);
        Assert.That(_playlist.GetEntries().Count, Is.EqualTo(2));
    }

    [Test]
    public void TestTotalDurationForEmptyPlaylist()
    {
        Assert.That(_playlist.GetTotalDuration(), Is.EqualTo(0));
    }

    [Test]
    public void TestTotalDurationForNonEmptyPlaylist()
    {
        _playlist.AddSong(_song1);
        _playlist.AddSong(_song2);
        Assert.That(_playlist.GetTotalDuration(), Is.EqualTo(420));
    }
}

// %%
RunTests(typeof(PlaylistBasicsTest));

// %%
[TestFixture]
public class PlaylistLimitsTest
{
    private Song _song;
    private Playlist _premiumPlaylist;
    private Playlist _nonPremiumPlaylist;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _song = new Song("Test Song", "Test Artist", 180);
    }

    [SetUp]
    public void SetUp()
    {
        User premiumUser = new User("premium", "premium@example.com", true);
        User nonPremiumUser = new User("regular", "regular@example.com", false);
        _premiumPlaylist = new Playlist(premiumUser, "Premium Playlist");
        _nonPremiumPlaylist = new Playlist(nonPremiumUser, "Regular Playlist");
    }

    [Test]
    public void TestCanAddMoreSongsForPremiumUser()
    {
        for (int i = 0; i < 150; i++)
        {
            _premiumPlaylist.AddSong(_song);
        }
        Assert.That(_premiumPlaylist.CanAddMoreSongs(), Is.True);
    }

    [Test]
    public void TestCanAddMoreSongsForNonPremiumUser()
    {
        for (int i = 0; i < 99; i++)
        {
            _nonPremiumPlaylist.AddSong(_song);
        }
        Assert.That(_nonPremiumPlaylist.CanAddMoreSongs(), Is.True);
        _nonPremiumPlaylist.AddSong(_song);
        Assert.That(_nonPremiumPlaylist.CanAddMoreSongs(), Is.False);
    }

    [Test]
    public void TestTotalPlayCount()
    {
        _premiumPlaylist.AddSong(_song);
        _premiumPlaylist.GetEntries()[0].IncrementPlayCount();
        _premiumPlaylist.GetEntries()[0].IncrementPlayCount();
        Assert.That(_premiumPlaylist.GetTotalPlayCount(), Is.EqualTo(2));
    }
}

// %%
RunTests<PlaylistLimitsTest>();

// %%
