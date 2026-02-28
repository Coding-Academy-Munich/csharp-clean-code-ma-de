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
        _baseCost = baseCost;
        _qualityRating = qualityRating;
    }

    public int UnitCost()
    {
        return _baseCost + _qualityRating * 2;
    }

    private int _baseCost;
    private int _qualityRating;
}

// %%
public class Product
{
    public Product(Supplier supplier, int margin)
    {
        _supplier = supplier;
        this.margin = margin;
    }

    public int Price()
    {
        return _supplier.UnitCost() + margin;
    }

    public void ReleaseSupplierContract() { _supplier = null; }

    private Supplier _supplier;
    public int margin;
}

// %%
public class OrderLine
{
    public OrderLine(Product product, int quantity) { _product = product; _quantity = quantity; }
    public int Total()
    {
        int baseTotal = _product.Price() * _quantity;
        if (_quantity < 10) { return baseTotal; }
        else if (_quantity < 50) { return baseTotal * 90 / 100; }
        else if (_quantity < 100) { return baseTotal * 80 / 100; }
        else { return baseTotal * 70 / 100; }
    }
    private Product _product;
    private int _quantity;
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

        product.ReleaseSupplierContract();
    }

    [Test]
    public void TestTotal2()
    {
        Supplier supplier = new Supplier(3, 1);
        Product product = new Product(supplier, 6);
        OrderLine unit = new OrderLine(product, 20);

        Assert.That(unit.Total(), Is.EqualTo(198));

        product.ReleaseSupplierContract();
    }

    [Test]
    public void TestTotal3()
    {
        Supplier supplier = new Supplier(3, 1);
        Product product = new Product(supplier, 5);
        OrderLine unit = new OrderLine(product, 75);

        Assert.That(unit.Total(), Is.EqualTo(600));

        product.ReleaseSupplierContract();
    }

    [Test]
    public void TestTotal4()
    {
        Supplier supplier = new Supplier(3, 1);
        Product product = new Product(supplier, 5);
        OrderLine unit = new OrderLine(product, 150);

        Assert.That(unit.Total(), Is.EqualTo(1050));

        product.ReleaseSupplierContract();
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
        _product = new Product(supplier, 5);
    }

    private Product _product;

    [Test]
    public void TestTotal1()
    {
        SetupDependencies();
        OrderLine unit = new OrderLine(_product, 5);

        Assert.That(unit.Total(), Is.EqualTo(50));

        _product.ReleaseSupplierContract();
    }

    [Test]
    public void TestTotal2()
    {
        SetupDependencies();
        OrderLine unit = new OrderLine(_product, 20);

        Assert.That(unit.Total(), Is.EqualTo(180));

        _product.ReleaseSupplierContract();
    }

    [Test]
    public void TestTotal3()
    {
        SetupDependencies();
        OrderLine unit = new OrderLine(_product, 75);

        Assert.That(unit.Total(), Is.EqualTo(600));

        _product.ReleaseSupplierContract();
    }

    [Test]
    public void TestTotal4()
    {
        SetupDependencies();
        OrderLine unit = new OrderLine(_product, 150);

        Assert.That(unit.Total(), Is.EqualTo(1050));

        _product.ReleaseSupplierContract();
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
        _product.ReleaseSupplierContract();
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
        OrderLine unit = new OrderLine(_product, 75);
        Assert.That(unit.Total(), Is.EqualTo(600));
    }
}

// %%
RunTests<OrderLineTestWithSetUpAndTearDown>();

// %% [markdown]
//
// ## `[OneTimeSetUp]` und `[OneTimeTearDown]`
//
// - Was, wenn Teile des Setups teuer sind (z.B. Datenbankverbindung)?
// - `[OneTimeSetUp]`: Wird **einmal pro Testklasse** ausgefuehrt
// - `[OneTimeTearDown]`: Wird **einmal nach allen Tests** ausgefuehrt
// - Zustand wird zwischen allen Tests der Klasse geteilt
// - Kann mit `[SetUp]`/`[TearDown]` kombiniert werden

// %% [markdown]
//
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
    private Supplier _supplier;
    private Product _product;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Console.WriteLine("  [OneTimeSetUp] Creating shared supplier");
        _supplier = new Supplier(3, 1);
    }

    [SetUp]
    public void SetUp()
    {
        Console.WriteLine("  [SetUp] Creating per-test product");
        _product = new Product(_supplier, 5);
    }

    [TearDown]
    public void TearDown()
    {
        Console.WriteLine("  [TearDown] Releasing supplier contract");
        _product.ReleaseSupplierContract();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Console.WriteLine("  [OneTimeTearDown] Cleaning up shared supplier");
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
        OrderLine unit = new OrderLine(_product, 75);
        Assert.That(unit.Total(), Is.EqualTo(600));
    }
}

// %%
RunTests<CombinedSetUpTest>();

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
    private List<PlaylistEntry> _entries = new List<PlaylistEntry>();
    private User _owner;
    public string Name { get; }

    public Playlist(User owner, string name)
    {
        _owner = owner;
        Name = name;
    }

    public void AddSong(Song song)
    {
        _entries.Add(new PlaylistEntry(song));
    }

    public int GetTotalDuration()
    {
        return _entries.Sum(entry => entry.Song.DurationInSeconds);
    }

    public int GetTotalPlayCount()
    {
        return _entries.Sum(entry => entry.PlayCount);
    }

    public bool CanAddMoreSongs()
    {
        if (_owner.IsPremium)
        {
            return true;
        }
        return _entries.Count < 100;
    }

    public List<PlaylistEntry> GetEntries()
    {
        return new List<PlaylistEntry>(_entries);
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
