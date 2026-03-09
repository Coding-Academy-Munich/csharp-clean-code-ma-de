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
// ```csharp
// [TestFixture]
// public class MarsRoverTests
// {
//     [Test]
//     public void Rover_Initializes_To_Zero_Zero_Facing_North()
//     {
//         var rover = new Rover();
//
//         Assert.That(rover.Position, Is.EqualTo(new Point(0, 0)));
//         Assert.That(rover.Direction, Is.EqualTo(Direction.N));
//     }
// }
// ```

// %% [markdown]
//
// #### GRUEN:
//
// - `Rover`-Klasse, `Point` (record struct) und `Direction` (enum) erstellen
// - Test besteht

// %% [markdown]
//
// ```csharp
// namespace MarsRover;
//
// public record struct Point(int X, int Y);
//
// public enum Direction { N, E, S, W }
//
// public class Rover
// {
//     public Point Position { get; private set; }
//     public Direction Direction { get; private set; }
//
//     public Rover()
//     {
//         Position = new Point(0, 0);
//         Direction = Direction.N;
//     }
// }
// ```

// %% [markdown]
//
// ### 2. Zyklus: Verhalten hinzufügen - Drehen nach rechts
//
// #### ROT:
// - Test: Befehl `R` an nach `N` ausgerichteten Rover => Richtung `E`
// - Schlägt fehl -- keine `ExecuteCommands`-Methode vorhanden

// %% [markdown]
//
// ```csharp
// [Test]
// public void Rover_Turning_Right_Once_Changes_Direction_To_East()
// {
//     var rover = new Rover();
//     rover.ExecuteCommands("R");
//     Assert.That(rover.Direction, Is.EqualTo(Direction.E));
// }
// ```

// %% [markdown]
//
// #### GRUEN:
//
// - `ExecuteCommands`-Methode mit einfacher Logik implementieren

// %% [markdown]
//
// ```csharp
// public void ExecuteCommands(string commands)
// {
//     if (command == "R")
//     {
//         Direction = Direction.E;
//     }
// }
// ```

// %% [markdown]
//
// ### 3. Zyklus: Zweimal nach rechts drehen
//
// #### ROT:
// - Test: Befehl `RR` an nach `N` ausgerichteten Rover => Richtung `S`
// - Schlägt fehl

// %% [markdown]
//
// ```csharp
// [Test]
// public void Rover_Turning_Right_Twice_Changes_Direction_To_South()
// {
//     var rover = new Rover();
//     rover.ExecuteCommands("RR");
//     Assert.That(rover.Direction, Is.EqualTo(Direction.S));
// }
// ```

// %% [markdown]
//
// #### GRUEN:
// - `ExecuteCommands` um `"RR"`-Fall erweitern

// %% [markdown]
//
// ```csharp
// public void ExecuteCommands(string commands)
// {
//     if (command == "R")
//     {
//         Direction = Direction.E;
//     }
//     else if (command == "RR")
//     {
//         Direction = Direction.S;
//     }
// }
// ```

// %% [markdown]
//
// #### REFACTOR (1):
//
// - Ad-hoc-Logik durch Schleife ersetzen
// - Jeden Befehl einzeln als `char` verarbeiten

// %% [markdown]
//
// ```csharp
// public void ExecuteCommands(string commands)
// {
//     foreach (char command in commands)
//     {
//         if (command == 'R')
//         {
//             Direction = Direction switch
//             {
//                 Direction.N => Direction.E,
//                 Direction.E => Direction.S,
//                 _ => Direction
//             };
//         }
//     }
// }
// ```

// %% [markdown]
//
// #### REFACTOR (2):
//
// - Drehlogik in eigene Methode `TurnRight` extrahieren
// - Rover in `[SetUp]`-Methode erzeugen (Member-Variable)
// - Zwei Einzeltests durch parametrisierten Test ersetzen

// %% [markdown]
//
// ```csharp
// private void TurnRight()
// {
//     Direction = Direction switch
//     {
//         Direction.N => Direction.E,
//         Direction.E => Direction.S,
//         _ => Direction
//     };
// }
//
// public void ExecuteCommands(string commands)
// {
//     foreach (char command in commands)
//     {
//         if (command == 'R')
//         {
//             TurnRight();
//         }
//     }
// }
// ```

// %% [markdown]
//
// ```csharp
// [TestFixture]
// public class MarsRoverTests
// {
//     private Rover _rover;
//
//     [SetUp]
//     public void SetUp()
//     {
//         _rover = new Rover(new Point(0, 0), Direction.N);
//     }
//
//     // ...
//
//     [TestCase("R", Direction.E)]
//     [TestCase("RR", Direction.S)]
//     public void Rover_Turning_Right_Changes_Direction_Correctly(string commands, Direction expectedDirection)
//     {
//         _rover.ExecuteCommands(commands);
//         Assert.That(_rover.Direction, Is.EqualTo(expectedDirection));
//     }
// }
// ```

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
// ```csharp
// [TestCase("R", Direction.E)]
// [TestCase("RR", Direction.S)]
// [TestCase("RRR", Direction.W)]
// [TestCase("RRRR", Direction.N)]
// public void Rover_Turning_Right_Changes_Direction_Correctly(string commands, Direction expectedDirection)
// {
//     _rover.ExecuteCommands(commands);
//     Assert.That(_rover.Direction, Is.EqualTo(expectedDirection));
// }
// ```

// %% [markdown]
//
// #### GRUEN & REFACTOR:
//
// - `TurnRight` um alle vier Richtungen erweitern
// - Parametrisierter Test ist jetzt grün

// %% [markdown]
//
// ```csharp
// private void TurnRight()
// {
//     Direction = Direction switch
//     {
//         Direction.N => Direction.E,
//         Direction.E => Direction.S,
//         Direction.S => Direction.W,
//         Direction.W => Direction.N,
//         _ => Direction
//     };
// }
// ```

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
// ```csharp
// [TestCase("R", Direction.E)]
// [TestCase("RR", Direction.S)]
// [TestCase("RRR", Direction.W)]
// [TestCase("RRRR", Direction.N)]
// [TestCase("L", Direction.W)]
// [TestCase("LL", Direction.S)]
// [TestCase("LLL", Direction.E)]
// [TestCase("LLLL", Direction.N)]
// public void Rover_Turning_Changes_Direction_Correctly(string commands, Direction expectedDirection)
// {
//     _rover.ExecuteCommands(commands);
//     Assert.That(_rover.Direction, Is.EqualTo(expectedDirection));
// }
// ```

// %% [markdown]
//
// #### GRUEN & REFACTOR:
//
// - `TurnLeft`-Methode implementieren
// - `ExecuteCommands` um `L`-Befehl erweitern
// - Parametrisierter Test ist jetzt grün

// %% [markdown]
//
// ```csharp
// private void TurnLeft()
// {
//     Direction = Direction switch
//     {
//         Direction.N => Direction.W,
//         Direction.W => Direction.S,
//         Direction.S => Direction.E,
//         Direction.E => Direction.N,
//         _ => Direction
//     };
// }
//
// public void ExecuteCommands(string commands)
// {
//     foreach (char command in commands)
//     {
//         if (command == 'R')
//         {
//             TurnRight();
//         }
//         else if (command == 'L')
//         {
//             TurnLeft();
//         }
//     }
// }
// ```

// %% [markdown]
//
// ### 6. Zyklus: Bewegen
//
// #### ROT:
// - Test: `M` an Rover bei `(10, 10)`, Richtung `N` => Position `(10, 11)`
// - Schlägt fehl

// %% [markdown]
//
// ```csharp
// [Test]
// public void Rover_Moves_Forward_Facing_North()
// {
//     var rover = new Rover(new Point(10, 10), Direction.N);
//     rover.ExecuteCommands("M");
//     Assert.That(rover.Position, Is.EqualTo(new Point(10, 11)));
// }
// ```

// %% [markdown]
//
// #### GRUEN & REFACTOR:
//
// - Bewegungslogik für Richtung `N` implementieren
// - Test ist jetzt grün

// %% [markdown]
//
// ```csharp
// private void Move()
// {
//     Position = Direction switch
//     {
//         Direction.N => new Point(Position.X, Position.Y + 1),
//         _ => Position
//     };
// }
//
// public void ExecuteCommands(string commands)
// {
//     foreach (char command in commands)
//     {
//         if (command == 'R')
//         {
//             TurnRight();
//         }
//         else if (command == 'L')
//         {
//             TurnLeft();
//         }
//         else if (command == 'M')
//         {
//             Move();
//         }
//     }
// }
// ```

// %% [markdown]
//
// ### 7. Zyklus: Bewegen in alle Richtungen
//
// #### ROT:
// - Parametrisierten Test für alle vier Bewegungsrichtungen erstellen
// - Schlägt fehl

// %% [markdown]
//
// ```csharp
// [TestCase(Direction.N, 10, 11)]
// [TestCase(Direction.S, 10, 9)]
// [TestCase(Direction.E, 11, 10)]
// [TestCase(Direction.W, 9, 10)]
// public void Rover_Moves_Forward_One_Grid_Point_In_Correct_Direction(
//     Direction direction, int expectedX, int expectedY)
// {
//     var rover = new Rover(new Point(10, 10), direction);
//     var expectedPosition = new Point(expectedX, expectedY);
//     rover.ExecuteCommands("M");
//     Assert.That(rover.Position, Is.EqualTo(expectedPosition));
//     Assert.That(rover.Direction, Is.EqualTo(direction));
// }
// ```

// %% [markdown]
//
// #### GRUEN & REFACTOR:
//
// - `Move`-Methode um alle vier Richtungen erweitern
// - Parametrisierter Test ist jetzt grün

// %% [markdown]
//
// ```csharp
// private void Move()
// {
//     Position = Direction switch
//     {
//         Direction.N => new Point(Position.X, Position.Y + 1),
//         Direction.E => new Point(Position.X + 1, Position.Y),
//         Direction.S => new Point(Position.X, Position.Y - 1),
//         Direction.W => new Point(Position.X - 1, Position.Y),
//         _ => Position
//     };
// }
// ```

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
// ```csharp
// [Test]
// public void Rover_Wraps_Around_The_Grid_Edge()
// {
//     var rover = new Rover(new Point(5, 9), Direction.N);
//     rover.ExecuteCommands("M");
//     Assert.That(rover.Position, Is.EqualTo(new Point(5, 0)));
// }
// ```

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
// #### GRUEN & REFACTOR:
//
// - Test deckt Designproblem auf: Rover kennt zu viele Welt-Regeln
// - Designverbesserung:
//   - `Grid`-Klasse für Grid-Logik erstellen
//   - Rover delegiert Bewegungsberechnung an `Grid`
// - Ergebnis: entkoppeltes Design, geboren aus der Notwendigkeit eines
//   Tests

// %% [markdown]
//
// ```csharp
// public class Grid
// {
//     public int Width { get; }
//     public int Height { get; }
//
//     public Grid(int width, int height)
//     {
//         if (width <= 0 || height <= 0)
//         {
//             throw new ArgumentException("Grid dimensions must be positive.");
//         }
//         Width = width;
//         Height = height;
//     }
//
//     public Point CalculateNextPosition(Point current, Direction dir)
//     {
//         return dir switch
//         {
//             Direction.N => new Point(current.X, (current.Y + 1) % Height),
//             Direction.E => new Point((current.X + 1) % Width, current.Y),
//             Direction.S => new Point(current.X, (current.Y - 1 + Height) % Height),
//             Direction.W => new Point((current.X - 1 + Width) % Width, current.Y),
//             _ => current
//         };
//     }
// }
// ```

// %% [markdown]
//
// ```csharp
// public class Rover
// {
//     private readonly Grid _grid;
//
//     public Rover(Grid grid, Point startPosition = new(), Direction startDirection = Direction.N)
//     {
//         _grid = grid;
//         Position = startPosition;
//         Direction = startDirection;
//     }
//
//     private void Move()
//     {
//         Position = _grid.CalculateNextPosition(Position, Direction);
//     }
//     // ... rest of the Rover code
// }
// ```

// %% [markdown]
//
// - Test ist jetzt grün
// - Grid-Instanz wird nicht modifiziert => `[OneTimeSetUp]` möglich
// - Hier nicht nötig (Grid ist billig), aber gute Praxis für komplexere
//   Objekte

// %% [markdown]
//
// ```csharp
// [TestFixture]
// public class MarsRoverTests
// {
//     private Grid _grid;
//     private Rover _rover;
//
//     [OneTimeSetUp]
//     public void OneTimeSetUp()
//     {
//         _grid = new Grid(100, 100);
//     }
//
//     [SetUp]
//     public void SetUp()
//     {
//         _rover = new Rover(_grid, new Point(0, 0), Direction.N);
//     }
//     // ... rest of the tests
// }
// ```
