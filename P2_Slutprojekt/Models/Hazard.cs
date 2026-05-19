using Microsoft.Xna.Framework;
using Snake;

namespace Snake.Models;

// Hazards & lagra position, utseende och beteende.
public abstract class Hazard
{
    protected Hazard(Point position, string name, Color tint)
    {
        Position = position;
        Name = name;
        Tint = tint;
    }

    public Point Position { get; protected set; }
    public string Name { get; }
    public Color Tint { get; }

    public abstract void Update(Game1 game);
    public abstract void ResolveCollision(Game1 game);
}
