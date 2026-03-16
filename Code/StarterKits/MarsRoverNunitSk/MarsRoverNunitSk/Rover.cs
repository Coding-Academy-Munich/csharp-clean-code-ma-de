namespace MarsRoverNUnitSk;

public record struct Point(int X, int Y);

public enum Direction { N, E, S, W }

public class Rover
{
    public Point Position { get; private set; } = new(0, 0);
    public Direction Direction { get; private set; } = Direction.N;

    public void ExecuteCommands(string commands)
    {
        if (commands == "R")
        {
            Direction = Direction.E;
        }
    }
}
