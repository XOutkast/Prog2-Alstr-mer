namespace ModularMath.Core;

/// <summary>
/// Abstract base class for all calculator modules.
/// Enforces a common contract: every calculator must have a title and expose a menu.
/// </summary>
public abstract class Calculator
{
    /// <summary>Display name shown in menus and headers.</summary>
    public abstract string Title { get; }

    /// <summary>
    /// Runs the calculator's interactive sub-menu loop.
    /// Implementations should loop until the user chooses to go back.
    /// </summary>
    public abstract void ShowMenu();

    /// <summary>
    /// Prints a standard boxed header using the calculator's Title.
    /// Reused by every subclass so the look stays consistent.
    /// </summary>
    protected void PrintHeader()
    {
        string line = new string('═', Title.Length + 6);
        Console.Clear();
        Console.WriteLine($"╔{line}╗");
        Console.WriteLine($"║   {Title}   ║");
        Console.WriteLine($"╚{line}╝\n");
    }

    /// <summary>
    /// Shared "press any key" pause used after every result screen.
    /// </summary>
    protected static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    /// <summary>
    /// Reads a double from stdin. Returns false and prints an error on bad input.
    /// </summary>
    protected static bool ReadDouble(string prompt, out double value)
    {
        Console.Write(prompt);
        if (double.TryParse(Console.ReadLine(), out value))
            return true;

        Console.WriteLine("Invalid input — please enter a numeric value.");
        return false;
    }

    /// <summary>
    /// Reads an int from stdin. Returns false and prints an error on bad input.
    /// </summary>
    protected static bool ReadInt(string prompt, out int value)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out value))
            return true;

        Console.WriteLine("Invalid input — please enter a whole number.");
        return false;
    }

    /// <summary>
    /// Reads a menu choice string from stdin.
    /// </summary>
    protected static string ReadChoice(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? "";
    }
}
