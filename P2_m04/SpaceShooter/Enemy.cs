using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter;

// Enemy klass
// Abstrakt basklass för fiender
internal abstract class Enemy : PhysicalObject
{
    // Skapa Enemy() med konstruktor för fiendeklasser
    public Enemy(Texture2D texture, float x, float y, float speedX, float speedY)
        : base(texture, x, y, speedX, speedY)
    {
    }

    // Implementera Update() i respektive fiendetyp
    public abstract void Update(GameWindow window);
}