using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShooter;

// Basklass för alla spelobjektet som har en sprite och en position
class GameObject
{
    // Definiera medlemsvariabler
    protected Texture2D texture;
    protected Vector2 vector;

    // GameObject() - konstruktor
    public GameObject(Texture2D texture, float x, float y)
    {
        this.texture = texture;
        vector.X = x;
        vector.Y = y;
    }

    // Rita objekt på skärmen i Draw()
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, vector, Color.White);
    }

    // Definiera properties
    public float X { get => vector.X; set => vector.X = value; }
    public float Y { get => vector.Y; set => vector.Y = value; }
    public float Width => texture.Width;
    public float Height => texture.Height;
}
