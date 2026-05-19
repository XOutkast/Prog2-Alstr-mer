using Microsoft.Xna.Framework;
using Snake;

namespace Snake.Models;

// Basklass för collectible & lagra position, namn, poäng och effekt.
public abstract class Collectible
{
    // Konstruktor: init alla värde
    protected Collectible(Point position, string name, Color tint, int points, int growth)
    {
        Position = position;
        Name = name;
        Tint = tint;
        Points = points;
        Growth = growth;
    }

    public Point Position { get; }
    public string Name { get; }
    public Color Tint { get; }
    public int Points { get; }
    public int Growth { get; }

    // Abstrakt:låt varje collectible vara sin egen
    public abstract void Apply(Game1 game);
}
