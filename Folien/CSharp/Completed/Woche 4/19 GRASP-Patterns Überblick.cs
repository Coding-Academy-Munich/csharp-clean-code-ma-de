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
//
// 1. Creator
// 2. Information Expert
// 3. Low Coupling
// 4. High Cohesion
// 5. Controller
//
// **Fortgeschrittene Patterns:**
//
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
// ### Szenario: Online-Shop
//
// - Produkte mit Name und Preis
// - Einkaufswagen enthält Produkte mit Stückzahl
// - Frage: Wer erzeugt die Einträge im Einkaufswagen?

// %%
record Product(string Name, decimal Price);

class CartItem
{
    public Product Product { get; }
    public int Quantity { get; }

    public CartItem(Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }

    public decimal Subtotal => Product.Price * Quantity;
}

// %% [markdown]
//
// `ShoppingCart` enthält und aggregiert `CartItem`-Objekte
//
// Deshalb ist `ShoppingCart` der Creator von `CartItem`:

// %%
class ShoppingCart
{
    private readonly List<CartItem> items = new();

    public void Add(Product product, int quantity)
    {
        items.Add(new CartItem(product, quantity));
    }

    public IReadOnlyList<CartItem> Items => items;
}

// %%
var cart = new ShoppingCart();
cart.Add(new Product("Laptop", 999.99m), 1);
cart.Add(new Product("Mouse", 29.99m), 2);

foreach (var item in cart.Items)
    Console.WriteLine($"  {item.Product.Name}: {item.Subtotal:C}");

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
// - Suche/Filterung → Klasse mit der Sammlung

// %% [markdown]
//
// ### Neue Anforderung
//
// - Wir brauchen den Gesamtpreis des Einkaufswagens
// - `CartItem` kennt `Product` und `Quantity`
// - `ShoppingCart` kennt alle `CartItem`s
// - Wo gehört `CalculateTotal()` hin?

// %% [markdown]
//
// Versuch: Gesamtpreis extern berechnen (Feature Envy):

// %%
decimal total = 0m;
foreach (var item in cart.Items)
    total += item.Product.Price * item.Quantity;
Console.WriteLine($"Total (extern): {total:C}");

// %% [markdown]
//
// Information Expert: `CartItem` hat Preis und Stückzahl → berechnet
// sein Subtotal. `ShoppingCart` hat alle Items → berechnet das
// Gesamttotal:

// %%
class ShoppingCartWithTotal
{
    private readonly List<CartItem> items = new();

    public void Add(Product product, int quantity)
        => items.Add(new CartItem(product, quantity));

    public IReadOnlyList<CartItem> Items => items;

    public decimal CalculateTotal()
        => items.Sum(i => i.Subtotal);

    public CartItem FindItem(string productName)
        => items.FirstOrDefault(i => i.Product.Name == productName);
}

// %%
var expertCart = new ShoppingCartWithTotal();
expertCart.Add(new Product("Laptop", 999.99m), 1);
expertCart.Add(new Product("Mouse", 29.99m), 2);
Console.WriteLine($"Total: {expertCart.CalculateTotal():C}");
Console.WriteLine($"Found: {expertCart.FindItem("Mouse")?.Product.Name}");

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

// %% [markdown]
//
// ### Szenario: Bestellbestätigung
//
// - Nach dem Kauf soll der Kunde eine Bestätigung erhalten
// - Aktuell: immer per E-Mail

// %%
class EmailSender
{
    public void Send(string to, string message)
        => Console.WriteLine($"Email to {to}: {message}");
}

// %% [markdown]
//
// Hohe Kopplung: `CheckoutService` erzeugt `EmailSender` direkt:

// %%
class CheckoutService
{
    private readonly EmailSender email = new();

    public void Checkout(ShoppingCartWithTotal cart, string customerEmail)
    {
        decimal total = cart.CalculateTotal();
        email.Send(customerEmail, $"Order confirmed. Total: {total:C}");
    }
}

// %%
var checkoutCart = new ShoppingCartWithTotal();
checkoutCart.Add(new Product("Laptop", 999.99m), 1);
var checkout = new CheckoutService();
checkout.Checkout(checkoutCart, "alice@example.com");

// %% [markdown]
//
// ### Problem
//
// - `CheckoutService` erzeugt `EmailSender` direkt (`new`)
// - Kann nicht auf SMS oder Push-Benachrichtigung umgestellt werden
// - Kann in Tests nicht ersetzt werden
// - Neue Anforderung: SMS-Benachrichtigung unterstützen

// %% [markdown]
//
// Geringe Kopplung: Abhängigkeit von einer Abstraktion statt vom
// konkreten Typ:

// %%
interface IShopNotifier
{
    void Notify(string to, string message);
}

class ShopEmailNotifier : IShopNotifier
{
    public void Notify(string to, string message)
        => Console.WriteLine($"Email to {to}: {message}");
}

class ShopSmsNotifier : IShopNotifier
{
    public void Notify(string to, string message)
        => Console.WriteLine($"SMS to {to}: {message}");
}

// %%
class CheckoutServiceV2
{
    private readonly IShopNotifier notifier;

    public CheckoutServiceV2(IShopNotifier notifier)
    {
        this.notifier = notifier;
    }

    public void Checkout(ShoppingCartWithTotal cart, string customerContact)
    {
        decimal total = cart.CalculateTotal();
        notifier.Notify(customerContact, $"Order confirmed. Total: {total:C}");
    }
}

// %%
var lcCart = new ShoppingCartWithTotal();
lcCart.Add(new Product("Keyboard", 79.99m), 1);

var emailCheckout = new CheckoutServiceV2(new ShopEmailNotifier());
emailCheckout.Checkout(lcCart, "alice@example.com");

var smsCheckout = new CheckoutServiceV2(new ShopSmsNotifier());
smsCheckout.Checkout(lcCart, "+49123456");

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
// Geringe Kohäsion: `OrderProcessor` mischt Preisberechnung,
// Formatierung und Benachrichtigung:

// %%
class OrderProcessor
{
    private readonly List<CartItem> items = new();
    private readonly List<string> log = new();
    private decimal taxRate = 0.19m;

    public void AddItem(Product product, int quantity)
        => items.Add(new CartItem(product, quantity));

    public IReadOnlyList<CartItem> Items => items;

    public decimal CalculateSubtotal()
        => items.Sum(i => i.Subtotal);

    public decimal CalculateTax()
        => CalculateSubtotal() * taxRate;

    public decimal CalculateTotal()
        => CalculateSubtotal() + CalculateTax();

    public string FormatReceipt()
    {
        var lines = items.Select(i => $"{i.Product.Name}: {i.Subtotal:C}");
        return string.Join("\n", lines) + $"\nTax: {CalculateTax():C}"
            + $"\nTotal: {CalculateTotal():C}";
    }

    public void SendReceipt(string email)
    {
        log.Add($"Receipt sent to {email}");
        Console.WriteLine($"Sending receipt to {email}");
    }

    public IReadOnlyList<string> GetLog() => log;
}

// %%
var proc = new OrderProcessor();
proc.AddItem(new Product("Laptop", 999.99m), 1);
proc.AddItem(new Product("Mouse", 29.99m), 2);
Console.WriteLine(proc.FormatReceipt());
proc.SendReceipt("alice@example.com");

// %% [markdown]
//
// ### Probleme
//
// - Drei Änderungsgründe: Preislogik, Formatierung, Benachrichtigung
// - `taxRate`, `items`, `log` werden von verschiedenen Methoden verwendet
// - Name "OrderProcessor" ist zu vage → Hinweis auf niedrige Kohäsion

// %% [markdown]
//
// Hohe Kohäsion: jede Klasse hat einen klaren Zweck und den
// dazugehörigen Zustand:

// %%
class PriceCalculator
{
    private readonly decimal taxRate;

    public PriceCalculator(decimal taxRate = 0.19m) { this.taxRate = taxRate; }

    public decimal Subtotal(IReadOnlyList<CartItem> items)
        => items.Sum(i => i.Subtotal);

    public decimal Tax(IReadOnlyList<CartItem> items)
        => Subtotal(items) * taxRate;

    public decimal Total(IReadOnlyList<CartItem> items)
        => Subtotal(items) + Tax(items);
}

// %%
class ReceiptFormatter
{
    public string Format(IReadOnlyList<CartItem> items, decimal tax, decimal total)
    {
        var lines = items.Select(i => $"{i.Product.Name}: {i.Subtotal:C}");
        return string.Join("\n", lines) + $"\nTax: {tax:C}\nTotal: {total:C}";
    }
}

// %%
var pricing = new PriceCalculator(0.19m);
var formatter = new ReceiptFormatter();

var hcCart = new ShoppingCartWithTotal();
hcCart.Add(new Product("Laptop", 999.99m), 1);
hcCart.Add(new Product("Mouse", 29.99m), 2);

decimal tax = pricing.Tax(hcCart.Items);
decimal hcTotal = pricing.Total(hcCart.Items);
Console.WriteLine(formatter.Format(hcCart.Items, tax, hcTotal));

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

// %% [markdown]
//
// Der Controller koordiniert den gesamten Bestellvorgang,
// erledigt aber nicht die eigentliche Arbeit:

// %%
class ShopController
{
    private readonly PriceCalculator pricing;
    private readonly ReceiptFormatter receiptFormatter;
    private readonly IShopNotifier notifier;

    public ShopController(PriceCalculator pricing,
        ReceiptFormatter receiptFormatter, IShopNotifier notifier)
    {
        this.pricing = pricing;
        this.receiptFormatter = receiptFormatter;
        this.notifier = notifier;
    }

    public void ProcessOrder(ShoppingCartWithTotal cart, string customerContact)
    {
        decimal tax = pricing.Tax(cart.Items);
        decimal orderTotal = pricing.Total(cart.Items);
        string receipt = receiptFormatter.Format(cart.Items, tax, orderTotal);
        notifier.Notify(customerContact, receipt);
    }
}

// %%
var ctrlCart = new ShoppingCartWithTotal();
ctrlCart.Add(new Product("Laptop", 999.99m), 1);
ctrlCart.Add(new Product("Mouse", 29.99m), 2);

var controller = new ShopController(
    new PriceCalculator(),
    new ReceiptFormatter(),
    new ShopEmailNotifier());

controller.ProcessOrder(ctrlCart, "alice@example.com");

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
record Book(string Title, string Author, string Isbn);

record Member(string Name, string MemberNumber);

class Loan
{
    public Book Book { get; }
    public Member Member { get; }
    public DateTime LoanDate { get; }
    public DateTime DueDate { get; }
    public DateTime? ReturnDate { get; private set; }

    public Loan(Book book, Member member)
    {
        Book = book;
        Member = member;
        LoanDate = DateTime.Now;
        DueDate = LoanDate.AddDays(14);
    }

    public decimal CalculateLateFee()
    {
        if (ReturnDate == null || ReturnDate <= DueDate) return 0;
        int daysLate = (ReturnDate.Value - DueDate).Days;
        return daysLate * 0.50m;
    }

    public void Return() => ReturnDate = DateTime.Now;
}

// %%
class Library
{
    private List<Loan> activeLoans = new();

    public Loan LendBook(Book book, Member member)
    {
        var loan = new Loan(book, member);
        activeLoans.Add(loan);
        return loan;
    }

    public List<Loan> GetLoansForMember(string memberNumber)
        => activeLoans.Where(l => l.Member.MemberNumber == memberNumber).ToList();
}

class LibraryController
{
    private readonly Library library;

    public LibraryController(Library library) { this.library = library; }

    public Loan CheckOutBook(Book book, Member member)
    {
        return library.LendBook(book, member);
    }
}

// %%
var library = new Library();
var controller = new LibraryController(library);

var book = new Book("Clean Code", "Robert C. Martin", "978-0132350884");
var member = new Member("Alice", "M001");

var loan = controller.CheckOutBook(book, member);
Console.WriteLine($"Lent '{loan.Book.Title}' to {loan.Member.Name}");
Console.WriteLine($"Due: {loan.DueDate:d}");
