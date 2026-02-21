// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>SOLID-Prinzipien: Überblick</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// ## Was ist SOLID?
//
// SOLID ist ein Akronym für fünf Entwurfsprinzipien:
//
// - **S**ingle Responsibility Principle (SRP)
// - **O**pen/Closed Principle (OCP)
// - **L**iskov Substitution Principle (LSP)
// - **I**nterface Segregation Principle (ISP)
// - **D**ependency Inversion Principle (DIP)
//
// Ziel: Wartbare, erweiterbare und verständliche Software

// %% [markdown]
//
// ## Single Responsibility Principle (SRP)
//
// *"Eine Klasse sollte nur einen Grund haben, sich zu ändern."*
//
// - Jede Klasse hat genau eine Verantwortlichkeit
// - Änderungen an einer Funktion betreffen nur eine Klasse
// - Hohe Kohäsion innerhalb der Klasse

// %%
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

// %% [markdown]
//
// ### SRP-Verletzung

// %%
class OrderProcessor
{
    public decimal CalculateTotal(List<(string Name, decimal Price, int Qty)> items)
    {
        return items.Sum(i => i.Price * i.Qty);
    }

    public void SaveToDatabase(int orderId, decimal total)
    {
        // Database logic mixed with business logic
        Console.WriteLine($"Saving order {orderId} with total {total}");
    }

    public void SendEmail(string to, int orderId)
    {
        // Email logic mixed with business logic
        Console.WriteLine($"Sending confirmation to {to} for order {orderId}");
    }
}

// %% [markdown]
//
// ### SRP eingehalten

// %%
class OrderCalculator
{
    public decimal CalculateTotal(List<(string Name, decimal Price, int Qty)> items)
        => items.Sum(i => i.Price * i.Qty);
}

class OrderRepository
{
    public void Save(int orderId, decimal total)
        => Console.WriteLine($"Saving order {orderId} with total {total}");
}

class OrderNotification
{
    public void SendConfirmation(string to, int orderId)
        => Console.WriteLine($"Sending confirmation to {to} for order {orderId}");
}

// %% [markdown]
//
// ## Open/Closed Principle (OCP)
//
// *"Software-Entitäten sollten offen für Erweiterungen sein,
// aber geschlossen für Änderungen."*
//
// - Neues Verhalten hinzufügen, ohne bestehenden Code zu ändern
// - Typischerweise durch Interfaces und Polymorphismus
// - Vermeidet wachsende if/else- oder switch-Ketten

// %% [markdown]
//
// ### OCP-Verletzung

// %%
class AreaCalculator
{
    public double CalculateArea(object shape)
    {
        if (shape is Circle c)
            return Math.PI * c.Radius * c.Radius;
        else if (shape is Rectangle r)
            return r.Width * r.Height;
        // Adding a new shape requires changing this method!
        throw new ArgumentException("Unknown shape");
    }
}

record Circle(double Radius);
record Rectangle(double Width, double Height);

// %% [markdown]
//
// ### OCP eingehalten

// %%
interface IShape
{
    double CalculateArea();
}

record CircleShape(double Radius) : IShape
{
    public double CalculateArea() => Math.PI * Radius * Radius;
}

record RectangleShape(double Width, double Height) : IShape
{
    public double CalculateArea() => Width * Height;
}

// New shape: no changes to existing code needed!
record TriangleShape(double Base, double Height) : IShape
{
    public double CalculateArea() => 0.5 * Base * Height;
}

// %%
List<IShape> shapes = new()
{
    new CircleShape(5),
    new RectangleShape(3, 4),
    new TriangleShape(6, 3)
};

double totalArea = shapes.Sum(s => s.CalculateArea());
Console.WriteLine($"Total area: {totalArea:F2}");

// %% [markdown]
//
// ## Liskov Substitution Principle (LSP)
//
// *"Objekte einer Basisklasse sollten durch Objekte ihrer Unterklassen
// ersetzt werden können, ohne das Programm zu verändern."*
//
// - Unterklassen müssen den Vertrag der Basisklasse einhalten
// - Keine Überraschungen bei der Verwendung von Polymorphismus
// - Fundamentales Prinzip der Objektorientierung

// %% [markdown]
//
// ### LSP-Verletzung

// %%
class Bird
{
    public virtual void Fly()
    {
        Console.WriteLine("Flying...");
    }
}

class Penguin : Bird
{
    public override void Fly()
    {
        throw new InvalidOperationException("Penguins can't fly!");
    }
}

// %% [markdown]
//
// ### LSP eingehalten

// %%
interface IFlyable
{
    void Fly();
}

class Sparrow : IFlyable
{
    public void Fly() => Console.WriteLine("Sparrow flying...");
}

class PenguinLsp
{
    public void Swim() => Console.WriteLine("Penguin swimming...");
}

// %% [markdown]
//
// ## Interface Segregation Principle (ISP)
//
// *"Clients sollten nicht gezwungen werden, von Interfaces abzuhängen,
// die sie nicht verwenden."*
//
// - Lieber mehrere kleine, spezifische Interfaces
// - Statt eines großen, allgemeinen Interfaces
// - Jedes Interface hat einen klaren Zweck

// %% [markdown]
//
// ### ISP-Verletzung

// %%
interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
}

class Robot : IWorker
{
    public void Work() => Console.WriteLine("Working...");
    public void Eat() => throw new NotSupportedException(); // Robots don't eat!
    public void Sleep() => throw new NotSupportedException(); // Robots don't sleep!
}

// %% [markdown]
//
// ### ISP eingehalten

// %%
interface IWorkable { void Work(); }
interface IFeedable { void Eat(); }
interface ISleepable { void Sleep(); }

class Human : IWorkable, IFeedable, ISleepable
{
    public void Work() => Console.WriteLine("Working...");
    public void Eat() => Console.WriteLine("Eating...");
    public void Sleep() => Console.WriteLine("Sleeping...");
}

class RobotIsp : IWorkable
{
    public void Work() => Console.WriteLine("Working...");
}

// %% [markdown]
//
// ## Dependency Inversion Principle (DIP)
//
// *"High-Level-Module sollten nicht von Low-Level-Modulen abhängen.
// Beide sollten von Abstraktionen abhängen."*
//
// - Abhängigkeiten zeigen in Richtung Abstraktionen
// - Konkrete Implementierungen werden injiziert
// - Ermöglicht leichtes Austauschen und Testen

// %% [markdown]
//
// ### DIP-Verletzung

// %%
class EmailService
{
    public void Send(string to, string message)
        => Console.WriteLine($"Email to {to}: {message}");
}

class UserRegistration
{
    private EmailService emailService = new EmailService(); // Direct dependency!

    public void Register(string email)
    {
        // ... registration logic ...
        emailService.Send(email, "Welcome!");
    }
}

// %% [markdown]
//
// ### DIP eingehalten

// %%
interface INotificationService
{
    void Send(string to, string message);
}

class EmailNotification : INotificationService
{
    public void Send(string to, string message)
        => Console.WriteLine($"Email to {to}: {message}");
}

class SmsNotification : INotificationService
{
    public void Send(string to, string message)
        => Console.WriteLine($"SMS to {to}: {message}");
}

class UserRegistrationDip
{
    private readonly INotificationService notificationService;

    public UserRegistrationDip(INotificationService service) // Injected!
    {
        notificationService = service;
    }

    public void Register(string email)
    {
        // ... registration logic ...
        notificationService.Send(email, "Welcome!");
    }
}

// %%
var registration = new UserRegistrationDip(new EmailNotification());
registration.Register("alice@example.com");

var regSms = new UserRegistrationDip(new SmsNotification());
regSms.Register("+49123456");

// %% [markdown]
//
// ## Zusammenfassung
//
// | Prinzip | Kernaussage |
// |---|---|
// | SRP | Eine Klasse, eine Verantwortlichkeit |
// | OCP | Erweiterbar ohne Änderung |
// | LSP | Unterklassen halten den Vertrag ein |
// | ISP | Kleine, spezifische Interfaces |
// | DIP | Abhängigkeit von Abstraktionen |

// %% [markdown]
//
// ## Workshop: SOLID-Verletzungen erkennen
//
// Analysieren Sie den folgenden Code. Identifizieren Sie, welche
// SOLID-Prinzipien verletzt werden, und schlagen Sie Verbesserungen vor.

// %%
using System;
using System.Collections.Generic;

// %%
class Logger
{
    public void Log(string message, string type)
    {
        if (type == "console")
            Console.WriteLine($"[LOG] {message}");
        else if (type == "file")
            File.AppendAllText("log.txt", $"[LOG] {message}\n");
        // Adding a new log target requires changing this method (OCP violation)
    }
}

class ReportGenerator
{
    private Logger logger = new Logger(); // Direct dependency (DIP violation)

    public string GenerateReport(List<string> data)
    {
        logger.Log("Generating report", "console");
        string report = string.Join("\n", data);
        logger.Log("Report generated", "console");
        SaveReport(report);                // Multiple responsibilities (SRP violation)
        return report;
    }

    private void SaveReport(string report)
    {
        File.WriteAllText("report.txt", report);
    }
}

// %%
