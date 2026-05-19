using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter;

//  Fiende med diagonal rörelse och studsar sidknaterna
class StoneEnemy : Enemy
{
    // StoneEnemy() konstruktor
	// Position & hastighet
    public StoneEnemy(Texture2D texture, float x, float y)
        : base(texture, x, y, 2.5f, 2.2f)
    {
    }

    // Flytta fienden diagonalt, studsa mot sidorna och ta bort när den lämnar skärmen
    public override void Update(GameWindow window)
    {
        vector.X += speed.X;
        vector.Y += speed.Y;

        if (vector.X < 0 || vector.X > window.ClientBounds.Width - texture.Width)
            speed.X = -speed.X;

        if (vector.Y > window.ClientBounds.Height)
            isAlive = false;
    }
}
