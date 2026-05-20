using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShooter;

// Game1: huvudklass för spelet
// Hantera gamestates, uppdatering, ritning, poäng och highscoreflöde
public class Game1 : Game
{
    // Poängregler
    private const int PointsPerKill = 1;
    private const int PointsPerCoin = 2;
    private const double EnemySpawnIntervalMs = 1200;
    private const double CoinSpawnIntervalMs = 1800; // för varje 1800 ms (1.8s)

    // Grafik
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Bakgrund och UI
    private float backgroundScroll;
    private Texture2D backgroundTexture;
    private Texture2D bulletTexture;
    private List<Coin> coins;
    private int coinsCollected;
    private Texture2D coinTexture;

    // State-hantering
    private State currentState;
    private List<Enemy> enemies;
    private int enemiesShot;

    // Highscore
    private HighScore highScore;
    private string highScoreFilePath;
    private Texture2D menuExitTexture;
    private Texture2D menuHighScoreTexture;
    private Texture2D menuStartTexture;
    private Texture2D menuTexture;
    private Texture2D mineTexture;

    // Spelobjekt
    private Player player;

    // Texturer och font
    private Texture2D playerTexture;
    private KeyboardState previousKeyboardState;
    private Random random;
    private int selectedMenuItem;
    private Texture2D stoneTexture;
    private double timeSinceLastCoinSpawn;
    private double timeSinceLastEnemySpawn;
    private Texture2D tripodTexture;
    private SpriteFont uiFont;

    // konstruktor för Game1()
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    // Initiera state och variabler 
    protected override void Initialize()
    {
        enemies = new List<Enemy>();
        coins = new List<Coin>();
        random = new Random();
        selectedMenuItem = 0;
        currentState = State.Menu;
        highScoreFilePath = Path.Combine(AppContext.BaseDirectory, "highscore.txt"); // filen som HighScore använder
        enemiesShot = 0;
        coinsCollected = 0;
        timeSinceLastEnemySpawn = 0;
        timeSinceLastCoinSpawn = 0;

        base.Initialize();
    }

    // Ladda sprites, font och highscoredata
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        playerTexture = Content.Load<Texture2D>("ship");
        bulletTexture = Content.Load<Texture2D>("bullet");
        coinTexture = Content.Load<Texture2D>("coin");
        mineTexture = Content.Load<Texture2D>("mine");
        tripodTexture = Content.Load<Texture2D>("tripod");
        stoneTexture = Content.Load<Texture2D>("stone");
        menuTexture = Content.Load<Texture2D>("menu");
        menuStartTexture = Content.Load<Texture2D>("menu/start");
        menuHighScoreTexture = Content.Load<Texture2D>("menu/highscore");
        menuExitTexture = Content.Load<Texture2D>("menu/exit");
        backgroundTexture = Content.Load<Texture2D>("background");
        uiFont = Content.Load<SpriteFont>("MyFont");

        backgroundScroll = 0;

        // Skapa spelaren
        player = new Player(
            playerTexture,
            Window.ClientBounds.Width / 2f,
            Window.ClientBounds.Height - 120,
            5f,
            5f,
            bulletTexture);

        // Ladda HighScore
        highScore = new HighScore(10);
        highScore.LoadFromFile(highScoreFilePath);

        SpawnEnemies();
    }

    // Uppdatera gameloopen beroende på state
    protected override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        // ESC- återgå till meny
        if (IsNewKeyPress(keyboardState, Keys.Escape) && currentState == State.Run) currentState = State.Menu;

        switch (currentState)
        {
            case State.Menu:
                MenuUpdate(keyboardState, gameTime);
                break;

            case State.Run:
                RunUpdate(gameTime);
                break;

            case State.EnterScore:
                EnterScoreUpdate(gameTime);
                break;

            case State.HighScore:
                HighScoreUpdate(keyboardState, gameTime);
                break;

            case State.About:
                AboutUpdate(keyboardState, gameTime);
                break;

            case State.HowToPlay:
                HowToPlayUpdate(keyboardState, gameTime);
                break;

            case State.Quit:
                Exit();
                break;
        }

        previousKeyboardState = keyboardState;

        Window.Title = $"SpaceShooter - State: {currentState}";

        base.Update(gameTime);
    }

    // Rita gameloopen beroende på state
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointWrap,
            DepthStencilState.None,
            RasterizerState.CullNone);

        DrawBackground();

        switch (currentState)
        {
            case State.Menu:
                MenuDraw();
                break;

            case State.Run:
                RunDraw();
                break;

            case State.EnterScore:
                EnterScoreDraw();
                break;

            case State.HighScore:
                HighScoreDraw();
                break;

            case State.About:
                AboutDraw();
                break;

            case State.HowToPlay:
                HowToPlayDraw();
                break;
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    // Tangentval i menyn (menyinput)
    private void MenuUpdate(KeyboardState keyboardState, GameTime gameTime)
    {
        // UpdateBackground(gameTime);

        if (IsNewKeyPress(keyboardState, Keys.Down))
            selectedMenuItem = (selectedMenuItem + 1) % 3;

        if (IsNewKeyPress(keyboardState, Keys.Up))
            selectedMenuItem = (selectedMenuItem + 2) % 3;

        if (IsNewKeyPress(keyboardState, Keys.S))
        {
            selectedMenuItem = 0;
            ActivateMenuItem();
        }

        if (IsNewKeyPress(keyboardState, Keys.H))
        {
            selectedMenuItem = 1;
            ActivateMenuItem();
        }

        if (IsNewKeyPress(keyboardState, Keys.A))
        {
            selectedMenuItem = 2;
            ActivateMenuItem();
        }

        if (IsNewKeyPress(keyboardState, Keys.O)) currentState = State.About;

        if (IsNewKeyPress(keyboardState, Keys.P)) currentState = State.HowToPlay;

        if (IsNewKeyPress(keyboardState, Keys.Enter))
            ActivateMenuItem();
    }

    // Aktivera valt menyobjekt
    private void ActivateMenuItem()
    {
        if (selectedMenuItem == 0)
        {
            ResetGame();
            currentState = State.Run;
        }
        else if (selectedMenuItem == 1)
        {
            currentState = State.HighScore;
        }
        else
        {
            currentState = State.Quit;
        }
    }

    // Uppdatera spelet under en aktiv runda
    private void RunUpdate(GameTime gameTime)
    {
        UpdateBackground(gameTime);
        player.Update(Window, gameTime);
        UpdateCoins(gameTime);
        UpdateEnemies(gameTime);

        if (!player.IsAlive)
        {
            if (highScore.IsHighScore(player.Points))
                currentState = State.EnterScore;
            else
                currentState = State.HighScore;
        }
    }

    // Namninmatning för highscore
    private void EnterScoreUpdate(GameTime gameTime)
    {
        UpdateBackground(gameTime);

        var done = highScore.EnterUpdate(gameTime, player.Points);
        if (done)
        {
            highScore.SaveToFile(highScoreFilePath);
            currentState = State.HighScore;
        }
    }

    // Visa highscore-lista
    private void HighScoreUpdate(KeyboardState keyboardState, GameTime gameTime)
    {
        UpdateBackground(gameTime);

        if (IsNewKeyPress(keyboardState, Keys.Enter) || IsNewKeyPress(keyboardState, Keys.Escape))
            currentState = State.Menu;
    }

    // Visa credits/OM
    private void AboutUpdate(KeyboardState keyboardState, GameTime gameTime)
    {
        UpdateBackground(gameTime);

        if (IsNewKeyPress(keyboardState, Keys.Enter) || IsNewKeyPress(keyboardState, Keys.Escape))
            currentState = State.Menu;
    }

    // Visa instruktioner för hur man spelar
    private void HowToPlayUpdate(KeyboardState keyboardState, GameTime gameTime)
    {
        UpdateBackground(gameTime);

        if (IsNewKeyPress(keyboardState, Keys.Enter) || IsNewKeyPress(keyboardState, Keys.Escape))
            currentState = State.Menu;
    }

    // Returnera true endast på nytt knapptryck (inte när man håller det)
    private bool IsNewKeyPress(KeyboardState keyboardState, Keys key)
    {
        return keyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
    }

    // Rita huvudmenyn
    private void MenuDraw()
    {
        // Rita titel
        Vector2 titlePos = new((Window.ClientBounds.Width - menuTexture.Width) / 3.5f,
            170f);
        // 3.5f - 170f
        _spriteBatch.Draw(menuTexture, titlePos, Color.White);

        // Startposition för menyval
        var menuX = (Window.ClientBounds.Width - menuStartTexture.Width) / 2f;
        // float menuY = 210f;
        var menuY = 160f;
        var rowGap = 55f;

        //Rita menyval, markera valt alternativ
        _spriteBatch.Draw(menuStartTexture, new Vector2(menuX, menuY),
            selectedMenuItem == 0 ? Color.Yellow : Color.White);
        _spriteBatch.Draw(menuHighScoreTexture, new Vector2(menuX, menuY + rowGap),
            selectedMenuItem == 1 ? Color.Yellow : Color.White);
        _spriteBatch.Draw(menuExitTexture, new Vector2(menuX, menuY + rowGap * 2),
            selectedMenuItem == 2 ? Color.Yellow : Color.White);

        // Rita instruktioner för menyinout
        _spriteBatch.DrawString(uiFont, "UPP/NER + ENTER eller [S] [H] [A]",
            new Vector2(menuX - 8, menuY + rowGap * 3 + 20), Color.White);
        // Instruktioner för Hur/OM/Spelet
        _spriteBatch.DrawString(uiFont, "[O] OM/Credits - [P] Hur man spelar",
            new Vector2(menuX - 8, menuY + rowGap * 3 + 55), Color.White);
    }

    // Rita spelare, fiender och HUD (Heads-Up Display)
    private void RunDraw()
    {
        // Rita alla fiender
        foreach (var e in enemies)
            e.Draw(_spriteBatch);

        // Rita alla coins
        foreach (var c in coins)
            c.Draw(_spriteBatch);

        // Rita spelaren
        player.Draw(_spriteBatch);

        // Rita HUD-information (poäng, statistik, kontroller)
        _spriteBatch.DrawString(uiFont, $"Points: {player.Points}", new Vector2(10, 10), Color.White);
        _spriteBatch.DrawString(uiFont, $"Kills: {enemiesShot}", new Vector2(10, 34), Color.White);
        _spriteBatch.DrawString(uiFont, $"Coins: {coinsCollected}", new Vector2(10, 58), Color.White);
        _spriteBatch.DrawString(uiFont, "SPACE -> Shoot", new Vector2(10, 82), Color.White);
        _spriteBatch.DrawString(uiFont, "ESC -> Menu", new Vector2(10, 106), Color.White);
    }

    // Rita highscorelistan
    private void HighScoreDraw()
    {
        highScore.PrintDraw(_spriteBatch, uiFont);
        _spriteBatch.DrawString(uiFont, "ENTER eller ESC för meny", new Vector2(20, Window.ClientBounds.Height - 40),
            Color.White);
    }

    // Rita vy för namninmatning
    private void EnterScoreDraw()
    {
        // Instruktion för att gå tillbaka
        var title = "NY HIGHSCORE";
        var help = "Skriv 1-3 bokstäver";
        var controls = "ENTER = spara, BACKSPACE = radera";
        var score = $"Poäng: {player.Points}";

        var centerX = Window.ClientBounds.Width / 2f;
        var baseY = 110f;
        var lineGap = 38f;

        Vector2 titlePos = new(centerX - uiFont.MeasureString(title).X / 2f, baseY);
        Vector2 helpPos = new(centerX - uiFont.MeasureString(help).X / 2f, baseY + lineGap);
        Vector2 controlsPos = new(centerX - uiFont.MeasureString(controls).X / 2f, baseY + lineGap * 2);
        Vector2 scorePos = new(centerX - uiFont.MeasureString(score).X / 2f, baseY + lineGap * 3);
        // Rita text
        _spriteBatch.DrawString(uiFont, title, titlePos, Color.Yellow);
        _spriteBatch.DrawString(uiFont, help, helpPos, Color.White);
        _spriteBatch.DrawString(uiFont, controls, controlsPos, Color.White);
        _spriteBatch.DrawString(uiFont, score, scorePos, Color.White);
        highScore.EnterDraw(_spriteBatch, uiFont);
    }

    // Rita information & credits
    private void AboutDraw()
    {
        _spriteBatch.DrawString(uiFont, "Om Spelet", new Vector2(20, 20), Color.Yellow);
        _spriteBatch.DrawString(uiFont, "SpaceShooter byggt i MonoGame enligt P02 C# kurs tutorial.",
            new Vector2(20, 60), Color.White);
        _spriteBatch.DrawString(uiFont, "Skapare: X -", new Vector2(20, 88), Color.White);
        _spriteBatch.DrawString(uiFont, "Distribution: Endast för skoluppgift, ej för vidare distribution.",
            new Vector2(20, 116), Color.White);
        _spriteBatch.DrawString(uiFont, "Credits/licens:", new Vector2(20, 154), Color.Yellow);
        _spriteBatch.DrawString(uiFont, "- Kodbas: kapitel 8-9 + en extern HighScore-klass", new Vector2(20, 182),
            Color.White);
        _spriteBatch.DrawString(uiFont, "- Sprites: lokala assets för SpaceSh.. (meny, skepp, fiender, bakgrund)",
            new Vector2(20, 210), Color.White);
        _spriteBatch.DrawString(uiFont, "- Eventuella externa assets ska ..... Lorem ipsum dolor sit amet,  ",
            new Vector2(20, 238), Color.White);
        _spriteBatch.DrawString(uiFont, "Tryck ENTER eller ESC för att gå tillbaka till menyn.",
            new Vector2(20, Window.ClientBounds.Height - 40), Color.White);
    }

    // Rita instruktioner för spelaren om (Hur man spealr)
    private void HowToPlayDraw()
    {
        _spriteBatch.DrawString(uiFont, "Hur man spelar", new Vector2(20, 20), Color.Yellow);
        _spriteBatch.DrawString(uiFont, "- Pil-tangenter eller WASD: flytta skeppet", new Vector2(20, 60), Color.White);
        _spriteBatch.DrawString(uiFont, "- SPACE: skjut", new Vector2(20, 88), Color.White);
        _spriteBatch.DrawString(uiFont, "- ESC: tillbaka till menyn", new Vector2(20, 116), Color.White);
        _spriteBatch.DrawString(uiFont, "Tryck ENTER eller ESC för att gå tillbaka till menyn.",
            new Vector2(20, Window.ClientBounds.Height - 40), Color.White);
    }

    // Rita en scrollande bakgrund (loopa vertikalt)
    private void DrawBackground()
    {
        var width = Window.ClientBounds.Width;
        var height = Window.ClientBounds.Height;
        // Destination = hela skärmen
        var destination = new Rectangle(0, 0, width, height);
        // Source flytta för att skapa scroll-effekt
        var source = new Rectangle(0, (int)backgroundScroll, width, height);
        _spriteBatch.Draw(backgroundTexture, destination, source, Color.White);
    }

    // Uppdatera bakgrundens scroll-position
    private void UpdateBackground(GameTime gameTime)
    {
        var scrollSpeedPerSecond = 1.4f * 60f;
        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        // Flytta bakgrunden nedåt
        backgroundScroll += scrollSpeedPerSecond * dt;
        // Loopa tillbaka när slutet nås
        if (backgroundScroll >= backgroundTexture.Height)
            backgroundScroll -= backgroundTexture.Height;
    }

    //  En ny fiendevåg
    private void SpawnEnemies()
    {
        enemies.Clear();

        //Minor-fiender
        for (var i = 0; i < 10; i++)
        {
            var x = GetSpawnXAwayFromPlayer(mineTexture.Width);
            var y = random.Next(-Window.ClientBounds.Height, -mineTexture.Height);
            enemies.Add(new Mine(mineTexture, x, y));
        }

        // Tripod-fiender
        for (var i = 0; i < 5; i++)
        {
            var x = GetSpawnXAwayFromPlayer(tripodTexture.Width);
            var y = random.Next(-Window.ClientBounds.Height, -tripodTexture.Height);
            enemies.Add(new Tripod(tripodTexture, x, y));
        }

        // Lägg till extra fiende
        for (var i = 0; i < 4; i++)
        {
            var x = GetSpawnXAwayFromPlayer(stoneTexture.Width);
            var y = random.Next(-Window.ClientBounds.Height, -stoneTexture.Height);
            enemies.Add(new StoneEnemy(stoneTexture, x, y));
        }
    }

    // Spawnar en fiende av slumpad typ för kontinuerligt spel utan vågor
    private void SpawnRandomEnemy()
    {
        var enemyType = random.Next(0, 3);

        if (enemyType == 0)
        {
            var x = GetSpawnXAwayFromPlayer(mineTexture.Width);
            var y = random.Next(-Window.ClientBounds.Height, -mineTexture.Height);
            enemies.Add(new Mine(mineTexture, x, y));
        }
        else if (enemyType == 1)
        {
            var x = GetSpawnXAwayFromPlayer(tripodTexture.Width);
            var y = random.Next(-Window.ClientBounds.Height, -tripodTexture.Height);
            enemies.Add(new Tripod(tripodTexture, x, y));
        }
        else
        {
            var x = GetSpawnXAwayFromPlayer(stoneTexture.Width);
            var y = random.Next(-Window.ClientBounds.Height, -stoneTexture.Height);
            enemies.Add(new StoneEnemy(stoneTexture, x, y));
        }
    }

    // Minska risken att ny våg spawnar rakt i spelarens lane
    private int GetSpawnXAwayFromPlayer(int enemyWidth)
    {
        var maxX = Math.Max(1, Window.ClientBounds.Width - enemyWidth);

        if (player == null || !player.IsAlive)
            return random.Next(0, maxX);

        var playerCenterX = player.X + player.Width / 2f;
        const float minHorizontalGap = 90f;

        for (var i = 0; i < 12; i++)
        {
            var x = random.Next(0, maxX);
            var enemyCenterX = x + enemyWidth / 2f;
            if (Math.Abs(enemyCenterX - playerCenterX) >= minHorizontalGap)
                return x;
        }

        return random.Next(0, maxX);
    }

    // Guldmynt ovanför skärmen
    private void SpawnCoin()
    {
        // Se till att X är alltid inom skärmen
        var maxX = Math.Max(1, Window.ClientBounds.Width - coinTexture.Width);
        var x = random.Next(0, maxX);
        var y = -coinTexture.Height;
        coins.Add(new Coin(coinTexture, x, y));
    }

    // Uppdatera alla coins (spawn, rörelse, kollision)
    private void UpdateCoins(GameTime gameTime)
    {
        if (gameTime.TotalGameTime.TotalMilliseconds - timeSinceLastCoinSpawn >= CoinSpawnIntervalMs)
        {
            SpawnCoin();
            timeSinceLastCoinSpawn = gameTime.TotalGameTime.TotalMilliseconds;
        }

        foreach (var c in coins.ToList())
        {
            if (c.IsAlive)
            {
                c.Update(Window);
                // KOntrollera kollisionen
                if (player.IsAlive && c.IsAlive && c.CheckCollision(player))
                {
                    c.IsAlive = false;

                    // Uppdatera statistik och poäng
                    coinsCollected++;
                    player.Points += PointsPerCoin;
                }
            }

            // Ta bort coin om det är inte längre aktiv
            if (!c.IsAlive)
                coins.Remove(c);
        }
    }

    // Uppdatera fiender, kollisioner och tidsstyrd spawn
    private void UpdateEnemies(GameTime gameTime)
    {
        foreach (var e in enemies.ToList())
            if (e.IsAlive)
            {
                foreach (var b in player.Bullets)
                    // Kolla om någon kula har träffat fienden
                    if (e.IsAlive && b.IsAlive && CheckBulletHit(e, b))
                    {
                        // Döda fiende och kula
                        e.IsAlive = false;
                        b.IsAlive = false;
                        // Ge poäng
                        player.Points += PointsPerKill;
                        enemiesShot++;
                        break;
                    }

                if (e.IsAlive)
                {
                    if (player.IsAlive && e.CheckCollision(player))
                        player.IsAlive = false;

                    e.Update(Window);
                }
            }
            else
            {
                enemies.Remove(e);
            }

        if (player.IsAlive &&
            gameTime.TotalGameTime.TotalMilliseconds - timeSinceLastEnemySpawn >= EnemySpawnIntervalMs)
        {
            SpawnRandomEnemy();
            timeSinceLastEnemySpawn = gameTime.TotalGameTime.TotalMilliseconds;
        }
    }

    // Kontrollera om en kula träffar en fiende (hitbox)
    private bool CheckBulletHit(Enemy enemy, Bullet bullet)
    {
        // Skapa rektanglar för fiende och kula
        var enemyRect = new Rectangle((int)enemy.X, (int)enemy.Y, (int)enemy.Width, (int)enemy.Height);
        var bulletRect = new Rectangle((int)bullet.X, (int)bullet.Y, (int)bullet.Width, (int)bullet.Height);

        // Gör hitboxen lite större
        bulletRect.Inflate(3, 3);
        return enemyRect.Intersects(bulletRect);
    }

    // Återställ spelet
    private void ResetGame()
    {
        player.Reset( // Återställ spelarens position och stats
            Window.ClientBounds.Width / 2f,
            Window.ClientBounds.Height - 120,
            5f,
            5f);
        // Nollställ statistik
        enemiesShot = 0;
        coinsCollected = 0;
        coins.Clear(); // REnsa coins
        timeSinceLastEnemySpawn = 0;
        timeSinceLastCoinSpawn = 0;
        SpawnEnemies();
        SpawnCoin();
    }

    // Gamestate
    private enum State
    {
        Menu,
        Run,
        EnterScore,
        HighScore,
        About,
        HowToPlay,
        Quit
    }
}