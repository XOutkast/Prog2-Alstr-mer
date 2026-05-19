using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter;

// Tripod: fiende som rör sig rakt nedåt
internal class Tripod : Enemy
{
    // Tripod() konstruktor  - positoin & hastighet
    public Tripod(Texture2D texture, float x, float y)
        : base(texture, x, y, 0f, 3f)
    {
    }

    // Uppdatera position & tar bort när den lämnar skärmen
    public override void Update(GameWindow window)
    {
        vector.Y += speed.Y;
        if (vector.Y > window.ClientBounds.Height) isAlive = false;
    }
}