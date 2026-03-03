// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Isolation von Unit-Tests: London School</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// ## Rückblick: Zwei Schulen des Unit-Testings
//
// - **Detroit School**: Isolation = Tests unabhängig voneinander
//   - Unit = Verhalten (unit of behavior)
//   - Verwende echte Abhängigkeiten, wenn möglich
// - **London School**: Isolation = getestete Unit von Abhängigkeiten isoliert
//   - Unit = Klasse (unit of code)
//   - Ersetze alle veränderlichen Abhängigkeiten durch Test-Doubles

// %%
using System;
using System.Collections.Generic;

// %%
#r "nuget: NUnit, *"
#r "nuget: Moq, *"
#load "NUnitTestRunner.cs"
using NUnit.Framework;
using Moq;
using static NUnitTestRunner;

// %% [markdown]
//
// ## Beispiel: Verkauf von Veranstaltungs-Tickets
//
// - Klasse `Show` repräsentiert eine Veranstaltung
// - Klasse `TicketOffice` verwaltet den Verkauf

// %%
public class Show
{
    public Show(string name, int capacity)
    {
        Name = name;
        Capacity = capacity;
    }

    public string Name { get; }
    public int Capacity { get; private set; }

    public void Purchase(int numTickets)
    {
        if (numTickets > Capacity)
        {
            throw new InvalidOperationException("Not enough capacity");
        }
        Capacity -= numTickets;
    }
}

// %%
public class TicketOffice
{
    public void AddShow(Show show)
    {
        shows[show.Name] = show;
    }

    public Show GetShow(string showName)
    {
        return shows[showName];
    }

    public bool PurchaseTickets(string showName, int numTickets)
    {
        if (shows.ContainsKey(showName))
        {
            try
            {
                shows[showName].Purchase(numTickets);
                return true;
            }
            catch (InvalidOperationException e)
            {
                Console.Error.WriteLine($"Cannot purchase {numTickets} tickets for {showName}");
                Console.Error.WriteLine(e.Message);
                return false;
            }
        }
        return false;
    }

    private Dictionary<string, Show> shows = new Dictionary<string, Show>();
}

// %% [markdown]
//
// ### Test im Detroit-School-Stil

// %%
public class TestPurchaseTickets
{
    [Test]
    public void PurchaseTickets_ReturnsTrueAndReducesCapacity()
    {
        TicketOffice ticketOffice = new TicketOffice();
        Show show = new Show("C# Conference", 100);
        ticketOffice.AddShow(show);

        bool result = ticketOffice.PurchaseTickets("C# Conference", 10);

        Assert.That(result, Is.True);
        Assert.That(ticketOffice.GetShow("C# Conference").Capacity, Is.EqualTo(90));
    }
}

// %%
RunTests<TestPurchaseTickets>();

// %%
public interface IShow
{
    string Name { get; }
    int Capacity { get; }
    void Purchase(int numTickets);
}

// %%
public class ConcreteShow : IShow
{
    public ConcreteShow(string name, int capacity)
    {
        Name = name;
        Capacity = capacity;
    }

    public string Name { get; }
    public int Capacity { get; private set; }

    public void Purchase(int numTickets)
    {
        if (numTickets > Capacity)
        {
            throw new InvalidOperationException("Not enough capacity");
        }
        Capacity -= numTickets;
    }
}

// %%
public class TicketOffice
{
    public void AddShow(IShow show)
    {
        shows[show.Name] = show;
    }

    public IShow GetShow(string showName)
    {
        return shows[showName];
    }

    public bool PurchaseTickets(string showName, int numTickets)
    {
        if (shows.ContainsKey(showName))
        {
            try
            {
                shows[showName].Purchase(numTickets);
                return true;
            }
            catch (InvalidOperationException e)
            {
                Console.Error.WriteLine($"Cannot purchase {numTickets} tickets for {showName}");
                Console.Error.WriteLine(e.Message);
                return false;
            }
        }
        return false;
    }

    private Dictionary<string, IShow> shows = new Dictionary<string, IShow>();
}

// %% [markdown]
//
// ### Isolation von `TicketOffice` für Tests
//
// - Entkopplung von allen Abhängigkeiten
// - `Mock<IShow>` mit Moq

// %%
public class TestPurchaseTicketsLondonSchool
{
    [Test]
    public void PurchaseTickets_CallsPurchaseWithNumberOfTickets()
    {
        var ticketOffice = new TicketOffice();
        var showMock = new Mock<IShow>();
        showMock.Setup(s => s.Name).Returns("C# Conference");
        showMock.Setup(s => s.Capacity).Returns(90);
        ticketOffice.AddShow(showMock.Object);
        const int numTickets = 10;

        bool result = ticketOffice.PurchaseTickets("C# Conference", numTickets);

        Assert.That(result, Is.True);
        Assert.That(ticketOffice.GetShow("C# Conference").Capacity, Is.EqualTo(90));
        showMock.Verify(s => s.Purchase(numTickets), Times.Once);
    }
}

// %%
RunTests<TestPurchaseTicketsLondonSchool>();

// %% [markdown]
//
// ## Vorteile der Isolation der getesteten Unit
//
// - Einfache Struktur der Tests
//   - Jeder Test gehört zu genau einer Unit
// - Genaue Identifikation von Fehlern
// - Aufbrechen von Abhängigkeiten/des Objektgraphen

// %% [markdown]
//
// ## Nachteile der Isolation der getesteten Unit
//
// - Potenziell höherer Aufwand (z.B. Mocks)
// - Fehler in der Interaktion zwischen Units werden nicht gefunden
// - Verleiten zum Schreiben von "Interaktionstests"
// - **Risiko von Kopplung an Implementierungsdetails**

// %% [markdown]
//
// ## Empfehlung
//
// - Verwenden Sie isolierte Unit-Tests (Detroit School)
// - Isolieren Sie Abhängigkeiten, die "eine Rakete starten"
//   - nicht-deterministisch (z.B. Zufallszahlen, aktuelle Zeit, aktuelles Datum)
//   - langsam
//   - externe Systeme (z.B. Datenbank)
// - Isolieren Sie Abhängigkeiten, die ein komplexes Setup benötigen

// %% [markdown]
//
// ## Workshop: Virtuelle Universität
//
// - Im `code`-Ordner finden Sie eine Implementierung eins sehr einfachen
//   Verwaltungssystems für eine Universität:
// - Es gibt Kurse, Professoren, die die Kurse halten, Studenten, die Aufgaben
//   bearbeiten und abgeben müssen.
// - Der Code ist in `Code/StarterKits/virtual-university-sk` zu finden.
// - Die `Main.cs`-Datei illustriert, wie die Klassen zusammenarbeiten und
//   verwendet werden können.

// %% [markdown]
//
// - Identifizieren Sie, welche Klassen und Methoden zu den "wertvollsten"
//   Unit-Tests führen.
// - Implementieren Sie diese Unit-Tests mit NUnit.
//   - Idealerweise implementieren sie Tests für alle Klassen, die sinnvolle
//     Tests haben.
//   - Falls Sie dafür nicht genug Zeit haben, können Sie auch nur Tests für
//     einen Teil des Codes schreiben.
//   - Die Klasse `Student` ist ein ganz guter Startpunkt, weil sie eine sehr
//     begrenzte Funktionalität hat, die Sie mit relativ wenigen Tests abdecken
//     können.
// - Sind Ihre Tests isoliert?
//   - Nach der Detroit- oder London-School?

// %% [markdown]
//
// - Falls Sie Ihre Tests nach der Detroit-School isoliert haben:
//   - Überlegen Sie, wie Sie das System überarbeiten müssten, um die Klassen in
//     Tests vollständig zu isolieren, also im London School Stil zu testen.
