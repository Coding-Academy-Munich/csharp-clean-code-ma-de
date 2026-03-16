namespace MarsRoverNUnitSkTest;

using MarsRoverNUnitSk;

[TestFixture]
public class MarsRoverNUnitSkTests
{
    [Test]
    public void Rover_InitializesToZero_FacingNorth()
    {
        var rover = new Rover();

        Assert.That(rover.Position, Is.EqualTo(new Point(0, 0)));
        Assert.That(rover.Direction, Is.EqualTo(Direction.N));
    }
}
