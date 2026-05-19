using Microsoft.Xna.Framework;
using Snake;

namespace Snake.Models;

public class Apple : Collectible
{
    public Apple(Point position)
        : base(position, "Apple", Color.SeaGreen, 10, 1)
    {
    }

    public override void Apply(Game1 game)
    {
        game.AddScore(Points); // ge pöang
        game.GrowSnake(Growth); // growth
    }
}
