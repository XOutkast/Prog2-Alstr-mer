using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter;

// Klass: Player för spelarens rymdskepp
// Hantera spelarens rörelse, skott, rymskep och poäng
internal class Player : PhysicalObject
{
    // Medlemsvariabler
    private readonly Texture2D bulletTexture;
    private double timeSinceLastBullet;

    // Player() konstruktor för spelarobjektet
    // Sätt startposition, hastighet & skott
    public Player(Texture2D texture, float x, float y, float speedX, float speedY, Texture2D bulletTexture)
        : base(texture, x, y, speedX, speedY)
    {
        Bullets = new List<Bullet>();
        this.bulletTexture = bulletTexture;
        Points = 0;
    }

    // Properties
    public List<Bullet> Bullets { get; }

    public int Points { get; set; }

    // Hantera input, uppdatera position och skjut skott
    public void Update(GameWindow window, GameTime gameTime)
    {
        if (!isAlive) return;

        // Gör rörelsen oberoende av FPS (frameScale)
        var frameScale = (float)(gameTime.ElapsedGameTime.TotalSeconds * 60.0);

        var keyboardState = Keyboard.GetState();

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
            if (gameTime.TotalGameTime.TotalMilliseconds - timeSinceLastBullet > 200)
            {
                var temp = new Bullet(
                    bulletTexture,
                    vector.X + texture.Width / 2f - bulletTexture.Width / 2f,
                    vector.Y);
                Bullets.Add(temp);
                timeSinceLastBullet = gameTime.TotalGameTime.TotalMilliseconds;
            }

        // Uppdatera alla skott och ta bort de som är döda
        foreach (var b in Bullets.ToList())
        {
            b.Update();
            if (!b.IsAlive)
                Bullets.Remove(b);
        }
    }

    // Rita spelaren och alla aktiva skott i Draw()
    public override void Draw(SpriteBatch spriteBatch)
    {
        if (isAlive) base.Draw(spriteBatch);

        foreach (var b in Bullets)
            b.Draw(spriteBatch);
    }

    // Återställ spelaren för en ny spelrunda i Reset()
    public void Reset(float x, float y, float speedX, float speedY)
    {
        vector.X = x;
        vector.Y = y;
        speed.X = speedX;
        speed.Y = speedY;
        Bullets.Clear();
        timeSinceLastBullet = 0;
        Points = 0;
        isAlive = true;
    }
}