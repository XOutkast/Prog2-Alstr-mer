using System;

namespace Snake.Services;

// Representera en highscore-post och spara spelarnamn, poäng och tid.
public class HighScoreEntry
{
    public string PlayerName { get; set; } = string.Empty;
    public int Score { get; set; }
    public DateTime PlayedAt { get; set; }
}
