using Microsoft.Xna.Framework;
using Snake;

namespace Snake.Models;

// SpikeTrap: döda ormen vid kollision.
public class SpikeTrap : Hazard
{
    public SpikeTrap(Point position)
        : base(position, "Spike Trap", Color.OrangeRed)
    {
    }

    public override void Update(Game1 game)
    {
    }

    public override void ResolveCollision(Game1 game)
    {
        game.EndSnakeRun();
    }
}
