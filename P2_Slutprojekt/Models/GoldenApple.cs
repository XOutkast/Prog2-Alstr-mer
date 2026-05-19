using Microsoft.Xna.Framework;
using Snake;

namespace Snake.Models;

// GoldenApple: ge extra poäng och växt.
public class GoldenApple : Collectible
{
    public GoldenApple(Point position)
        : base(position, "Golden Apple", Color.Gold, 25, 2)
    {
    }

    public override void Apply(Game1 game)
    {
        game.AddScore(Points);
        game.GrowSnake(Growth);
    }
    }
