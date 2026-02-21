// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Design Patterns: Überblick</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// ## Was sind Design Patterns?
//
// - Bewährte Lösungen für wiederkehrende Entwurfsprobleme
// - Dokumentiert von der "Gang of Four" (GoF): Gamma, Helm, Johnson, Vlissides
// - 23 klassische Patterns in drei Kategorien:
//   - **Erzeugungsmuster**: Wie Objekte erstellt werden
//   - **Strukturmuster**: Wie Klassen zusammengesetzt werden
//   - **Verhaltensmuster**: Wie Objekte interagieren

// %% [markdown]
//
// ## Wann Patterns verwenden?
//
// - Wenn ein bekanntes Problem vorliegt
// - Nicht prophylaktisch ("vielleicht brauchen wir das")
// - Die einfachste Lösung ist oft die beste
// - Patterns sind Werkzeuge, keine Ziele

// %%
using System;
using System.Collections.Generic;
using System.Linq;

// %% [markdown]
//
// ## Strategy Pattern
//
// **Problem:** Ein Algorithmus soll zur Laufzeit austauschbar sein
//
// **Lösung:** Definiere eine Familie von Algorithmen, kapsle jeden
// einzelnen und mache sie austauschbar.
//
// **Wann:** Verschiedene Varianten eines Verhaltens, die gewechselt
// werden können

// %% [markdown]
//
// ### Ohne Strategy: if/else-Kette

// %%
class SorterWithoutStrategy
{
    public List<int> Sort(List<int> data, string algorithm)
    {
        if (algorithm == "bubble")
        {
            // Bubble sort implementation
            var result = new List<int>(data);
            for (int i = 0; i < result.Count; i++)
                for (int j = 0; j < result.Count - 1; j++)
                    if (result[j] > result[j + 1])
                        (result[j], result[j + 1]) = (result[j + 1], result[j]);
            return result;
        }
        else if (algorithm == "quick")
        {
            // Quick sort implementation
            var result = new List<int>(data);
            result.Sort();
            return result;
        }
        throw new ArgumentException("Unknown algorithm");
    }
}

// %% [markdown]
//
// ### Mit Strategy Pattern

// %%
interface ISortStrategy
{
    List<int> Sort(List<int> data);
}

class BubbleSortStrategy : ISortStrategy
{
    public List<int> Sort(List<int> data)
    {
        var result = new List<int>(data);
        for (int i = 0; i < result.Count; i++)
            for (int j = 0; j < result.Count - 1; j++)
                if (result[j] > result[j + 1])
                    (result[j], result[j + 1]) = (result[j + 1], result[j]);
        return result;
    }
}

class QuickSortStrategy : ISortStrategy
{
    public List<int> Sort(List<int> data)
    {
        var result = new List<int>(data);
        result.Sort();
        return result;
    }
}

// %%
class Sorter
{
    private ISortStrategy strategy;

    public Sorter(ISortStrategy strategy)
    {
        this.strategy = strategy;
    }

    public void SetStrategy(ISortStrategy newStrategy)
    {
        strategy = newStrategy;
    }

    public List<int> Sort(List<int> data)
    {
        return strategy.Sort(data);
    }
}

// %%
var data = new List<int> { 5, 3, 8, 1, 9, 2 };

var sorter = new Sorter(new BubbleSortStrategy());
Console.WriteLine("Bubble: " + string.Join(", ", sorter.Sort(data)));

sorter.SetStrategy(new QuickSortStrategy());
Console.WriteLine("Quick: " + string.Join(", ", sorter.Sort(data)));

// %% [markdown]
//
// ## Observer Pattern
//
// **Problem:** Ein Objekt soll andere Objekte über Zustandsänderungen
// informieren, ohne sie zu kennen
//
// **Lösung:** Definiere eine 1-zu-N-Abhängigkeit. Wenn sich ein Objekt
// ändert, werden alle abhängigen Objekte benachrichtigt.
//
// **Wann:** Event-Systeme, UI-Updates, Logging

// %%
interface IObserver
{
    void Update(string eventType, object data);
}

class EventManager
{
    private Dictionary<string, List<IObserver>> listeners = new();

    public void Subscribe(string eventType, IObserver observer)
    {
        if (!listeners.ContainsKey(eventType))
            listeners[eventType] = new List<IObserver>();
        listeners[eventType].Add(observer);
    }

    public void Unsubscribe(string eventType, IObserver observer)
    {
        if (listeners.ContainsKey(eventType))
            listeners[eventType].Remove(observer);
    }

    public void Notify(string eventType, object data)
    {
        if (listeners.ContainsKey(eventType))
            foreach (var observer in listeners[eventType])
                observer.Update(eventType, data);
    }
}

// %%
class TemperatureSensor
{
    private readonly EventManager events = new();
    private double temperature;

    public EventManager Events => events;

    public double Temperature
    {
        get => temperature;
        set
        {
            temperature = value;
            events.Notify("temperatureChanged", temperature);
            if (temperature > 100)
                events.Notify("overheating", temperature);
        }
    }
}

// %%
class DisplayObserver : IObserver
{
    public void Update(string eventType, object data)
        => Console.WriteLine($"Display: {eventType} = {data}");
}

class AlarmObserver : IObserver
{
    public void Update(string eventType, object data)
        => Console.WriteLine($"ALARM: {eventType} = {data}!");
}

// %%
var sensor = new TemperatureSensor();
var display = new DisplayObserver();
var alarm = new AlarmObserver();

sensor.Events.Subscribe("temperatureChanged", display);
sensor.Events.Subscribe("overheating", alarm);

sensor.Temperature = 25;
sensor.Temperature = 110;

// %% [markdown]
//
// ## C# Events: Eingebauter Observer
//
// C# hat mit `event` und `delegate` einen eingebauten Observer-Mechanismus:

// %%
class TemperatureSensorCSharp
{
    public event Action<double> TemperatureChanged;
    public event Action<double> Overheating;

    private double temperature;

    public double Temperature
    {
        get => temperature;
        set
        {
            temperature = value;
            TemperatureChanged?.Invoke(temperature);
            if (temperature > 100)
                Overheating?.Invoke(temperature);
        }
    }
}

// %%
var csSensor = new TemperatureSensorCSharp();
csSensor.TemperatureChanged += t => Console.WriteLine($"Display: {t}°C");
csSensor.Overheating += t => Console.WriteLine($"ALARM: {t}°C!");

csSensor.Temperature = 25;
csSensor.Temperature = 110;

// %% [markdown]
//
// ## Weitere wichtige GoF-Patterns
//
// | Pattern | Kategorie | Kernidee |
// |---|---|---|
// | Factory Method | Erzeugung | Objekt-Erzeugung an Unterklassen delegieren |
// | Singleton | Erzeugung | Genau eine Instanz einer Klasse |
// | Adapter | Struktur | Inkompatible Interfaces verbinden |
// | Decorator | Struktur | Verhalten dynamisch hinzufügen |
// | Command | Verhalten | Operationen als Objekte kapseln |
// | Template Method | Verhalten | Algorithmus-Skelett in Basisklasse |

// %% [markdown]
//
// ## Workshop: Strategy Pattern implementieren
//
// Implementieren Sie ein Zahlungssystem mit dem Strategy Pattern:
//
// 1. Erstellen Sie ein Interface `IPaymentStrategy` mit einer
//    Methode `Pay(decimal amount)`
// 2. Implementieren Sie mindestens drei Strategien:
//    - `CreditCardPayment`
//    - `PayPalPayment`
//    - `BankTransferPayment`
// 3. Erstellen Sie eine `ShoppingCart`-Klasse, die eine
//    `IPaymentStrategy` verwendet
// 4. Demonstrieren Sie den Strategiewechsel zur Laufzeit

// %%
interface IPaymentStrategy
{
    void Pay(decimal amount);
}

class CreditCardPayment : IPaymentStrategy
{
    private string cardNumber;
    public CreditCardPayment(string cardNumber) { this.cardNumber = cardNumber; }
    public void Pay(decimal amount)
        => Console.WriteLine($"Paid {amount:C} with credit card {cardNumber[^4..]}");
}

class PayPalPayment : IPaymentStrategy
{
    private string email;
    public PayPalPayment(string email) { this.email = email; }
    public void Pay(decimal amount)
        => Console.WriteLine($"Paid {amount:C} via PayPal ({email})");
}

class BankTransferPayment : IPaymentStrategy
{
    private string iban;
    public BankTransferPayment(string iban) { this.iban = iban; }
    public void Pay(decimal amount)
        => Console.WriteLine($"Paid {amount:C} via bank transfer ({iban})");
}

// %%
class PaymentShoppingCart
{
    private List<(string Name, decimal Price)> items = new();
    private IPaymentStrategy paymentStrategy;

    public PaymentShoppingCart(IPaymentStrategy strategy)
    {
        paymentStrategy = strategy;
    }

    public void AddItem(string name, decimal price) => items.Add((name, price));

    public void SetPaymentStrategy(IPaymentStrategy strategy)
        => paymentStrategy = strategy;

    public void Checkout()
    {
        decimal total = items.Sum(i => i.Price);
        paymentStrategy.Pay(total);
        items.Clear();
    }
}

// %%
var cart = new PaymentShoppingCart(new CreditCardPayment("1234567890123456"));
cart.AddItem("Book", 19.99m);
cart.AddItem("Pen", 2.49m);
cart.Checkout();

cart.SetPaymentStrategy(new PayPalPayment("alice@example.com"));
cart.AddItem("Notebook", 8.99m);
cart.Checkout();
