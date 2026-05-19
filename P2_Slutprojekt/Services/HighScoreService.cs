using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Snake.Services;

// Hantera läsning och skrivning av highscore-listan i en JSON-fil.
public class HighScoreService
{
    private readonly string filePath;

    public HighScoreService(string filePath)
    {
        this.filePath = filePath;
    }

    // Läs highscores från fil och returnera topp 10.
    public List<HighScoreEntry> LoadScores()
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return new List<HighScoreEntry>();
            }

            var json = File.ReadAllText(filePath);
            var scores = JsonSerializer.Deserialize<List<HighScoreEntry>>(json);
            return scores?.OrderByDescending(x => x.Score).Take(10).ToList() ?? new List<HighScoreEntry>();
        }
        catch
        {
            return new List<HighScoreEntry>();
        }
    }

    // Avgör om poängen kvalificerar för topplistan.
    public bool IsHighScore(int score)
    {
        var scores = LoadScores();
        return score > 0 && (scores.Count < 10 || score > scores.Min(x => x.Score));
    }

    // Spara ny score och skriv den uppdaterade topplistan till fil.
    public void SaveScore(string playerName, int score)
    {
        try
        {
            var scores = LoadScores();
            scores.Add(new HighScoreEntry
            {
                PlayerName = string.IsNullOrWhiteSpace(playerName) ? "ANON" : playerName.Trim(),
                Score = score,
                PlayedAt = DateTime.Now
            });

            var ordered = scores.OrderByDescending(x => x.Score).Take(10).ToList();
            var json = JsonSerializer.Serialize(ordered, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
        catch
        {
        }
    }
}
