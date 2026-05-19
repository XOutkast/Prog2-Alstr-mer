// Riktningsvektorer för rörelse i spelrutnätet.
namespace Snake.Core;

public readonly record struct Direction(int X, int Y)
{
    public static readonly Direction Up = new(0, -1);
    public static readonly Direction Down = new(0, 1);
    public static readonly Direction Left = new(-1, 0);
    public static readonly Direction Right = new(1, 0);

    public static Direction Opposite(Direction direction) => new(-direction.X, -direction.Y);
}
