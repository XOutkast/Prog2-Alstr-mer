using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter;

// Klass Bullet för spelarens skott
internal class Bullet : PhysicalObject
{
    // Konstruktor för Bullet()
    // Sätt startposition och ge skottet en konstant hastighet uppåt
    public Bullet(Texture2D texture, float x, float y)
        : base(texture, x, y, 0f, 7f)
    {
    }

    // Flytta skottet uppåt och markera det som dött om det lämnar skärmen
    public void Update()
    {
        vector.Y -= speed.Y;
        if (vector.Y < 0) isAlive = false;
    }
}