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
// ## Stub
//
// - Objekt, das eine minimale Implementierung einer Abhängigkeit bereitstellt
// - Gibt typischerweise immer den gleichen Wert zurück
// - Wird verwendet um
//   - komplexe Abhängigkeiten zu ersetzen
//   - Tests deterministisch zu machen

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
// ## Spy
//
// - Objekt, das Informationen über die Interaktion mit ihm speichert
// - Wird verwendet um
//   - zu überprüfen, ob eine Abhängigkeit korrekt verwendet wird

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
void Check(bool condition)
{
    if (!condition)
    {
        Console.WriteLine("Test failed!");
    }
    else
    {
        Console.WriteLine("Success!");
    }
}

// %%
void TestCartManagerAddsItemWithCorrectPrice()
{
    var priceProvider = new ProductPriceProviderStub();
    var cart = new ShoppingCartSpy();
    var manager = new CartManager(priceProvider, cart);
    string productId = "book-123";

    manager.AddToCart(productId);

    Check(cart.Items.Count == 1);
    Check(cart.Items[0].ProductId == productId);
    Check(cart.Items[0].Price == 99.99);
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
// ## Workshop: Raumschiff-Steuerung testen
//
// Sie sollen eine Test-Suite für einen `SpacecraftCommandController`
// implementieren. Diese Klasse sendet Befehle an ein Raumschiff. Ihre
// Aufgaben sind:
//
// - Die Missions-Sicherheit hat Priorität: Überprüfen Sie den Zustand des
//   Systems (`ITelemetrySystem`), bevor Sie Befehle ausführen.
// - Führen Sie Manöver mit den Schubdüsen durch (`IThrusterControl`).
// - Benachrichtigen Sie die Bodenstation über den Status von Befehlen
//   (`IGroundControlLink`).

// %% [markdown]
// Zuerst die Interfaces für die Subsysteme des Raumschiffs:

// %%
public enum SubSystem { Core, Thrusters, ScienceBay }

// %%
public interface ITelemetrySystem
{
    int GetPowerLevelPercent(SubSystem system);
}

// %%
public interface IThrusterControl
{
    void FireThrusters(int durationMs);
}

// %%
public interface IGroundControlLink
{
    void SendStatusReport(string report);
}

// %% [markdown]
// Die zu testende Klasse: `SpacecraftCommandController`.

// %%
public class SpacecraftCommandController
{
    private readonly ITelemetrySystem telemetry;
    private readonly IThrusterControl thrusters;
    private readonly IGroundControlLink groundControl;

    public SpacecraftCommandController(
        ITelemetrySystem telemetry,
        IThrusterControl thrusters,
        IGroundControlLink groundControl)
    {
        this.telemetry = telemetry;
        this.thrusters = thrusters;
        this.groundControl = groundControl;
    }

    public void ExecuteBurnManeuver(int durationMs)
    {
        int powerLevel = telemetry.GetPowerLevelPercent(SubSystem.Thrusters);
        if (powerLevel < 50)
        {
            groundControl.SendStatusReport(
                "ERROR: Thruster power too low for maneuver.");
            return;
        }

        thrusters.FireThrusters(durationMs);
        groundControl.SendStatusReport("SUCCESS: Maneuver executed.");
    }
}

// %% [markdown]
//
// ## Ihre Aufgabe
//
// Schreiben Sie Tests für die folgenden Szenarien. Implementieren Sie dafür
// die notwendigen Test-Doubles.
//
// 1.  **Erfolgreiches Manöver:** Das Schubdüsen-System hat genug Energie
//     (>50%). Überprüfen Sie, ob der `FireThrusters`-Befehl gesendet wird und
//     eine Erfolgsmeldung an die Bodenstation geht.
// 2.  **Manöver abgebrochen (zu wenig Energie):** Das Schubdüsen-System hat
//     zu wenig Energie (<50%). Überprüfen Sie, ob die Schubdüsen **nicht**
//     gezündet werden und eine Fehlermeldung an die Bodenstation geht.

// %% [markdown]
// ## Lösung: Test-Doubles
//
// Wir benötigen einen konfigurierbaren Stub für `ITelemetrySystem` und Spies
// für `IThrusterControl` und `IGroundControlLink`.

// %%
public class TelemetrySystemStub : ITelemetrySystem
{
    public int PowerLevel { get; set; } = 100;
    public int GetPowerLevelPercent(SubSystem system) { return PowerLevel; }
}

// %%
public class ThrusterControlSpy : IThrusterControl
{
    public int BurnDurationMs { get; private set; }
    public int TimesFired { get; private set; }

    public void FireThrusters(int durationMs)
    {
        TimesFired++;
        BurnDurationMs = durationMs;
    }
}

// %%
public class GroundControlLinkSpy : IGroundControlLink
{
    public List<string> Reports { get; } = new();

    public void SendStatusReport(string report)
    {
        Reports.Add(report);
    }
}

// %% [markdown]
// ## Lösung: Tests

// %%
void TestSuccessfulBurnManeuver()
{
    var telemetry = new TelemetrySystemStub();
    telemetry.PowerLevel = 75;

    var thrusters = new ThrusterControlSpy();
    var groundControl = new GroundControlLinkSpy();

    var controller = new SpacecraftCommandController(
        telemetry, thrusters, groundControl);

    controller.ExecuteBurnManeuver(500);

    Check(thrusters.TimesFired == 1);
    Check(thrusters.BurnDurationMs == 500);
    Check(groundControl.Reports.Count == 1);
    Check(groundControl.Reports[0] == "SUCCESS: Maneuver executed.");
}

// %%
void TestAbortedManeuverDueToLowPower()
{
    var telemetry = new TelemetrySystemStub();
    telemetry.PowerLevel = 40;

    var thrusters = new ThrusterControlSpy();
    var groundControl = new GroundControlLinkSpy();

    var controller = new SpacecraftCommandController(
        telemetry, thrusters, groundControl);

    controller.ExecuteBurnManeuver(500);

    Check(thrusters.TimesFired == 0);
    Check(groundControl.Reports.Count == 1);
    Check(groundControl.Reports[0] ==
        "ERROR: Thruster power too low for maneuver.");
}

// %%
TestSuccessfulBurnManeuver();
TestAbortedManeuverDueToLowPower();
