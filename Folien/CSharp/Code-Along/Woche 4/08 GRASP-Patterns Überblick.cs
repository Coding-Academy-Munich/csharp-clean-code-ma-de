// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>GRASP-Patterns: Überblick</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
//
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>


// %% [markdown]
//
// ## Was ist GRASP?
//
// **G**eneral **R**esponsibility **A**ssignment **S**oftware **P**atterns
//
// - Leitlinien zur Zuweisung von Verantwortlichkeiten zu Klassen
// - Helfen bei der Frage: "Welche Klasse sollte das tun?"
// - 9 Patterns von Craig Larman
// - Fundamentale Prinzipien des objektorientierten Entwurfs

// %% [markdown]
//
// ## Die 9 GRASP-Patterns
//
// **Grundlegende Patterns:**
// 1. Creator
// 2. Information Expert
// 3. Low Coupling
// 4. High Cohesion
// 5. Controller
//
// **Fortgeschrittene Patterns:**
// 6. Polymorphism
// 7. Pure Fabrication
// 8. Indirection
// 9. Protected Variations

// %%
using System;
using System.Collections.Generic;
using System.Linq;

// %% [markdown]
//
// ## Creator
//
// *Wer sollte ein Objekt B erzeugen?*
//
// Klasse A sollte B erzeugen, wenn A:
// - B enthält oder zusammenfasst
// - B eng verwendet
// - Die Daten besitzt, um B zu initialisieren

// %% [markdown]
//
// `Order` ist der Creator von `OrderItem`, weil `Order` die Items
// enthält und aggregiert:

// %%
class OrderItem
{
    public string Product { get; }
    public decimal Price { get; }
    public int Quantity { get; }

    public OrderItem(string product, decimal price, int quantity)
    {
        Product = product;
        Price = price;
        Quantity = quantity;
    }
}

class Order
{
    private List<OrderItem> items = new();

    public void AddItem(string product, decimal price, int quantity)
    {
        items.Add(new OrderItem(product, price, quantity));
    }

    public List<OrderItem> Items => items.ToList();
}

// %%
var order = new Order();
order.AddItem("Laptop", 999.99m, 1);
order.AddItem("Mouse", 29.99m, 2);
Console.WriteLine($"Items: {order.Items.Count}");

// %% [markdown]
//
// ## Information Expert
//
// *Wer sollte eine Verantwortlichkeit übernehmen?*
//
// Die Klasse, die die notwendigen Informationen hat!
//
// - Berechnung von Werten → Klasse mit den Daten
// - Validierung → Klasse mit dem Wissen
// - Formatierung → Klasse mit den Attributen

// %% [markdown]
//
// `Order` ist der Information Expert für die Gesamtberechnung (es hat die
// Items und deren Preise) und für die Artikelsuche:

// %%
class OrderWithExpert
{
    private List<OrderItem> items = new();

    public void AddItem(string product, decimal price, int quantity)
        => items.Add(new OrderItem(product, price, quantity));

    public decimal CalculateTotal()
        => items.Sum(i => i.Price * i.Quantity);

    public OrderItem FindItem(string product)
        => items.FirstOrDefault(i => i.Product == product);
}

// %%
var expertOrder = new OrderWithExpert();
expertOrder.AddItem("Laptop", 999.99m, 1);
expertOrder.AddItem("Mouse", 29.99m, 2);
Console.WriteLine($"Total: {expertOrder.CalculateTotal():C}");

// %% [markdown]
//
// ## Low Coupling (Geringe Kopplung)
//
// - Minimiere Abhängigkeiten zwischen Klassen
// - Weniger Kopplung → einfacher zu ändern und zu testen
// - Arten der Kopplung:
//   - Direkte Instanziierung
//   - Typ-Referenzen
//   - Methodenaufrufe
//   - Vererbung (stärkste Kopplung!)

// %%
interface INotificationService { void Send(string to, string message); }
class EmailService : INotificationService { public void Send(string to, string message) { } }

// %% [markdown]
//
// Hohe Kopplung: `OrderService` hängt direkt von `EmailService` ab:

// %%
class HighCouplingExample
{
    private EmailService email = new EmailService();

    public void PlaceOrder(Order order)
    {
        // ... process order ...
        email.Send("customer@example.com", "Order placed");
    }
}

// %% [markdown]
//
// Geringe Kopplung: hängt von einer Abstraktion ab:

// %%
class LowCouplingExample
{
    private readonly INotificationService notification;

    public LowCouplingExample(INotificationService notification)
    {
        this.notification = notification;
    }

    public void PlaceOrder(Order order)
    {
        // ... process order ...
        notification.Send("customer@example.com", "Order placed");
    }
}

// %% [markdown]
//
// ## High Cohesion (Hohe Kohäsion)
//
// - Alle Elemente einer Klasse gehören logisch zusammen
// - Eine Klasse hat einen klaren, fokussierten Zweck
// - Gegenteil: "God Class" die alles macht
// - Eng verwandt mit SRP (Single Responsibility Principle)

// %% [markdown]
//
// Geringe Kohäsion: die Klasse macht zu viele unzusammenhängende Dinge:

// %%
class CustomerGodClass
{
    public void SaveCustomer() { }
    public void SendEmail() { }
    public void GenerateInvoice() { }
    public void CalculateTax() { }
    public void RenderHtml() { }
    public void ConnectToDatabase() { }
}

// %% [markdown]
//
// Hohe Kohäsion: jede Klasse hat einen fokussierten Zweck:

// %%
class CustomerRepository { public void Save() { } }
class CustomerNotification { public void SendEmail() { } }
class InvoiceGenerator { public void Generate() { } }

// %% [markdown]
//
// ## Controller
//
// *Wer soll eingehende System-Ereignisse behandeln?*
//
// - Ein Controller empfängt und koordiniert System-Operationen
// - Enthält keine Geschäftslogik
// - Delegiert an die richtigen Objekte
// - Typisch: Use-Case-Controller oder Fassaden-Controller

// %%
class OrderCalculator { public decimal CalculateTotal(List<(string, decimal, int)> items) => 0m; }
class OrderRepository { public void Save(int id, decimal total) { } }
class OrderNotification { public void SendConfirmation(string email, int id) { } }

// %% [markdown]
//
// Der Controller koordiniert, erledigt aber nicht die eigentliche Arbeit:

// %%
class OrderController
{
    private readonly OrderCalculator calculator;
    private readonly OrderRepository repository;
    private readonly OrderNotification notification;

    public OrderController(OrderCalculator calculator,
        OrderRepository repository, OrderNotification notification)
    {
        this.calculator = calculator;
        this.repository = repository;
        this.notification = notification;
    }

    public void PlaceOrder(int orderId, List<(string, decimal, int)> items, string email)
    {
        decimal total = calculator.CalculateTotal(items);
        repository.Save(orderId, total);
        notification.SendConfirmation(email, orderId);
    }
}

// %% [markdown]
//
// ## Fortgeschrittene Patterns (Kurzübersicht)
//
// - **Polymorphism**: Verwende Polymorphismus statt Typ-Abfragen
// - **Pure Fabrication**: Erstelle Hilfsklassen, die keinem Domänen-Konzept
//   entsprechen (z.B. Repository, Service)
// - **Indirection**: Füge eine Zwischenschicht ein, um Kopplung zu reduzieren
// - **Protected Variations**: Schütze vor Änderungen durch stabile Interfaces

// %% [markdown]
//
// ## Zusammenfassung
//
// | Pattern | Frage | Antwort |
// |---|---|---|
// | Creator | Wer erzeugt B? | Wer B enthält/aggregiert |
// | Expert | Wer berechnet X? | Wer die Daten hat |
// | Low Coupling | Wie wenig Abhängigkeiten? | Über Abstraktionen |
// | High Cohesion | Wie fokussiert? | Eine Verantwortlichkeit |
// | Controller | Wer koordiniert? | Dedizierter Controller |

// %% [markdown]
//
// ## Workshop: Verantwortlichkeiten zuweisen
//
// Sie entwerfen ein einfaches Bibliothekssystem mit folgenden Konzepten:
// - Bücher (Titel, Autor, ISBN)
// - Mitglieder (Name, Mitgliedsnummer)
// - Ausleihen (Buch, Mitglied, Datum)
//
// Beantworten Sie mit GRASP-Prinzipien:
// 1. Welche Klasse sollte `Loan`-Objekte erzeugen? (Creator)
// 2. Welche Klasse berechnet die Leihgebühr? (Expert)
// 3. Wie halten Sie die Kopplung gering? (Low Coupling)
// 4. Welche Klasse koordiniert den Ausleih-Vorgang? (Controller)
//
// Implementieren Sie Ihre Lösung.

// %%

// %%

// %%
var library = new Library();
var controller = new LibraryController(library);

var book = new Book("Clean Code", "Robert C. Martin", "978-0132350884");
var member = new Member("Alice", "M001");

var loan = controller.CheckOutBook(book, member);
Console.WriteLine($"Lent '{loan.Book.Title}' to {loan.Member.Name}");
Console.WriteLine($"Due: {loan.DueDate:d}");
