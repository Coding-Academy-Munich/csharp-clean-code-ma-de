namespace MarsRoverNUnitSk;

public record struct Point(int X, int Y);

public enum Direction { N, E, S, W }

public class Rover
{
    public Point Position { get; private set; }
    public Direction Direction { get; private set; }

    public Rover()
    {
        Position = new Point(0, 0);
        Direction = Direction.N;
    }
}
