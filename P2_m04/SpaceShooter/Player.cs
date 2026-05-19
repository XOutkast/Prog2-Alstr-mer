using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter;

// Klass: Player för spelarens rymdskepp
// Hantera spelarens rörelse, skott, rymskep och poäng
class Player : PhysicalObject
{
    // Medlemsvariabler
    readonly List<Bullet> bullets;
    readonly Texture2D bulletTexture;
    double timeSinceLastBullet;
    int points;

    // Player() konstruktor för spelarobjektet
	// Sätt startposition, hastighet & skott
    public Player(Texture2D texture, float x, float y, float speedX, float speedY, Texture2D bulletTexture)
        : base(texture, x, y, speedX, speedY)
    {
        bullets = new List<Bullet>();
        this.bulletTexture = bulletTexture;
        points = 0;
    }

    // Hantera input, uppdatera position och skjut skott
    public void Update(GameWindow window, GameTime gameTime)
    {
        if (!isAlive)
        {
            return;
        }

        // Gör rörelsen oberoende av FPS (frameScale)
        float frameScale = (float)(gameTime.ElapsedGameTime.TotalSeconds * 60.0);

        KeyboardState keyboardState = Keyboard.GetState();

        // Flytta spelaren beroende på tangentbordet
        if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            vector.X += speed.X * frameScale;
        if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            vector.X -= speed.X * frameScale;
        if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            vector.Y += speed.Y * frameScale;
        if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            vector.Y -= speed.Y * frameScale;

		// Håll spelaren inom skärmen
        if (vector.X < 0)
            vector.X = 0;
        if (vector.X > window.ClientBounds.Width - texture.Width)
            vector.X = window.ClientBounds.Width - texture.Width;
        if (vector.Y < 0)
            vector.Y = 0;
        if (vector.Y > window.ClientBounds.Height - texture.Height)
            vector.Y = window.ClientBounds.Height - texture.Height;

		// Skjuta skott med mellanslag
        if (keyboardState.IsKeyDown(Keys.Space))
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - timeSinceLastBullet > 200)
            {
                Bullet temp = new Bullet(
                    bulletTexture,
                    vector.X + texture.Width / 2f - bulletTexture.Width / 2f,
                    vector.Y);
                bullets.Add(temp);
                timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }

		// Uppdatera alla skott och ta bort de som är döda
        foreach (Bullet b in bullets.ToList())
        {
            b.Update();
            if (!b.IsAlive)
                bullets.Remove(b);
        }
    }

    // Rita spelaren och alla aktiva skott i Draw()
    public override void Draw(SpriteBatch spriteBatch)
    {
        if (isAlive)
        {
            base.Draw(spriteBatch);
        }

        foreach (Bullet b in bullets)
            b.Draw(spriteBatch);
    }

    // Återställ spelaren för en ny spelrunda i Reset()
    public void Reset(float x, float y, float speedX, float speedY)
    {
        vector.X = x;
        vector.Y = y;
        speed.X = speedX;
        speed.Y = speedY;
        bullets.Clear();
        timeSinceLastBullet = 0;
        points = 0;
        isAlive = true;
    }

    // Properties
    public List<Bullet> Bullets => bullets;
    public int Points { get => points; set => points = value; }
}
