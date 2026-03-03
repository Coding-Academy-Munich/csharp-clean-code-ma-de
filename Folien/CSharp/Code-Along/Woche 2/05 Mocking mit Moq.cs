// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Mocking mit Moq</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// # Das Moq Mocking Framework (für C#)
//
// - [Moq](https://github.com/moq/moq4) ist ein Mocking-Framework für C#
// - Erzeugt automatisch Mock-Objekte für Interfaces
// - Ersetzt handgeschriebene Stubs und Spies

// %%
#r "nuget: Moq, 4.17"
#r "nuget: NUnit, *"

// %%
#load "NUnitTestRunner.cs"

// %% [markdown]
//
// ## Mocks erstellen
//
// - `new Mock<T>()` erzeugt einen Mock für ein Interface `T`
// - `.Object` liefert das Proxy-Objekt, das das Interface implementiert
// - Alle Methoden haben Standardverhalten (nichts tun / Defaultwerte)
// - Der Mock-Wrapper zeichnet jeden Aufruf auf

// %% [markdown]
//
// ## Beispiel: Mocken eines Service

// %%
using Moq;
using System.Collections.Generic;

// %%
public interface IMessageService
{
    void Send(string message);
    string GetMessage(int index);
    void ClearAll();
}

// %%

// %%

// %% [markdown]
//
// ## Aufrufe überprüfen
//
// - `Verify(Ausdruck)` prüft, ob eine Methode aufgerufen wurde
// - Wirft `MockException` bei Fehlschlag (mit Diagnosemeldung)
// - Kann exakte Argumente oder Argument Matcher prüfen
// - Kann Anzahl der Aufrufe mit `Times` einschränken

// %%
mockedService.Verify(svc => svc.Send("Hello!"));

// %%

// %% [markdown]
//
// ## Vergleich: Handgeschriebener Spy vs. Moq
//
// **Vorher** (handgeschriebener Spy):
// ```csharp
// public class CartSpy : IShoppingCart
// {
//     public List<(string Id, double Price)> AddedItems { get; } = new();
//     public void AddItem(string id, double price)
//         => AddedItems.Add((id, price));
// }
//
// // Im Test:
// var spy = new CartSpy();
// manager.AddToCart("book-123");
// Assert.That(spy.AddedItems[0].Id, Is.EqualTo("book-123"));
// ```
//
// **Nachher** (mit Moq):
// ```csharp
// var mock = new Mock<IShoppingCart>();
// manager.AddToCart("book-123");
// mock.Verify(c => c.AddItem("book-123", It.IsAny<double>()));
// ```

// %% [markdown]
//
// ###  Überprüfen der Anzahl der Aufrufe
//
// - `Times.Exactly(n)`: genau n Aufrufe
// - `Times.Never`:
//    - Bestätigt, dass eine Methode **nicht** aufgerufen wurde
//    - Sehr nützlich für negative Tests
//    - Ersetzt die Überprüfung `spy.CallCount == 0` von handgeschriebenen
//      Doubles
// - `Times.AtLeastOnce()`, `Times.AtLeast(n)`, `Times.AtMost(n)` für Bereichsprüfungen

// %%
var mockedServiceCount = new Mock<IMessageService>();
mockedServiceCount.Object.Send("Hello!");

// %% [markdown]
//
// Wurde `Send("Hello!")` mindestens einmal aufgerufen?

// %%

// %%
mockedServiceCount.Object.Send("Hello!");

// %% [markdown]
//
// Gilt das immer noch?

// %%

// %% [markdown]
//
// Wurde `Send("Hello!")` genau zweimal aufgerufen?

// %%

// %% [markdown]
//
// #### Überprüfen, dass eine Methode nicht aufgerufen wurde:

// %%
var mockedServiceNever = new Mock<IMessageService>();

// %%

// %% [markdown]
//
// ### Mindest- und Maximalanzahl von Aufrufen:

// %%
var mockedServiceLimits = new Mock<IMessageService>();

// %%
mockedServiceLimits.Object.Send("Once!");
mockedServiceLimits.Object.Send("Twice!");
mockedServiceLimits.Object.Send("Twice!");
mockedServiceLimits.Object.Send("Three times!");
mockedServiceLimits.Object.Send("Three times!");
mockedServiceLimits.Object.Send("Three times!");

// %% [markdown]
//
// Wurde `Send("Once!")` mindestens einmal aufgerufen?

// %%

// %% [markdown]
//
// Wurde `Send("Once!")` mindestens zweimal aufgerufen?

// %%

// %% [markdown]
//
// Wurde `Send("Twice!")` mindestens einmal aufgerufen?

// %%

// %% [markdown]
//
// Wurde `Send("Twice!")` mindestens zweimal aufgerufen?

// %%

// %% [markdown]
//
// Wurde `Send("Three times!")` höchstens dreimal aufgerufen?

// %%

// %% [markdown]
//
// Wurde `Send("Three times!")` höchstens zweimal aufgerufen?

// %%

// %% [markdown]
//
// Wurde `ClearAll()` höchstens zweimal aufgerufen?

// %%

// %% [markdown]
//
// ## Argument Matcher
//
// - Überprüfung ohne genaue Argumentwerte
// - `It.IsAny<T>()` — passt auf jeden Wert vom Typ T
// - `It.Is<T>(Prädikat)` — passt, wenn das Prädikat true ergibt
// - `It.IsNotNull<T>()` — Kurzform für Null-Check
// - Funktionieren in `Verify()` und `Setup()`

// %%
var mockedServiceMatchers = new Mock<IMessageService>();
mockedServiceMatchers.Object.Send("Hello!");

// %% [markdown]
//
// Wurde `Send()` mit irgendeinem String aufgerufen?

// %%

// %% [markdown]
//
// Wurde `Send()` mit einem nicht-null String aufgerufen?

// %%

// %% [markdown]
//
// Wurde `Send()` mit einem String aufgerufen, der mit "lo!" endet?

// %%

// %% [markdown]
//
// Wurde `Send()` mit einem String aufgerufen, der mit "No" beginnt?

// %%

// %% [markdown]
//
// ### Null-Argumente überprüfen

// %%
var mockedServiceIsNull = new Mock<IMessageService>();
mockedServiceIsNull.Object.Send(null);

// %% [markdown]
//
// Wurde `Send()` mit einem null-Argument aufgerufen?

// %%

// %% [markdown]
//
// Wurde `Send()` mit irgendeinem String aufgerufen (einschließlich null)?

// %%

// %% [markdown]
//
// Wurde `Send()` mit einem String Argument aufgerufen, das nicht null ist?

// %%

// %% [markdown]
//
// ## Stubbing mit Setup/Returns/Throws
//
// - Bisher haben wir Aufrufe überprüft (ausgehendes Verhalten)
// - Jetzt konfigurieren wir Rückgabewerte (eingehendes Verhalten)
// - `Setup().Returns()` ersetzt handgeschriebene Stub-Klassen
// - `Setup().Throws()` simuliert Ausnahmen

// %%
var mockedServiceStub = new Mock<IMessageService>();

// %% [markdown]
//
// `mockedServiceStub.GetMessage(0)` soll "Hello!" zurückgeben

// %%

// %% [markdown]
//
// `mockedServiceStub.GetMessage(1)` soll eine Exception werfen

// %%

// %% [markdown]
//
// Setup prüfen:

// %%

// %%

// %% [markdown]
//
// `Verify()` funktioniert auch bei Stubs:

// %%

// %%

// %% [markdown]
//
// ## Setup vs. Verify
//
// - **Setup** (Stubbing): konfiguriert, was der Mock **zurückgibt** oder
//   **wirft** — kontrolliert eingehende Daten
// - **Verify** (Mocking): prüft, was das SUT mit dem Mock **gemacht hat**
//   — überprüft ausgehendes Verhalten
// - Ein einzelnes `Mock<T>` kann gleichzeitig Stub und Mock sein

// %% [markdown]
//
// ## Mock-Verhalten: Strict vs. Loose
//
// - Standardmäßig verwendet Moq `MockBehavior.Loose`
// - **Loose** (Standard): Nicht konfigurierte Aufrufe geben Defaultwerte
//   zurück (0, null, false)
// - **Strict**: Nicht konfigurierte Aufrufe werfen eine Exception
// - `new Mock<T>(MockBehavior.Strict)` für strenges Verhalten
// - In den meisten Fällen ist Loose empfohlen

// %% [markdown]
//
// ## Verwendung von Moq in Tests
//
// - In NUnit-Tests können Mock-Objekte verwendet werden, um Abhängigkeiten
//   zu simulieren
// - Die `Verify()`-Methode wirft eine Exception, wenn sie einen Fehler
//   findet
// - Das führt zu einem fehlgeschlagenen Test

// %%
using NUnit.Framework;
using Moq;

public class MessageRelay
{
    private readonly IMessageService _service;
    public MessageRelay(IMessageService service) => _service = service;
    public void Relay(string message) => _service.Send(message);
    public string GetFirstMessage() => _service.GetMessage(0);
}

// %%
[TestFixture]
public class MessageServiceTest
{
    [Test]
    public void TestRelay()
    {
        var mockedService = new Mock<IMessageService>();
        var relay = new MessageRelay(mockedService.Object);

        relay.Relay("Hello!");

        mockedService.Verify(m => m.Send("Hello!"));
    }

    [Test]
    public void FailedTest()
    {
        var mockedService = new Mock<IMessageService>();
        var relay = new MessageRelay(mockedService.Object);

        relay.Relay("Hello!");

        mockedService.Verify(m => m.Send("World!")); // This will fail
    }

    [Test]
    public void TestWithSetup()
    {
        var mockedService = new Mock<IMessageService>();
        mockedService.Setup(m => m.GetMessage(0)).Returns("Hello!");

        var relay = new MessageRelay(mockedService.Object);

        Assert.That(relay.GetFirstMessage(), Is.EqualTo("Hello!"));
    }
}

// %%

// %% [markdown]
//
// ## Workshop: Testen eines Bestellsystems
//
// In diesem Workshop arbeiten Sie mit einem einfachen Bestellsystem. Das
// System interagiert mit einem Produktkatalog, einem Zahlungs-Gateway und
// einem Benachrichtigungsdienst.

// %% [markdown]
//
// Hier sind die Interfaces für die Abhängigkeiten des Systems:

// %%
public interface IProductCatalog
{
    double GetPrice(string productId);
    bool IsInStock(string productId);
}

// %%
public interface IPaymentGateway
{
    bool ProcessPayment(string customerId, double amount);
}

// %%
public interface INotificationService
{
    void SendConfirmation(string customerId, string message);
}

// %% [markdown]
//
// Die zu testende Klasse:

// %%
public class OrderProcessor
{
    private readonly IProductCatalog _catalog;
    private readonly IPaymentGateway _payment;
    private readonly INotificationService _notifications;

    public OrderProcessor(
        IProductCatalog catalog,
        IPaymentGateway payment,
        INotificationService notifications)
    {
        _catalog = catalog;
        _payment = payment;
        _notifications = notifications;
    }

    public bool PlaceOrder(string customerId, string productId, int quantity)
    {
        if (!_catalog.IsInStock(productId))
        {
            _notifications.SendConfirmation(customerId,
                $"Product {productId} is out of stock.");
            return false;
        }

        double price = _catalog.GetPrice(productId);
        double total = price * quantity;

        if (!_payment.ProcessPayment(customerId, total))
        {
            _notifications.SendConfirmation(customerId,
                "Payment failed. Please try again.");
            return false;
        }

        _notifications.SendConfirmation(customerId,
            $"Order confirmed: {quantity}x {productId} for {total:F2}.");
        return true;
    }
}

// %% [markdown]
//
// Ihre Aufgabe ist es, Tests für den `OrderProcessor` unter Verwendung
// von Moq zu schreiben. Implementieren Sie die folgenden Testfälle:
//
// 1. Erfolgreiche Bestellung:
//    - Produkt ist auf Lager, Zahlung erfolgreich
//    - Überprüfen Sie, dass eine Bestätigung gesendet wird
// 2. Produkt nicht auf Lager:
//    - Überprüfen Sie, dass keine Zahlung verarbeitet wird
//    - Überprüfen Sie, dass eine "nicht auf Lager"-Nachricht gesendet wird
// 3. Zahlung fehlgeschlagen:
//    - Produkt ist auf Lager, aber die Zahlung schlägt fehl
//    - Überprüfen Sie, dass eine Fehlermeldung gesendet wird
// 4. Korrekter Betrag an Zahlungs-Gateway:
//    - Verwenden Sie Argument Matcher, um zu überprüfen, dass der richtige
//      Betrag (Preis * Menge) an das Payment-Gateway übergeben wird

// %% [markdown]
//
// #### Bonusaufgabe:
//
// 5. Katalog wirft eine Exception:
//    - Verwenden Sie `Setup().Throws()`, um einen Katalogfehler zu
//      simulieren
//    - Ändern Sie den `OrderProcessor`, um Ausnahmen zu behandeln, und
//      schreiben Sie einen Test dafür

// %%
using NUnit.Framework;
using Moq;

// %%

// %%

// %% [markdown]
//
// ## Optionaler Workshop: Raumschiff-Steuerung mit Moq
//
// Im vorherigen Workshop (Test Doubles) haben wir das
// `SpacecraftControlSystem` mit handgeschriebenen Stubs und Spies getestet.
// Implementieren Sie die gleichen Tests jetzt mit Moq und vergleichen
// Sie die Ergebnisse:
//
// - Ersetzen Sie die handgeschriebenen Stubs (`TemperatureSensorStub`,
//   `FuelSensorStub`) durch `Mock<ITemperatureSensor>` und
//   `Mock<IFuelSensor>` mit `Setup/Returns`
// - Ersetzen Sie den `RadioTransmitterSpy` durch `Mock<IRadioTransmitter>`
//   mit `Verify`
// - Vergleichen Sie den Code: Welche Version ist kürzer? Welche ist
//   verständlicher?

// %%
