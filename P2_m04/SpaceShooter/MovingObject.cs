using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter;

// Skapa klassen MovingObject
// Basklass för objekt med hastighet
class MovingObject : GameObject
{
    // Definiera hastighet i X- och Y-led
    protected Vector2 speed;

    // Konstruktor för MovingObject
	// Sätt startposition & initial hastighet
    public MovingObject(Texture2D texture, float x, float y, float speedX, float speedY)
        : base(texture, x, y)
    {
        speed.X = speedX;
        speed.Y = speedY;
    }
}
