// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Test-Doubles</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// ## Warum Test-Doubles?
//
// - Echte Abhängigkeiten können Tests problematisch machen:
//   - Nicht-deterministisch (Zeit, Zufallszahlen)
//   - Langsam (Datenbank, Netzwerk, Dateisystem)
//   - Schwer aufzusetzen (komplexe Objektgraphen, externe Services)
// - Test-Doubles geben uns Kontrolle über diese Abhängigkeiten

// %% [markdown]
//
// ## Test Doubles
//
// - Test Double: Vereinfachte Version einer Abhängigkeit im System
//   - z.B. Ersetzen einer Datenbankabfrage durch einen fixen Wert
// - Test Doubles sind wichtig zum Vereinfachen von Tests
// - Sie benötigen typischerweise ein Interface, das sie implementieren
// - Aber: zu viele oder komplexe Test Doubles machen Tests unübersichtlich
//   - Was wird von einem Test eigentlich getestet?

// %% [markdown]
//
// ## Arten von Test Doubles
//
// - Ausgehende Abhängigkeiten ("Mocks")
//   - Mocks
//   - Spies
// - Eingehende Abhängigkeiten ("Stubs")
//   - Dummies
//   - Stubs
//   - Fakes

// %% [markdown]
//
// ## Dummy
//
// - Objekt, das nur als Platzhalter dient
// - Wird übergeben, aber nicht verwendet
// - In C# manchmal `null`
// - Auch für ausgehende Abhängigkeiten

// %% [markdown]
//
// ## Beispiel: Dummy
//
// ```csharp
// var manager = new CartManager(priceProvider, cart: null!);
// ```

// %% [markdown]
//
// ## Stub
//
// - Objekt, das eine minimale Implementierung einer Abhängigkeit bereitstellt
// - Gibt typischerweise immer den gleichen Wert zurück
// - Wird verwendet um
//   - komplexe Abhängigkeiten zu ersetzen
//   - Tests deterministisch zu machen

// %% [markdown]
//
// ## Beispiel: Stub
//
// ```csharp
// public class FixedPriceProvider : IProductPriceProvider
// {
//     public double GetPrice(string productId) => 99.99;
// }
// ```

// %% [markdown]
//
// ## Fake
//
// - Objekt, das eine einfachere Implementierung einer Abhängigkeit bereitstellt
// - Kann z.B. eine In-Memory-Datenbank sein
// - Wird verwendet um
//   - Tests zu beschleunigen
//   - Konfiguration von Tests zu vereinfachen

// %% [markdown]
//
// ## Beispiel: Fake
//
// ```csharp
// public class InMemoryProductCatalog : IProductCatalog
// {
//     private readonly Dictionary<string, double> prices = new();
//     public void SetPrice(string id, double price) => prices[id] = price;
//     public double GetPrice(string id) => prices[id];
// }
// ```

// %% [markdown]
//
// ## Spy
//
// - Objekt, das Informationen über die Interaktion mit ihm speichert
// - Wird verwendet um
//   - zu überprüfen, ob eine Abhängigkeit korrekt verwendet wird

// %% [markdown]
//
// ## Beispiel: Spy
//
// ```csharp
// public class CartSpy : IShoppingCart
// {
//     public List<(string Id, double Price)> AddedItems { get; } = new();
//     public void AddItem(string id, double price) => AddedItems.Add((id, price));
// }
// ```

// %% [markdown]
//
// ## Mock
//
// - Objekt, das Information über die erwartete Interaktion speichert
// - Typischerweise deklarativ konfigurierbar
// - Automatisierte Implementierung von Spies
// - Wird verwendet um
//   - zu überprüfen, ob eine Abhängigkeit korrekt verwendet wird

// %% [markdown]
//
// ## Beispiel: Mock
//
// ```csharp
// var mockCart = new Mock<IShoppingCart>();
// // ... use mockCart.Object in test ...
// mockCart.Verify(c => c.AddItem("book-123", 99.99), Times.Once);
// ```

// %% [markdown]
// ## Beispiel: E-Commerce System
//
// Wir wollen einen Artikel in einen Warenkorb legen.
//
// - Der `CartManager` ist die zu testende Klasse (System Under Test).
// - Er benutzt einen `IProductPriceProvider`, um den Preis eines Artikels zu
//   erfahren (eingehende Abhängigkeit).
// - Er benutzt einen `IShoppingCart`, um den Artikel hinzuzufügen (ausgehende
//   Abhängigkeit).
//
// Zuerst definieren wir die Interfaces für unsere Abhängigkeiten:

// %%
public interface IProductPriceProvider
{
    double GetPrice(string productId);
}

// %%
public interface IShoppingCart
{
    void AddItem(string productId, double price);
}

// %% [markdown]
//
// ## Die zu testende Klasse: `CartManager`
//
// Der `CartManager` holt sich den Preis und fügt den Artikel dem Warenkorb
// hinzu.

// %%
public class CartManager
{
    private readonly IProductPriceProvider priceProvider;
    private readonly IShoppingCart cart;

    public CartManager(IProductPriceProvider priceProvider, IShoppingCart cart)
    {
        this.priceProvider = priceProvider;
        this.cart = cart;
    }

    public void AddToCart(string productId)
    {
        double price = priceProvider.GetPrice(productId);
        cart.AddItem(productId, price);
    }
}

// %% [markdown]
//
// ## Die Test-Doubles: Stub und Spy
//
// - Für den `IProductPriceProvider` verwenden wir einen **Stub**, der einen
//   festen Preis zurückgibt.
// - Für den `IShoppingCart` verwenden wir einen **Spy**, der sich merkt,
//   welche Artikel hinzugefügt wurden.

// %%
public class ProductPriceProviderStub : IProductPriceProvider
{
    public double GetPrice(string productId) { return 99.99; }
}

// %%
public class ShoppingCartSpy : IShoppingCart
{
    public record AddedItem(string ProductId, double Price);
    public List<AddedItem> Items { get; } = new();

    public void AddItem(string productId, double price)
    {
        Items.Add(new AddedItem(productId, price));
    }
}

// %% [markdown]
//
// Im Test überprüfen wir, ob der `CartManager` den `IShoppingCart` korrekt
// verwendet hat.

// %%
#r "nuget: NUnit, *"
#load "NUnitTestRunner.cs"
using NUnit.Framework;
using static NUnitTestRunner;

// %%
void TestCartManagerAddsItemWithCorrectPrice()
{
    var priceProvider = new ProductPriceProviderStub();
    var cart = new ShoppingCartSpy();
    var manager = new CartManager(priceProvider, cart);
    string productId = "book-123";

    manager.AddToCart(productId);

    Assert.That(cart.Items.Count, Is.EqualTo(1));
    Assert.That(cart.Items[0].ProductId, Is.EqualTo(productId));
    Assert.That(cart.Items[0].Price, Is.EqualTo(99.99));
}

// %%
TestCartManagerAddsItemWithCorrectPrice();

// %% [markdown]
//
// ## Typischer Einsatz von Test Doubles
//
// - Zugriff auf Datenbank, Dateisystem
// - Zeit, Zufallswerte
// - Nichtdeterminismus
// - Verborgener Zustand

// %% [markdown]
//
// ## Workshop: Test eines Steuerungssystems für ein Raumschiff
//
// In diesem Workshop arbeiten Sie mit einem einfachen Steuerungssystem für ein
// Raumschiff. Das System interagiert mit verschiedenen Sensoren und einem
// Funksender. Ihre Aufgabe ist es, Tests für dieses System mit
// handgeschriebenen Test-Doubles zu schreiben.

// %% [markdown]
//
// Hier ist eine einfache Implementierung unseres Raumschiff-Steuerungssystems:

// %%
public interface ITemperatureSensor
{
    double GetTemperature();
}

// %%
public interface IFuelSensor
{
    double GetFuelLevel();
}

// %%
public interface IRadioTransmitter
{
    void Transmit(string message);
}

// %%
public class SpacecraftControlSystem
{
    private readonly ITemperatureSensor _tempSensor;
    private readonly IFuelSensor _fuelSensor;
    private readonly IRadioTransmitter _radio;

    public SpacecraftControlSystem(ITemperatureSensor tempSensor, IFuelSensor fuelSensor, IRadioTransmitter radio)
    {
        _tempSensor = tempSensor;
        _fuelSensor = fuelSensor;
        _radio = radio;
    }

    public void CheckAndReportStatus()
    {
        var temp = _tempSensor.GetTemperature();
        var fuel = _fuelSensor.GetFuelLevel();

        if (temp > 100)
        {
            _radio.Transmit("Warning: High temperature!");
        }

        if (fuel < 10)
        {
            _radio.Transmit("Warning: Low fuel!");
        }

        _radio.Transmit($"Status: Temperature {temp}, Fuel {fuel}");
    }
}

// %% [markdown]
//
// Ihre Aufgabe ist es, Tests für das `SpacecraftControlSystem` unter
// Verwendung von handgeschriebenen Test-Doubles zu schreiben. Implementieren
// Sie die folgenden Testfälle:
//
// 1. Testen des normalen Betriebs:
//    - Überprüfen Sie den normalen Betrieb des Raumschiffs, wenn die
//      Temperatur normal und der Kraftstoffstand ausreichend ist.
//    - Stellen Sie sicher, dass keine Warnungen gesendet werden.
// 2. Testen der Warnung bei hoher Temperatur:
//    - Überprüfen Sie, dass eine Warnung gesendet wird, wenn die Temperatur
//      über 100 Grad liegt.
// 3. Testen der Warnung bei niedrigem Kraftstoffstand:
//    - Überprüfen Sie, dass eine Warnung gesendet wird, wenn der
//      Kraftstoffstand unter 10 liegt.
// 4. Testen mehrerer Warnungen:
//    - Überprüfen Sie, dass sowohl eine Warnung bei hoher Temperatur als
//      auch bei niedrigem Kraftstoffstand gesendet wird, wenn beide
//      Bedingungen erfüllt sind.

// %% [markdown]
//
// #### Bonusaufgabe:
//
// 5. Testen der Fehlerbehandlung:
//    - Ändern Sie das `SpacecraftControlSystem`, um Ausnahmen von den
//      Sensoren zu behandeln, und schreiben Sie einen Test, um dieses
//      Verhalten zu überprüfen.

// %% [markdown]
//
// ## Lösung: Test-Doubles
//
// Wir benötigen konfigurierbare Stubs für `ITemperatureSensor` und
// `IFuelSensor` und einen Spy für `IRadioTransmitter`.

// %%
public class TemperatureSensorStub : ITemperatureSensor
{
    public double Temperature { get; set; } = 75.0;
    public double GetTemperature() { return Temperature; }
}

// %%
public class FuelSensorStub : IFuelSensor
{
    public double FuelLevel { get; set; } = 50.0;
    public double GetFuelLevel() { return FuelLevel; }
}

// %%
public class RadioTransmitterSpy : IRadioTransmitter
{
    public List<string> Messages { get; } = new();

    public void Transmit(string message)
    {
        Messages.Add(message);
    }
}

// %% [markdown]
// ## Lösung: Tests

// %%
void TestNormalOperation()
{
    var tempSensor = new TemperatureSensorStub();
    var fuelSensor = new FuelSensorStub();
    var radio = new RadioTransmitterSpy();

    var spacecraft = new SpacecraftControlSystem(tempSensor, fuelSensor, radio);
    spacecraft.CheckAndReportStatus();

    Assert.That(radio.Messages.Count, Is.EqualTo(1));
    Assert.That(radio.Messages[0], Is.EqualTo("Status: Temperature 75, Fuel 50"));
}

// %%
void TestHighTemperatureWarning()
{
    var tempSensor = new TemperatureSensorStub { Temperature = 110.0 };
    var fuelSensor = new FuelSensorStub();
    var radio = new RadioTransmitterSpy();

    var spacecraft = new SpacecraftControlSystem(tempSensor, fuelSensor, radio);
    spacecraft.CheckAndReportStatus();

    Assert.That(radio.Messages, Does.Contain("Warning: High temperature!"));
    Assert.That(radio.Messages, Does.Contain("Status: Temperature 110, Fuel 50"));
    Assert.That(radio.Messages, Does.Not.Contain("Warning: Low fuel!"));
}

// %%
void TestLowFuelWarning()
{
    var tempSensor = new TemperatureSensorStub();
    var fuelSensor = new FuelSensorStub { FuelLevel = 5.0 };
    var radio = new RadioTransmitterSpy();

    var spacecraft = new SpacecraftControlSystem(tempSensor, fuelSensor, radio);
    spacecraft.CheckAndReportStatus();

    Assert.That(radio.Messages, Does.Contain("Warning: Low fuel!"));
    Assert.That(radio.Messages, Does.Contain("Status: Temperature 75, Fuel 5"));
    Assert.That(radio.Messages, Does.Not.Contain("Warning: High temperature!"));
}

// %%
void TestMultipleWarnings()
{
    var tempSensor = new TemperatureSensorStub { Temperature = 110.0 };
    var fuelSensor = new FuelSensorStub { FuelLevel = 5.0 };
    var radio = new RadioTransmitterSpy();

    var spacecraft = new SpacecraftControlSystem(tempSensor, fuelSensor, radio);
    spacecraft.CheckAndReportStatus();

    Assert.That(radio.Messages, Does.Contain("Warning: High temperature!"));
    Assert.That(radio.Messages, Does.Contain("Warning: Low fuel!"));
    Assert.That(radio.Messages, Does.Contain("Status: Temperature 110, Fuel 5"));
}

// %%
TestNormalOperation();
TestHighTemperatureWarning();
TestLowFuelWarning();
TestMultipleWarnings();

// %% [markdown]
// ## Bonusaufgabe: Fehlerbehandlung

// %%
public class SpacecraftControlSystemWithExceptionHandling
{
    private readonly ITemperatureSensor _tempSensor;
    private readonly IFuelSensor _fuelSensor;
    private readonly IRadioTransmitter _radio;

    public SpacecraftControlSystemWithExceptionHandling(
        ITemperatureSensor tempSensor, IFuelSensor fuelSensor, IRadioTransmitter radio)
    {
        _tempSensor = tempSensor;
        _fuelSensor = fuelSensor;
        _radio = radio;
    }

    public void CheckAndReportStatus()
    {
        try
        {
            var temp = _tempSensor.GetTemperature();
            var fuel = _fuelSensor.GetFuelLevel();

            if (temp > 100)
            {
                _radio.Transmit("Warning: High temperature!");
            }

            if (fuel < 10)
            {
                _radio.Transmit("Warning: Low fuel!");
            }

            _radio.Transmit($"Status: Temperature {temp}, Fuel {fuel}");
        }
        catch (Exception e)
        {
            _radio.Transmit("Error: Sensor malfunction - " + e.Message);
        }
    }
}

// %%
public class ThrowingTemperatureSensorStub : ITemperatureSensor
{
    public double GetTemperature()
    {
        throw new Exception("Temperature sensor failure");
    }
}

// %%
void TestExceptionHandling()
{
    var tempSensor = new ThrowingTemperatureSensorStub();
    var fuelSensor = new FuelSensorStub();
    var radio = new RadioTransmitterSpy();

    var spacecraft = new SpacecraftControlSystemWithExceptionHandling(
        tempSensor, fuelSensor, radio);
    spacecraft.CheckAndReportStatus();

    Assert.That(radio.Messages.Count, Is.EqualTo(1));
    Assert.That(radio.Messages[0],
        Is.EqualTo("Error: Sensor malfunction - Temperature sensor failure"));
}

// %%
TestExceptionHandling();
