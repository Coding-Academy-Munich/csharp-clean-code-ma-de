// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Mocking mit NSubstitute</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>

// %% [markdown]
//
// # Was ist NSubstitute?
//
// - [NSubstitute](https://nsubstitute.github.io/) ist ein Mocking-Framework
//   fuer C#
// - Einfachere, lesbarere Syntax als Moq
// - Kein `.Object`-Property noetig: Substitute wird direkt verwendet
// - Unterstuetzt Verifikation, Stubbing, Argument-Matcher

// %%
#r "nuget: NSubstitute, *"
#r "nuget: NUnit, *"

// %%
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

// %%
#load "NunitTestRunner.cs"

// %% [markdown]
//
// ## Beispiel: Mocken einer Liste
//
// - Erstellen eines Substitute-Objekts fuer eine Liste
// - Implementiert alle Methoden des `IList<T>`-Interfaces
// - Kann verwendet werden, um Methodenaufrufe zu ueberpruefen

// %%

// %%

// %%
mockedList.Received().Add("Hello!");

// %%

// %%

// %% [markdown]
//
// ## Ueberpruefen der Anzahl der Aufrufe
//
// - `Received(n)` prueft, ob eine Methode genau `n` Mal aufgerufen wurde
// - `DidNotReceive()` prueft, ob eine Methode nicht aufgerufen wurde

// %%
var mockedListCount = Substitute.For<IList<string>>();

// %%

// %%

// %%

// %%

// %%

// %% [markdown]
//
// - Ueberpruefen, dass eine Methode nicht aufgerufen wurde:

// %%
var mockedListNever = Substitute.For<IList<string>>();

// %%

// %% [markdown]
//
// ## Argument-Matcher
//
// - `Arg.Any<T>()`: beliebiger Wert vom Typ `T`
// - `Arg.Is<T>(predicate)`: Wert, der das Praedikat erfuellt
// - Koennen bei Verifikation und Stubbing verwendet werden

// %%
var mockedListMatchers = Substitute.For<IList<string>>();
mockedListMatchers.Add("Hello!");

// %%

// %%

// %%

// %%
var mockedListIsNull = Substitute.For<IList<string>>();
mockedListIsNull.Add(null);

// %%

// %% [markdown]
//
// ## Stubbing (Rueckgabewerte und Exceptions)
//
// - `method.Returns(value)`: definiert den Rueckgabewert
// - `method.Throws(exception)`: wirft eine Exception (erfordert
//   `using NSubstitute.ExceptionExtensions;`)
// - Kein `Setup()` noetig: direkt auf dem Substitute aufrufen

// %%
var mockedListStub = Substitute.For<IList<string>>();

// %%

// %%

// %%

// %%

// %%

// %%

// %% [markdown]
//
// ## Verwendung von NSubstitute in NUnit-Tests
//
// - In NUnit-Tests koennen Substitute-Objekte verwendet werden, um
//   Abhaengigkeiten zu simulieren
// - `Received()` wirft eine Exception, wenn die Verifikation fehlschlaegt
// - Das fuehrt zu einem fehlgeschlagenen Test

// %%
using NSubstitute;
using NUnit.Framework;

[TestFixture]
public class ListTest
{
    [Test]
    public void TestList()
    {
        var mockedList = Substitute.For<IList<string>>();

        mockedList.Add("Hello!");

        mockedList.Received().Add("Hello!");
    }

    [Test]
    public void FailedTest()
    {
        var mockedList = Substitute.For<IList<string>>();

        mockedList.Add("Hello!");

        mockedList.Received().Add("World!"); // This will fail
    }
}

// %%

// %% [markdown]
//
// ## Vergleich: NSubstitute vs. Moq
//
// | Moq | NSubstitute |
// |-----|-------------|
// | `new Mock<IFoo>()` | `Substitute.For<IFoo>()` |
// | `mock.Object` | direkt verwenden |
// | `mock.Setup(x => x.M()).Returns(v)` | `sub.M().Returns(v)` |
// | `mock.Verify(x => x.M())` | `sub.Received().M()` |
// | `Times.Never` | `DidNotReceive()` |
// | `Times.Exactly(n)` | `Received(n)` |
// | `It.IsAny<T>()` | `Arg.Any<T>()` |
// | `It.Is<T>(p)` | `Arg.Is<T>(p)` |

// %% [markdown]
//
// ## Workshop: Test eines Steuerungssystems fuer ein Raumschiff
//
// In diesem Workshop arbeiten Sie mit einem einfachen Steuerungssystem fuer ein
// Raumschiff. Das System interagiert mit verschiedenen Sensoren und einem
// Funksender. Ihre Aufgabe ist es, Tests fuer dieses System mit NSubstitute zu
// schreiben.
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
// ## Beispiel
//
// So koennten Sie dieses System verwenden:

// %%
public class RealTemperatureSensor : ITemperatureSensor
{
    public double GetTemperature()
    {
        return 75.0; // Simulated temperature reading
    }
}

// %%
public class RealFuelSensor : IFuelSensor
{
    public double GetFuelLevel()
    {
        return 50.0; // Simulated fuel level
    }
}

// %%
public class RealRadioTransmitter : IRadioTransmitter
{
    public void Transmit(string message)
    {
        Console.WriteLine("Transmitting: " + message);
    }
}

// %%
ITemperatureSensor realTempSensor = new RealTemperatureSensor();
IFuelSensor realFuelSensor = new RealFuelSensor();
IRadioTransmitter realRadio = new RealRadioTransmitter();

// %%
SpacecraftControlSystem spacecraft = new SpacecraftControlSystem(
                                            realTempSensor, realFuelSensor, realRadio);

// %%
spacecraft.CheckAndReportStatus();

// %% [markdown]
//
// Ihre Aufgabe ist es, Tests fuer das `SpacecraftControlSystem` unter Verwendung
// von NSubstitute zu schreiben. Implementieren Sie die folgenden Testfaelle:
//
// 1. Testen des normalen Betriebs:
//    - Ueberpruefen Sie den normalen Betrieb des Raumschiffs, wenn die Temperatur
//      normal und der Kraftstoffstand ausreichend ist.
// 2. Testen der Warnung bei hoher Temperatur:
//    - Ueberpruefen Sie, dass das Raumschiff eine Warnung bei einer Temperatur
//      ueber 100 Grad uebertraegt.
// 3. Testen der Warnung bei niedrigem Kraftstoffstand:
//    - Ueberpruefen Sie, dass das Raumschiff eine Warnung bei einem Kraftstoffstand
//      unter 10 uebertraegt.
// 4. Testen mehrerer Warnungen:
//    - Ueberpruefen Sie, dass das Raumschiff sowohl eine Warnung bei hoher Temperatur
//      als auch bei niedrigem Kraftstoffstand uebertraegt, wenn beide Bedingungen
//      erfuellt sind.

// %% [markdown]
//
// #### Bonusaufgabe:
//
// 5. Testen der Fehlerbehandlung:
//    - Aendern Sie das `SpacecraftControlSystem`, um Ausnahmen von den Sensoren
//      zu behandeln, und schreiben Sie einen Test, um dieses Verhalten zu
//      ueberpruefen.

// %%
using NSubstitute;
using NUnit.Framework;

// %%

// %%

// %%
using NSubstitute;
using NUnit.Framework;

// %%

// %%

// %%
