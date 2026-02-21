// ReSharper disable RedundantArgumentDefaultValue

namespace MarsRoverNunitTest;

using MarsRoverNunit;

[TestFixture]
public class MarsRoverTests
{
    private Grid _grid;
    private Rover _rover;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _grid = new Grid(100, 100);
    }

    [SetUp]
    public void SetUp()
    {
        _rover = new Rover(_grid, new Point(0, 0), Direction.N);
    }

    // Test 1: Initial state
    [Test]
    public void Rover_Initializes_To_Zero_Zero_Facing_North()
    {
        // For this specific test, we want to test the rover that is instantiated by default, so we create a fresh one.
        var newRover = new Rover(_grid);

        Assert.That(newRover.Position, Is.EqualTo(new Point(0, 0)));
        Assert.That(newRover.Direction, Is.EqualTo(Direction.N));
    }

    // Test 2: Turning
    // Initial test for turning.
    [Test]
    public void Rover_Turning_Right_Once_Changes_Direction_To_East()
    {
        _rover.ExecuteCommands("R");
        Assert.That(_rover.Direction, Is.EqualTo(Direction.E));
        Assert.That(_rover.Position, Is.EqualTo(new Point(0, 0)));
    }

    // Second test for turning. The structure is almost similar to the first test for turning.
    [Test]
    public void Rover_Turning_Right_Twice_Changes_Direction_To_South()
    {
        _rover.ExecuteCommands("RR");
        Assert.That(_rover.Direction, Is.EqualTo(Direction.S));
        Assert.That(_rover.Position, Is.EqualTo(new Point(0, 0)));
    }

    // Don't write tests like this!
    [Test]
    public void Rover_Turning_Right_Cycles_Through_Directions_Correctly()
    {
        _rover.ExecuteCommands("R");
        Assert.That(_rover.Direction, Is.EqualTo(Direction.E));
        Assert.That(_rover.Position, Is.EqualTo(new Point(0, 0)));
        _rover.ExecuteCommands("R");
        Assert.That(_rover.Direction, Is.EqualTo(Direction.S));
        Assert.That(_rover.Position, Is.EqualTo(new Point(0, 0)));
        _rover.ExecuteCommands("R");
        Assert.That(_rover.Direction, Is.EqualTo(Direction.W));
        Assert.That(_rover.Position, Is.EqualTo(new Point(0, 0)));
        _rover.ExecuteCommands("R");
        Assert.That(_rover.Direction, Is.EqualTo(Direction.N));
        Assert.That(_rover.Position, Is.EqualTo(new Point(0, 0)));
    }

    // If we have many similar tests, we can use a parametric test (TestCase) to reduce the amount of boilerplate code
    // we have to write.
    // To turn this into a parametric test, we need to be able to run multiple commands from a string.
    [TestCase("R", Direction.E)]
    [TestCase("RR", Direction.S)]
    [TestCase("RRR", Direction.W)]
    [TestCase("RRRR", Direction.N)]
    [TestCase("L", Direction.W)]
    [TestCase("LL", Direction.S)]
    [TestCase("LLL", Direction.E)]
    [TestCase("LLLL", Direction.N)]
    public void Rover_Turning_Changes_Direction_Correctly(string commands, Direction expectedDirection)
    {
        _rover.ExecuteCommands(commands);
        Assert.That(_rover.Direction, Is.EqualTo(expectedDirection));
        Assert.That(_rover.Position, Is.EqualTo(new Point(0, 0)));
    }

    // Test 3: Moving
    // Initial test for moving.
    [Test]
    public void Rover_Moves_Forward_Facing_North()
    {
        var rover = new Rover(_grid, new Point(10, 10), Direction.N);
        rover.ExecuteCommands("M");
        Assert.That(rover.Position, Is.EqualTo(new Point(10, 11)));
        Assert.That(rover.Direction, Is.EqualTo(Direction.N));
    }

    [TestCase(Direction.N, 10, 11)]
    [TestCase(Direction.S, 10, 9)]
    [TestCase(Direction.E, 11, 10)]
    [TestCase(Direction.W, 9, 10)]
    public void Rover_Moves_Forward_One_Grid_Point_In_Correct_Direction(
        Direction direction, int expectedX, int expectedY)
    {
        // This test requires a specific starting position, so it instantiates its own Rover.
        var rover = new Rover(_grid, new Point(10, 10), direction);
        var expectedPosition = new Point(expectedX, expectedY);

        rover.ExecuteCommands("M");

        Assert.That(rover.Position, Is.EqualTo(expectedPosition));
        Assert.That(rover.Direction, Is.EqualTo(direction));
    }

    // Test 4: Command Sequence
    // This test is unnecessary, since we already use command sequences in the test for turning.
    // It would, however, be a good test if we had not used command sequences previously.
    // It might still be useful to check that we can execute sequences containing a mix of commands.
    [Test]
    public void Rover_Can_Execute_A_Sequence_Of_Commands()
    {
        // This test requires a specific starting position, so it instantiates its own Rover.
        var rover = new Rover(_grid, new Point(5, 5), Direction.N);

        rover.ExecuteCommands("RMMMLM");

        Assert.That(rover.Position, Is.EqualTo(new Point(8, 6)));
        Assert.That(rover.Direction, Is.EqualTo(Direction.N));
    }

    // Test 5: The "Design Driver" test
    [TestCase(5, 9, Direction.N, 5, 0)]
    [TestCase(5, 0, Direction.S, 5, 9)]
    [TestCase(9, 5, Direction.E, 0, 5)]
    [TestCase(0, 5, Direction.W, 9, 5)]
    public void Rover_Wraps_Around_The_Grid_Edges(
        int startX, int startY, Direction direction, int expectedX, int expectedY)
    {
        // For this test, a specific 10x10 grid is needed, which is different from the fixture's 100x100 grid.
        // Therefore, we still instantiate a new Grid and Rover here.
        var grid = new Grid(10, 10);
        var rover = new Rover(grid, new Point(startX, startY), direction);

        rover.ExecuteCommands("M");

        Assert.That(rover.Position, Is.EqualTo(new Point(expectedX, expectedY)));
        Assert.That(rover.Direction, Is.EqualTo(direction));
    }
}
