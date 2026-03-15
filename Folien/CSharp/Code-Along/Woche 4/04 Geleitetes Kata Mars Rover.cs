// -*- coding: utf-8 -*-
// %% [markdown]
//
// <div style="text-align:center; font-size:200%;">
//  <b>Geleitetes Kata: Mars Rover</b>
// </div>
// <br/>
// <div style="text-align:center; font-size:120%;">Dr. Matthias Hölzl</div>
// <br/>
//
// <div style="text-align:center;">Coding-Akademie München</div>
// <br/>


// %% [markdown]
//
// ## Geleitetes Kata: Mars Rover
//
// - Rover navigiert auf einem Grid
// - Hat Position `(x, y)` und Richtung (N, E, S, W)
// - Empfängt Befehlsfolgen:
//   - `L` -- 90 Grad nach links drehen
//   - `R` -- 90 Grad nach rechts drehen
//   - `M` -- ein Gitterfeld vorwärts bewegen

// %% [markdown]
//
// ### 1. Zyklus: Erzeugen des Rovers
//
// #### ROT:
// - Test: neuer `Rover` steht an `(0, 0)`, Richtung `N`
// - Schlägt fehl -- `Rover`-Klasse existiert noch nicht

// %% [markdown]
//
// #### GRÜN:
//
// - `Rover`-Klasse, `Point` (record struct) und `Direction` (enum) erstellen
// - Test besteht

// %% [markdown]
//
// ### 2. Zyklus: Verhalten hinzufügen - Drehen nach rechts
//
// #### ROT:
// - Test: Befehl `R` an nach `N` ausgerichteten Rover => Richtung `E`
// - Schlägt fehl -- keine `ExecuteCommands`-Methode vorhanden

// %% [markdown]
//
// #### GRÜN:
//
// - `ExecuteCommands`-Methode mit einfacher Logik implementieren

// %% [markdown]
//
// ### 3. Zyklus: Zweimal nach rechts drehen
//
// #### ROT:
// - Test: Befehl `RR` an nach `N` ausgerichteten Rover => Richtung `S`
// - Schlägt fehl

// %% [markdown]
//
// #### GRÜN:
// - `ExecuteCommands` um `"RR"`-Fall erweitern

// %% [markdown]
//
// #### REFACTOR (1):
//
// - Ad-hoc-Logik durch Schleife ersetzen
// - Jeden Befehl einzeln als `char` verarbeiten

// %% [markdown]
//
// #### REFACTOR (2):
//
// - Drehlogik in eigene Methode `TurnRight` extrahieren
// - Rover in `[SetUp]`-Methode erzeugen (Member-Variable)
// - Zwei Einzeltests durch parametrisierten Test ersetzen

// %% [markdown]
//
// ### 4. Zyklus: Mehrfach nach rechts drehen
//
// #### ROT:
//
// - Parametrisierten Test um `RRR` und `RRRR` erweitern
// - Schlägt fehl

// %% [markdown]
//
// #### GRÜN & REFACTOR:
//
// - `TurnRight` um alle vier Richtungen erweitern
// - Parametrisierter Test ist jetzt grün

// %% [markdown]
//
// ### 5. Zyklus: Drehen nach links
//
// #### ROT:
//
// - Parametrisierten Test um ein- bis viermaliges Linksdrehen erweitern
// - Schlägt fehl

// %% [markdown]
//
// #### GRÜN & REFACTOR:
//
// - `TurnLeft`-Methode implementieren
// - `ExecuteCommands` um `L`-Befehl erweitern
// - Parametrisierter Test ist jetzt grün

// %% [markdown]
//
// ### 6. Zyklus: Bewegen
//
// #### ROT:
// - Test: `M` an Rover bei `(10, 10)`, Richtung `N` => Position `(10, 11)`
// - Schlägt fehl

// %% [markdown]
//
// #### GRÜN & REFACTOR:
//
// - Bewegungslogik für Richtung `N` implementieren
// - Test ist jetzt grün

// %% [markdown]
//
// ### 7. Zyklus: Bewegen in alle Richtungen
//
// #### ROT:
// - Parametrisierten Test für alle vier Bewegungsrichtungen erstellen
// - Schlägt fehl

// %% [markdown]
//
// #### GRÜN & REFACTOR:
//
// - `Move`-Methode um alle vier Richtungen erweitern
// - Parametrisierter Test ist jetzt grün

// %% [markdown]
//
// ### 8. Zyklus: Design durch eine neue Anforderung treiben
//
// #### ROT:
//
// - Neues Konzept: "Wrapping"
//   - Rover am Grid-Rand erscheint auf der gegenüberliegenden Seite
//   - Grid-Größe ist konfigurierbar
// - Schlägt fehl -- Rover kennt kein Grid

// %% [markdown]
//
// - Design muss überdacht werden für das neue Wrapping-Verhalten
// - Verschiedene Ansätze möglich:
//   - Grid-Größe bei `ExecuteCommands` übergeben:
//     ```csharp
//     rover.ExecuteCommands("M", gridWidth: 10, gridHeight: 10);
//     ```
//   - Grid-Größe im Konstruktor:
//     ```csharp
//     var rover = new Rover(new Point(5, 9), Direction.N, gridWidth: 10, gridHeight: 10);
//     ```
//   - Eigene `Grid`-Klasse für die Topologie:
//     ```csharp
//     var grid = new Grid(width: 10, height: 10);
//     var rover = new Rover(grid, new Point(5, 9), Direction.N);
//     ```

// %% [markdown]
//
// #### GRÜN & REFACTOR:
//
// - Test deckt Designproblem auf: Rover kennt zu viele Welt-Regeln
// - Designverbesserung:
//   - `Grid`-Klasse für Grid-Logik erstellen
//   - Rover delegiert Bewegungsberechnung an `Grid`
// - Ergebnis: entkoppeltes Design, geboren aus der Notwendigkeit eines
//   Tests

// %% [markdown]
//
// - Test ist jetzt grün
// - Grid-Instanz wird nicht modifiziert => `[OneTimeSetUp]` möglich
// - Hier nicht nötig (Grid ist billig), aber gute Praxis für komplexere
//   Objekte
