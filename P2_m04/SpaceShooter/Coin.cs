using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter;

// Klass Coin
// Ge poäng vid plockning
internal class Coin : PhysicalObject
{
    // Konstruktor för Coin()
    // Startposition & ge myntet en nedåtriktad hastighet
    public Coin(Texture2D texture, float x, float y)
        : base(texture, x, y, 0f, 1.8f)
    {
    }

    // Flytta myntet nedåt och markera det som dött om det lämnar skärmen
    public void Update(GameWindow window)
    {
        vector.Y += speed.Y;
        if (vector.Y > window.ClientBounds.Height) isAlive = false;
    }
}