using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter;

// Klass mine: fiende med sidledsrörelse och nedåtfart
internal class Mine : Enemy
{
    // Skapa Mine() - konstruktor
    public Mine(Texture2D texture, float x, float y)
        : base(texture, x, y, 2f, 1f)
    {
    }

    // Uppdatera fiendens position i Update()
    public override void Update(GameWindow window)
    {
        vector.X += speed.X;
        if (vector.X > window.ClientBounds.Width - texture.Width || vector.X < 0) speed.X = -speed.X;

        vector.Y += speed.Y;
        if (vector.Y > window.ClientBounds.Height) isAlive = false;
    }
}