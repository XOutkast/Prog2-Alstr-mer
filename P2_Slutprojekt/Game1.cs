using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Snake.Core;
using Snake.Models;
using Snake.Services;

namespace Snake;

// Hantera spelets loop, rendering och input.
public class Game1 : Game
{
    // konstanter
    private const int CellSize = 24;
    private const int BoardWidth = 24;
    private const int BoardHeight = 20;
    private const int BoardOffsetY = 72;
    private const int PointsPerLevel = 15;
    private const double BaseStepInterval = 0.26;

    // Status
    private enum ScreenState
    {
        Menu,
        Playing,
        GameOver,
        EnterName,
        HighScore
    }

    // Meny
    private enum MenuSelection
    {
        Start,
        HighScore,
        Quit
    }

    // Medlemsvariabler
    private readonly GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch = null!;
    private Texture2D pixel = null!;
    private SpriteFont font = null!;
    private readonly Random random = new();
    private readonly List<Point> walls = new();
    private readonly List<Collectible> collectibles = new();
    private readonly List<Hazard> hazards = new();
    private KeyboardState previousKeyboard;
    private ScreenState screen = ScreenState.Menu;
    private MenuSelection selectedMenu = MenuSelection.Start;
    private Models.Snake snake = null!;
    private HighScoreService highScoreService = null!;
    private List<HighScoreEntry> highScores = new();
    // Fixed-step frame counter (replaces accumulator timing)
    private int stepFrameCounter = 0;
    private double stepInterval = BaseStepInterval;
    // Temporär hastighetsboost efter level-up (2% snabbare under en kort stund)
    private double levelSpeedBoostTimer = 0.0;
    private const double LevelSpeedBoostDuration = 5.0; // sekunder
    private const double LevelSpeedBoostFactor = 0.98; // 2% snabbare
    private int score;
    private int level;
    private string playerName = string.Empty;
    private double nameCursorTimer;
    private bool nameCursorVisible = true;

    // Beräkna BoardPixelWidth/Height
    private int BoardPixelWidth => BoardWidth * CellSize;
    private int BoardPixelHeight => BoardHeight * CellSize;
    private int BoardLeft => (GraphicsDevice.Viewport.Width - BoardPixelWidth) / 2;

    public Game1()
    {
        // KOnstanter: Grafik, innehållskatalog och fönsteregenskaper.
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        // Använd fast uppdateringssteg för enklare timing
        IsFixedTimeStep = true;
        Window.AllowUserResizing = false;
        graphics.PreferredBackBufferWidth = 740;
        graphics.PreferredBackBufferHeight = 620;
        graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        // Init spelvariabler & fönstertitel.
        base.Initialize();
        Window.Title = "X Snake";
        score = 0;
        level = 1;
    }

    protected override void LoadContent()
    {
        // LoadContent: Ladda resurser och init tjänster
        spriteBatch = new SpriteBatch(GraphicsDevice);
        pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });
        font = Content.Load<SpriteFont>("MyFont");
        highScoreService = new HighScoreService(Path.Combine(AppContext.BaseDirectory, "data.db"));
        ResetRun();
    }

    protected override void Update(GameTime gameTime)
    {
        // Update: spel logik (switch state)
        var keyboard = Keyboard.GetState();

        switch (screen)
        {
            case ScreenState.Menu:
                UpdateMenu(keyboard);
                break;
            case ScreenState.Playing:
                UpdatePlaying(gameTime, keyboard);
                break;
            case ScreenState.GameOver:
                UpdateGameOver(keyboard);
                break;
            case ScreenState.EnterName:
                UpdateNameEntry(keyboard, gameTime);
                break;
            case ScreenState.HighScore:
                UpdateHighScore(keyboard);
                break;
        }

        previousKeyboard = keyboard;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Draw: Rendera scenen, bg, bräde ...
        GraphicsDevice.Clear(new Color(16, 18, 24));

        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);

        DrawFrame();

        switch (screen)
        {
            case ScreenState.Menu:
                DrawMenu();
                break;
            case ScreenState.Playing:
                DrawPlayScreen();
                break;
            case ScreenState.GameOver:
                DrawGameOver();
                break;
            case ScreenState.EnterName:
                DrawNameEntry();
                break;
            case ScreenState.HighScore:
                DrawHighScore();
                break;
        }

        spriteBatch.End();
        base.Draw(gameTime);
    }

    // Reset spelet
    private void ResetRun()
    {
        score = 0;
        level = 1;
        stepInterval = BaseStepInterval;
        stepFrameCounter = 0;
        playerName = string.Empty;
        nameCursorVisible = true;
        nameCursorTimer = 0;

        snake = new Models.Snake(new Point(BoardWidth / 2, BoardHeight / 2));
        walls.Clear();
        collectibles.Clear();
        hazards.Clear();

        SetupLevel(level);
        SpawnCollectible();
        screen = ScreenState.Menu;
    }

    private void StartGame()
    {
        score = 0;
        level = 1;
        stepInterval = BaseStepInterval;
        stepFrameCounter = 0;
        playerName = string.Empty;
        nameCursorVisible = true;
        nameCursorTimer = 0;

        snake = new Models.Snake(new Point(BoardWidth / 2, BoardHeight / 2));
        walls.Clear();
        collectibles.Clear();
        hazards.Clear();

        screen = ScreenState.Playing;
        SetupLevel(level);
        SpawnCollectible();
    }

    // Nivå setup
    private void SetupLevel(int currentLevel)
    {
        if (currentLevel == 1)
        {
            AddBorderWalls();
            AddCentralGateWall();
            hazards.Add(new SpikeTrap(RandomEmptyCell()));
            return;
        }

        AddRandomWallSegments(GetWallSegmentsToAdd(currentLevel));
        AddHazardsForLevel(currentLevel);
    }

    private void AddBorderWalls()
    {
        for (var x = 0; x < BoardWidth; x++)
        {
            walls.Add(new Point(x, 0));
            walls.Add(new Point(x, BoardHeight - 1));
        }

        for (var y = 1; y < BoardHeight - 1; y++)
        {
            walls.Add(new Point(0, y));
            walls.Add(new Point(BoardWidth - 1, y));
        }
    }

    private void AddCentralGateWall()
    {
        for (var y = 3; y < BoardHeight - 3; y++)
        {
            if (y == BoardHeight / 2)
            {
                continue;
            }

            walls.Add(new Point(BoardWidth / 2, y));
        }
    }

    private int GetWallSegmentsToAdd(int currentLevel)
    {
        if (currentLevel <= 3)
        {
            return 1;
        }

        if (currentLevel <= 5)
        {
            return 2;
        }

        if (currentLevel <= 8)
        {
            return 3;
        }

        return 4;
    }

    private void AddRandomWallSegments(int segmentCount)
    {
        for (var i = 0; i < segmentCount; i++)
        {
            var start = RandomEmptyCell();
            if (IsTooCloseToCenter(start, 2))
            {
                continue;
            }

            walls.Add(start);

            var horizontal = random.Next(2) == 0;
            var dir = random.Next(2) == 0 ? -1 : 1;
            var adjacent = horizontal
                ? new Point(start.X + dir, start.Y)
                : new Point(start.X, start.Y + dir);

            if (IsValidRandomWallCell(adjacent))
            {
                walls.Add(adjacent);
            }
        }
    }

    private bool IsValidRandomWallCell(Point cell)
    {
        return cell.X > 0
               && cell.X < BoardWidth - 1
               && cell.Y > 0
               && cell.Y < BoardHeight - 1
               && !IsOccupied(cell)
               && !IsTooCloseToCenter(cell, 2);
    }

    private bool IsTooCloseToCenter(Point position, int radius)
    {
        var center = new Point(BoardWidth / 2, BoardHeight / 2);
        return Math.Abs(position.X - center.X) <= radius && Math.Abs(position.Y - center.Y) <= radius;
    }

    private void AddHazardsForLevel(int currentLevel)
    {
        if (currentLevel % 2 == 0)
        {
            hazards.Add(new SpikeTrap(RandomEmptyCell()));
        }
    }

    // Input hantering
    private void UpdateMenu(KeyboardState keyboard)
    {
        if (IsNewKeyPress(keyboard, Keys.Down) || IsNewKeyPress(keyboard, Keys.S))
        {
            selectedMenu = selectedMenu switch
            {
                MenuSelection.Start => MenuSelection.HighScore,
                MenuSelection.HighScore => MenuSelection.Quit,
                MenuSelection.Quit => MenuSelection.Start,
                _ => MenuSelection.Start
            };
        }

        if (IsNewKeyPress(keyboard, Keys.Up) || IsNewKeyPress(keyboard, Keys.W))
        {
            selectedMenu = selectedMenu switch
            {
                MenuSelection.Start => MenuSelection.Quit,
                MenuSelection.HighScore => MenuSelection.Start,
                MenuSelection.Quit => MenuSelection.HighScore,
                _ => MenuSelection.Start
            };
        }

        if (IsNewKeyPress(keyboard, Keys.Enter) || IsNewKeyPress(keyboard, Keys.Space))
        {
            ActivateMenuSelection();
        }
    }

    private void ActivateMenuSelection()
    {
        switch (selectedMenu)
        {
            case MenuSelection.Start:
                StartGame();
                break;
            case MenuSelection.HighScore:
                highScores = highScoreService.LoadScores();
                screen = ScreenState.HighScore;
                break;
            case MenuSelection.Quit:
                Exit();
                break;
        }
    }

    //Uppdatering av spelet (PLAYING STATE)
    private void UpdatePlaying(GameTime gameTime, KeyboardState keyboard)
    {
        // Spelarens rörelseinput (pilar eller WASD)
        HandleMovementInput(keyboard);

        // Avsluta till meny med ESCAPE
        if (IsNewKeyPress(keyboard, Keys.Escape))
        {
            screen = ScreenState.Menu;
            return;
        }
        // Uppdatera eventuell temporär level-boost
        if (levelSpeedBoostTimer > 0)
        {
            levelSpeedBoostTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (levelSpeedBoostTimer < 0) levelSpeedBoostTimer = 0;
        }

        // Fixed-step: räkna uppdateringsramar istället för ackumulator
        var framesPerStep = Math.Max(1, (int)Math.Round(CurrentStepInterval / TargetElapsedTime.TotalSeconds));
        stepFrameCounter++;

        while (stepFrameCounter >= framesPerStep && screen == ScreenState.Playing)
        {
            stepFrameCounter -= framesPerStep;
            UpdateSnakeStep(); // kolla kollision, flytta snake, ät osv
            UpdateHazards();
            CheckLevelProgression();
        }
    }

    // Kontroll & info

    // GAVE_OVER display
    private void UpdateGameOver(KeyboardState keyboard)
    {
        if (IsNewKeyPress(keyboard, Keys.Enter))
        {
            highScores = highScoreService.LoadScores();
            screen = ScreenState.HighScore;
        }

        if (IsNewKeyPress(keyboard, Keys.Escape))
        {
            screen = ScreenState.Menu;
        }
    }

    // HighScore
    private void UpdateHighScore(KeyboardState keyboard)
    {
        if (IsNewKeyPress(keyboard, Keys.Enter) || IsNewKeyPress(keyboard, Keys.Escape))
        {
            screen = ScreenState.Menu;
        }
    }

    private void UpdateNameEntry(KeyboardState keyboard, GameTime gameTime)
    {
        nameCursorTimer += gameTime.ElapsedGameTime.TotalSeconds;
        if (nameCursorTimer >= 0.5)
        {
            nameCursorVisible = !nameCursorVisible;
            nameCursorTimer = 0;
        }

        foreach (var key in keyboard.GetPressedKeys())
        {
            if (previousKeyboard.IsKeyDown(key))
            {
                continue;
            }

            if (key == Keys.Back)
            {
                if (playerName.Length > 0)
                {
                    playerName = playerName[..^1];
                }

                continue;
            }

            if (key == Keys.Enter)
            {
                if (!string.IsNullOrWhiteSpace(playerName))
                {
                    highScoreService.SaveScore(playerName, score);
                }

                highScores = highScoreService.LoadScores();
                screen = ScreenState.HighScore;
                return;
            }

            if (TryMapKeyToChar(key, out var ch) && playerName.Length < 12)
            {
                playerName += ch;
            }
        }

        if (IsNewKeyPress(keyboard, Keys.Escape))
        {
            screen = ScreenState.Menu;
        }
    }

    // Game movement
    private void HandleMovementInput(KeyboardState keyboard)
    {
        if (IsNewKeyPress(keyboard, Keys.Up) || IsNewKeyPress(keyboard, Keys.W))
        {
            snake.Turn(Direction.Up);
        }
        else if (IsNewKeyPress(keyboard, Keys.Down) || IsNewKeyPress(keyboard, Keys.S))
        {
            snake.Turn(Direction.Down);
        }
        else if (IsNewKeyPress(keyboard, Keys.Left) || IsNewKeyPress(keyboard, Keys.A))
        {
            snake.Turn(Direction.Left);
        }
        else if (IsNewKeyPress(keyboard, Keys.Right) || IsNewKeyPress(keyboard, Keys.D))
        {
            snake.Turn(Direction.Right);
        }
    }

    private void UpdateSnakeStep()
    {
        // Direkt applicera aktuell riktning (ingen buffring eller hypotetisk säkerhetskontroll)
        var nextHead = snake.NextHead();

        if (IsWall(nextHead) || snake.HitsSelf(nextHead))
        {
            EndSnakeRun();
            return;
        }

        snake.MoveTo(nextHead);

        // kolla collectible
        var collectible = collectibles.FirstOrDefault(item => item.Position == snake.Head);
        if (collectible != null)
        {
            collectibles.Remove(collectible);
            collectible.Apply(this);
            SpawnCollectible();
        }

        // kolla Hazards
        foreach (var hazard in hazards.ToList())
        {
            if (hazard.Position == snake.Head)
            {
                hazard.ResolveCollision(this);
                if (screen != ScreenState.Playing)
                {
                    return;
                }
            }
        }
    }

    private void UpdateHazards()
    {
        foreach (var hazard in hazards)
        {
            hazard.Update(this);
            if (hazard.Position == snake.Head)
            {
                hazard.ResolveCollision(this);
                if (screen != ScreenState.Playing)
                {
                    return;
                }
            }
        }
    }

    private void CheckLevelProgression()
    {
        while (score >= level * PointsPerLevel && screen == ScreenState.Playing)
        {
            level++;
            stepInterval = Math.Max(0.06, stepInterval * 0.98);
            // Starta en kort temporär speed boost efter level-up
            levelSpeedBoostTimer = LevelSpeedBoostDuration;
            SetupLevel(level);
            SpawnCollectible();
        }
    }

    private void SpawnCollectible()
    {
        var position = RandomEmptyCell();
        var roll = random.NextDouble();
        Collectible collectible = roll switch
        {
            < 0.60 => new Apple(position),
            < 0.80 => new GoldenApple(position),
            _ => new SpeedOrb(position)
        };

        collectibles.Add(collectible);
    }

    private Point RandomEmptyCell()
    {
        for (var attempt = 0; attempt < 200; attempt++)
        {
            var position = new Point(random.Next(1, BoardWidth - 1), random.Next(1, BoardHeight - 1));
            if (!IsOccupied(position))
            {
                return position;
            }
        }

        return new Point(2, 2); // fallback
    }

    private bool IsOccupied(Point position)
    {
        return snake.Contains(position)
               || walls.Contains(position)
               || hazards.Any(hazard => hazard.Position == position)
               || collectibles.Any(item => item.Position == position);
    }

    public bool IsBlocked(Point position)
    {
        return IsWall(position) || snake.Contains(position) || walls.Contains(position);
    }

    private bool IsWall(Point position)
    {
        return position.X <= 0 || position.Y <= 0 || position.X >= BoardWidth - 1 || position.Y >= BoardHeight - 1 || walls.Contains(position);
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void GrowSnake(int amount)
    {
        snake.Grow(amount);
    }

    public void EndSnakeRun()
    {
        screen = highScoreService.IsHighScore(score) ? ScreenState.EnterName : ScreenState.GameOver;
        if (screen == ScreenState.GameOver)
        {
            highScores = highScoreService.LoadScores();
        }
    }

    private double CurrentStepInterval => stepInterval * (levelSpeedBoostTimer > 0 ? LevelSpeedBoostFactor : 1.0);

    private bool IsNewKeyPress(KeyboardState keyboard, Keys key)
    {
        return keyboard.IsKeyDown(key) && !previousKeyboard.IsKeyDown(key);
    }

    private static bool TryMapKeyToChar(Keys key, out char character)
    {
        character = key switch
        {
            Keys.A => 'A', Keys.B => 'B', Keys.C => 'C', Keys.D => 'D', Keys.E => 'E',
            Keys.F => 'F', Keys.G => 'G', Keys.H => 'H', Keys.I => 'I', Keys.J => 'J',
            Keys.K => 'K', Keys.L => 'L', Keys.M => 'M', Keys.N => 'N', Keys.O => 'O',
            Keys.P => 'P', Keys.Q => 'Q', Keys.R => 'R', Keys.S => 'S', Keys.T => 'T',
            Keys.U => 'U', Keys.V => 'V', Keys.W => 'W', Keys.X => 'X', Keys.Y => 'Y',
            Keys.Z => 'Z',
            _ => '\0'
        };
        return character != '\0';
    }

    private void DrawFrame()
    {
        var boardRect = new Rectangle(BoardLeft, BoardOffsetY, BoardPixelWidth, BoardPixelHeight);

        DrawRect(new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), new Color(13, 15, 20));
        DrawRect(new Rectangle(boardRect.X - 8, boardRect.Y - 8, boardRect.Width + 16, boardRect.Height + 16), new Color(37, 40, 49));
        DrawRect(boardRect, new Color(20, 24, 30));

        for (var x = 0; x < BoardWidth; x++)
        {
            for (var y = 0; y < BoardHeight; y++)
            {
                if ((x + y) % 2 == 0)
                {
                    DrawCell(new Point(x, y), new Color(24, 28, 36));
                }
                else
                {
                    DrawCell(new Point(x, y), new Color(22, 26, 32));
                }
            }
        }
    }

    private void DrawPlayScreen()
    {
        DrawGameBoard();
        DrawGameplayHud();
    }

    private void DrawGameBoard()
    {
        foreach (var wall in walls)
        {
            DrawCell(wall, new Color(72, 79, 91));
        }

        foreach (var hazard in hazards)
        {
            DrawCell(hazard.Position, hazard.Tint);
        }

        foreach (var collectible in collectibles)
        {
            DrawCollectible(collectible);
        }

        for (var i = snake.Body.Count - 1; i >= 0; i--)
        {
            var segment = snake.Body[i];
            var tint = i == 0 ? new Color(109, 225, 105) : new Color(53, 170, 73);
            DrawCell(segment, tint);
        }
    }

    private void DrawGameplayHud()
    {
        var topPanel = new Rectangle(BoardLeft, 14, BoardPixelWidth, 46);
        DrawRect(topPanel, new Color(24, 28, 36));

        var blockWidth = (topPanel.Width - 16) / 2;
        DrawStatBlock("POÄNG", score.ToString(), topPanel.X + 8, topPanel.Y + 6, blockWidth - 6, Color.White);
        DrawStatBlock("NIVÅ", level.ToString(), topPanel.X + 8 + blockWidth, topPanel.Y + 6, blockWidth - 6, Color.White);
    }

    private void DrawStatBlock(string label, string value, int x, int y, int width, Color valueColor)
    {
        var labelSize = font.MeasureString(label);
        var valueSize = font.MeasureString(value);
        var labelX = x + (width - (int)labelSize.X) / 2;
        var valueX = x + (width - (int)valueSize.X) / 2;

        DrawString(label, labelX, y, Color.Gold);
        DrawString(value, valueX, y + 18, valueColor);
    }

    private void DrawMenu()
    {
        DrawCenterTitle("X SNAKE", 108, Color.Gold);
        DrawCenterText("Play X snake game", 168, Color.White);
        DrawMenuOption(MenuSelection.Start, "Starta spel", 240);
        DrawMenuOption(MenuSelection.HighScore, "HighScore", 298);
        DrawMenuOption(MenuSelection.Quit, "Avsluta", 346);

        DrawCenterText("Enter / Space = val", 450, Color.Silver);
        DrawCenterText("Pilar eller WASD flyttar i spelet", 485, Color.Silver);
    }

    private void DrawGameOver()
    {
        DrawCenterTitle("GAME OVER", 130, Color.IndianRed);
        DrawCenterText($"Poäng: {score}", 210, Color.White);
        DrawCenterText($"Nivå: {level}", 246, Color.White);
        DrawCenterText("Enter = se HighScore", 370, Color.Silver);
        DrawCenterText("Esc = meny", 402, Color.Silver);
    }

    private void DrawNameEntry()
    {
        DrawCenterTitle("NY HIGH SCORE", 110, Color.Gold);
        DrawCenterText($"Din poäng: {score}", 186, Color.White);
        DrawCenterText("Skriv ditt namn & tryck Enter", 220, Color.White);
        var displayName = playerName + (nameCursorVisible ? "_" : "");
        DrawCenteredBoxText(displayName.Length == 0 ? "___" : displayName, 302, new Color(40, 46, 60), Color.White);
        DrawCenterText("Backspace tar bort bokstäver", 368, Color.Silver);
    }

    private void DrawHighScore()
    {
        DrawCenterTitle("HIGH SCORE", 84, Color.Gold);

        if (highScores.Count == 0)
        {
            DrawCenterText("Inga H-scores sparade ännu.", 170, Color.White);
        }
        else
        {
            var y = 170;
            for (var i = 0; i < highScores.Count; i++)
            {
                var entry = highScores[i];
                DrawCenteredText($"{i + 1,2}. {entry.PlayerName,-12} {entry.Score,5}   {entry.PlayedAt:yyyy-MM-dd HH:mm}", y, Color.White);
                y += 32;
            }
        }

        DrawCenterText("Enter eller Esc = tillbaka till meny", 566, Color.Silver);
    }

    private void DrawMenuOption(MenuSelection option, string text, int y)
    {
        var color = selectedMenu == option ? Color.Gold : Color.White;
        DrawCenteredText(text, y, color);
    }

    private void DrawCollectible(Collectible collectible)
    {
        var outer = CellRect(collectible.Position);
        var inner = new Rectangle(outer.X + 3, outer.Y + 3, outer.Width - 6, outer.Height - 6);
        DrawRect(outer, collectible.Tint * 0.55f);
        DrawRect(inner, collectible.Tint);
    }

    private void DrawCenterOverlay(string title, string subtitle)
    {
        DrawRect(new Rectangle(120, 220, 520, 150), new Color(10, 12, 18, 220));
        DrawCenteredText(title, 258, Color.Gold);
        DrawCenteredText(subtitle, 298, Color.White);
    }

    private void DrawCenterTitle(string text, int y, Color color)
    {
        DrawCenteredText(text, y, color, 32);
    }

    private void DrawCenterText(string text, int y, Color color)
    {
        DrawCenteredText(text, y, color, 18);
    }

    private void DrawCenteredText(string text, int y, Color color, int? sizeOverride = null)
    {
        var measured = font.MeasureString(text);
        var x = (GraphicsDevice.Viewport.Width - measured.X) / 2f;
        spriteBatch.DrawString(font, text, new Vector2(x, y), color);
    }

    private void DrawCenteredBoxText(string text, int y, Color background, Color color)
    {
        var measured = font.MeasureString(text);
        var width = Math.Max(260, (int)measured.X + 40);
        var rect = new Rectangle((GraphicsDevice.Viewport.Width - width) / 2, y - 10, width, 42);
        DrawRect(rect, background);
        DrawCenteredText(text, y, color);
    }

    private void DrawString(string text, int x, int y, Color color)
    {
        spriteBatch.DrawString(font, text, new Vector2(x, y), color);
    }

    private void DrawCell(Point position, Color tint)
    {
        spriteBatch.Draw(pixel, CellRect(position), tint);
    }

    private Rectangle CellRect(Point position)
    {
        return new Rectangle(BoardLeft + position.X * CellSize, BoardOffsetY + position.Y * CellSize, CellSize, CellSize);
    }

    private void DrawRect(Rectangle rectangle, Color color)
    {
        spriteBatch.Draw(pixel, rectangle, color);
    }
}
