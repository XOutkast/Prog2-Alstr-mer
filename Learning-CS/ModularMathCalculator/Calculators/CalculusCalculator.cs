using ModularMath.Core;

namespace ModularMath.Calculators;
/// Handles derivative and integral calculations using the power rule.
/// Inherits shared menu/display helpers from Calculator.
public class CalculusCalculator : Calculator
{
    public override string Title => "CALCULUS CALCULATOR";

    public override void ShowMenu()
    {   
        while (true)
        {
            PrintHeader();
            Console.WriteLine("1. Power Rule Derivative  (d/dx of x^n)");
            Console.WriteLine("2. Polynomial Derivative  (multi-term)");
            Console.WriteLine("3. Power Rule Integral    (∫x^n dx)");
            Console.WriteLine("4. Back to Main Menu\n");

            switch (ReadChoice("Enter your choice (1-4): "))
            {
                case "1": DerivativePowerRule(); break;
                case "2": PolynomialDerivative(); break;
                case "3": IntegralPowerRule(); break;
                case "4": return;
                default:
                    Console.WriteLine("\nInvalid choice.");
                    Pause();
                    break;
            }
        }
    }
    // - Private operations
    private void DerivativePowerRule()
    {
        PrintHeader();
        Console.WriteLine("Formula:  d/dx(x^n) = n * x^(n-1)\n");

        if (!ReadDouble("Enter the exponent (n): ", out double n))
        { Pause(); return; }

        double newExp = n - 1;
        Console.WriteLine(newExp == 0
            ? $"\nf'(x) = {n}"
            : $"\nf'(x) = {n} * x^{newExp}");

        Pause();
    }

    private void PolynomialDerivative()
    {
        PrintHeader();
        Console.WriteLine("Enter each term as:  <coefficient> <exponent>");
        Console.WriteLine("Example: 3x² → \"3 2\"\n");

        if (!ReadInt("How many terms? ", out int count) || count <= 0)
        { Console.WriteLine("Must be at least 1 term."); Pause(); return; }

        Console.WriteLine("\nDerivative terms:");
        for (int i = 0; i < count; i++)
        {
            Console.Write($"  Term {i + 1}: ");
            string[] parts = (Console.ReadLine() ?? "").Trim().Split(' ');

            if (parts.Length == 2
                && double.TryParse(parts[0], out double coeff)
                && double.TryParse(parts[1], out double exp))
            {
                double newCoeff = coeff * exp;
                double newExp   = exp - 1;
                string result   = newExp == 0
                    ? $"{newCoeff}"
                    : $"{newCoeff} * x^{newExp}";
                Console.WriteLine($"    → {result}");
            }
            else
            {
                Console.WriteLine("    → Invalid term, skipped.");
            }
        }

        Pause();
    }

    private void IntegralPowerRule()
    {
        PrintHeader();
        Console.WriteLine("Formula:  ∫x^n dx = x^(n+1) / (n+1) + C\n");

        if (!ReadDouble("Enter the exponent (n): ", out double n))
        { Pause(); return; }

        double newExp = n + 1;
        Console.WriteLine($"\nF(x) = x^{newExp} / {newExp} + C");
        Console.WriteLine($"     = {1.0 / newExp:F4} * x^{newExp} + C");

        Pause();
    }
}
