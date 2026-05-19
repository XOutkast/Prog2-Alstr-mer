using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter;

// Klassen PhysicalObject
// Basklass för objektet som kan kollidera & och har "leva/dö" status
abstract class PhysicalObject : MovingObject
{
    // Om objektet är aktivt eller inte
    protected bool isAlive = true;

    // PhysicalObject() konstruktor
    // Sätt startposition och hastighet
    public PhysicalObject(Texture2D texture, float x, float y, float speedX, float speedY)
        : base(texture, x, y, speedX, speedY)
    {
    }

    // Kontrollera kollision
    public bool CheckCollision(PhysicalObject other)
    {
        Rectangle myRect = new(
            (int)X,
            (int)Y,
            (int)Width,
            (int)Height);

        Rectangle otherRect = new(
            (int)other.X,
            (int)other.Y,
            (int)other.Width,
            (int)other.Height);

        return myRect.Intersects(otherRect);
    }

    // Ge åtkomst till livstatus
    public bool IsAlive { get => isAlive; set => isAlive = value; }
}
