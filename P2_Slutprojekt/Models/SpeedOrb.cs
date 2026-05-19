using Microsoft.Xna.Framework;
using Snake;

namespace Snake.Models;

public class SpeedOrb : Collectible
{
    public SpeedOrb(Point position)
        : base(position, "Speed Orb", Color.FloralWhite, 15, 0)
    {
    }

    public override void Apply(Game1 game)
    {
        game.AddScore(Points);
    }
}
