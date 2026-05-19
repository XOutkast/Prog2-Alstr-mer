using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


// =======================================================================
// HsItem, en behållare-klass som innehåller info om en person i
// highscorelistan.
// =======================================================================
class HSItem
{
	// Variabler och egenskaper för dem:
	string name;
	int points;

	public string Name{ get { return name; } set { name = value; } }

	public int Points { get { return points; } set { points = value; } }

	// =======================================================================
	// HSItem(), klassens konstruktor
	// =======================================================================
	public HSItem(string name, int points)
	{
		this.name = name;
		this.points = points;
	}
}

// =======================================================================
// HighScore, innehåller en lista med hsItems samt metoder för att
// manipulera listan.
// =======================================================================
class HighScore
{
	int maxInList = 5; // Hur många som får vara i listan
	List<HSItem> highscore = new List<HSItem>();
	string name; // Spelarens namn
	const int maxNameLength = 3;
	KeyboardState previousKeyboardState;

	// =======================================================================
	// HighScore(), klassens konstruktor
	// =======================================================================
	public HighScore(int maxInList)
	{
		this.maxInList = maxInList;
		name = "";
		previousKeyboardState = Keyboard.GetState();
	}

	// =======================================================================
	// IsHighScore(), avgör om poängen bör få plats i listan.
	// =======================================================================
	public bool IsHighScore(int points)
	{
		if (highscore.Count < maxInList)
			return true;

		return points > highscore[highscore.Count - 1].Points;
	}

	// =======================================================================
	// AddScore(), lägger till poäng med explicit namn utan tangentinmatning.
	// =======================================================================
	public void AddScore(string playerName, int points)
	{
		name = playerName;
		Add(points);
		name = "";
		previousKeyboardState = Keyboard.GetState();
	}

	// =======================================================================
	// Sort(),  metod som sorterar listan. Metoden
	// anropas av Add() när en ny person läggs till i
	// listan. Använder algoritmen bubblesort
	// =======================================================================
	void Sort()
	{
		int max = highscore.Count - 1;

		// Den yttre loopen, går igenom hela listan            
		for (int i = 0; i < max; i++)
		{
			// Den inre, går igenom element för element
			int nrLeft = max - i; // För att se hur många som redan gåtts igenom
			for (int j = 0; j < nrLeft; j++)
			{
				if (highscore[j].Points < highscore[j + 1].Points) // Jämför elementen
				{
					// Byt plats!
					HSItem temp = highscore[j];
					highscore[j] = highscore[j + 1];
					highscore[j + 1] = temp;
				}
			}
		}
	}

	// =======================================================================
	// Add(), lägger till en person i highscore-listan.
	// =======================================================================
	void Add(int points)
	{
		// Skapa en temporär variabel av typen HSItem:
		HSItem temp = new HSItem(name, points);
		// Lägg till tmp i listan. Observera att följande Add()
		// tillhör klassen List (är alltså skapad av Microsoft).
		// Metoden har endast samma namn, som just denna Add():
		highscore.Add(temp);
		Sort(); // Sortera listan efter att vi har lagt till en person!

		// Är det för många i listan?
		if (highscore.Count > maxInList)
		{
			// Eftersom vi har lagt till endast en person nu, så betyder
			// det att det är en person för mycket. Index på personen
			// som är sist i listan, är samma som maxInList. Vi vill ju
			// att det högsta indexet ska vara maxInList-1. Alltså kan
			// vi bara ta bort elementet med index maxInList.
			// Exempel:
			// maxInList är 5, vi har 6 element i listan. Det sjätte
			// elementet har index 5. Vi gör highscore.RemoveAt(5):
			highscore.RemoveAt(maxInList);
		}
	}

	// =======================================================================
	// IsNewKeyPress(), returnerar true endast på nytt knapptryck.
	// =======================================================================
	bool IsNewKeyPress(KeyboardState keyboardState, Keys key)
	{
		return keyboardState.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
	}

	// =======================================================================
	// PrintDraw(), metod för att skriva ut listan. Det finns ingen
	// PrintUpdate() då det är en helt statisk text som skrivs ut.
	// =======================================================================
	public void PrintDraw(SpriteBatch spriteBatch, SpriteFont font)
	{
		string text = "HIGHSCORE\n";
		foreach(HSItem h in highscore)
			text += h.Name + " " + h.Points + "\n";

		spriteBatch.DrawString(font, text, Vector2.Zero, Color.White);
	}

	// =======================================================================
	// EnterUpdate(), användaren skriver namn med tangentbordet
	// och bekräftar med ENTER.
	// =======================================================================
	public bool EnterUpdate(GameTime gameTime, int points)
	{
		KeyboardState keyboardState = Keyboard.GetState();

		for (Keys key = Keys.A; key <= Keys.Z; key++)
		{
			if (name.Length < maxNameLength && IsNewKeyPress(keyboardState, key))
			{
				name += key.ToString();
			}
		}

		if (name.Length > 0 && IsNewKeyPress(keyboardState, Keys.Back))
		{
			name = name.Substring(0, name.Length - 1);
		}

		if (name.Length > 0 && IsNewKeyPress(keyboardState, Keys.Enter))
		{
			Add(points);
			name = "";
			previousKeyboardState = keyboardState;
			return true;
		}

		previousKeyboardState = keyboardState;
		return false;
	}

	// =======================================================================
	// EnterDraw(), skriver ut namnet som spelaren hittills har matat in.
	// =======================================================================
	public void EnterDraw(SpriteBatch spriteBatch, SpriteFont font)
	{
		string text = "ENTER NAME: " + name;
		spriteBatch.DrawString(font, text, Vector2.Zero, Color.White);
	}

	// =======================================================================
	// SaveToFile(), spara till fil.
	// =======================================================================
	public void SaveToFile(string filename)
	{
		using (StreamWriter writer = new StreamWriter(filename, false))
		{
			foreach (HSItem h in highscore)
			{
				writer.WriteLine(h.Name + ";" + h.Points);
			}
		}
	}

	// =======================================================================
	// LoadFromFile(), ladda från fil.
	// =======================================================================
	public void LoadFromFile(string filename)
	{
		highscore.Clear();

		if (!File.Exists(filename))
			return;

		using (StreamReader reader = new StreamReader(filename))
		{
			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine();
				if (string.IsNullOrWhiteSpace(line))
					continue;

				string[] parts = line.Split(';');
				if (parts.Length != 2)
					continue;

				string n = parts[0].Trim();
				if (n.Length == 0)
					continue;

				if (int.TryParse(parts[1], out int p))
					highscore.Add(new HSItem(n, p));
			}
		}

		Sort();
		if (highscore.Count > maxInList)
			highscore.RemoveRange(maxInList, highscore.Count - maxInList);
	}
}

