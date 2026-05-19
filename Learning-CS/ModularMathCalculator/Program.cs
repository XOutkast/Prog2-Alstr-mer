using ModularMath.Calculators;
using ModularMath.Core;

// ── Calculator registry ─────────────────────────────────────────────────────
// To add a new calculator, just add it here — no other file needs to change.
List<Calculator> calculators =
[
    new CalculusCalculator(),
    new GeometryCalculator(),
    new TrigonometryCalculator(),
];

// ── Main menu loop ──────────────────────────────────────────────────────────
while (true)
{
    Console.Clear();
    Console.WriteLine("╔══════════════════════════════════════╗");
    Console.WriteLine("║   MODULAR MATH CALCULATOR SYSTEM     ║");
    Console.WriteLine("╚══════════════════════════════════════╝\n");

    Console.WriteLine("Select a Calculator:");
    for (int i = 0; i < calculators.Count; i++)
        Console.WriteLine($"{i + 1}. {calculators[i].Title}");
    Console.WriteLine($"{calculators.Count + 1}. Exit\n");

    Console.Write($"Enter your choice (1-{calculators.Count + 1}): ");
    string input = Console.ReadLine() ?? "";

    if (int.TryParse(input, out int choice))
    {
        if (choice >= 1 && choice <= calculators.Count)
        {
            calculators[choice - 1].ShowMenu();
            continue;
        }
        if (choice == calculators.Count + 1)
        {
            Console.WriteLine("\nThank you for using the Modular Math Calculator!");
            break;
        }
    }

    Console.WriteLine("\nInvalid choice. Press any key to continue...");
    Console.ReadKey();
}
