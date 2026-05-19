using ModularMath.Core;

namespace ModularMath.Calculators;

/// <summary>
/// Handles forward and inverse trigonometric calculations.
/// Supports both degree and radian input.
/// Inherits shared menu/display helpers from Calculator.
/// </summary>
public class TrigonometryCalculator : Calculator
{
    public override string Title => "TRIGONOMETRY CALCULATOR";

    public override void ShowMenu()
    {
        while (true)
        {
            PrintHeader();
            Console.WriteLine("1. Forward Functions  (sin, cos, tan from angle)");
            Console.WriteLine("2. Inverse Functions  (angle from arcsin/arccos/arctan)");
            Console.WriteLine("3. Back to Main Menu\n");

            switch (ReadChoice("Enter your choice (1-3): "))
            {
                case "1": ForwardTrig(); break;
                case "2": InverseTrig(); break;
                case "3": return;
                default:
                    Console.WriteLine("\nInvalid choice.");
                    Pause();
                    break;
            }
        }
    }

    // ── Private operations 

    private void ForwardTrig()
    {
        PrintHeader();
        Console.WriteLine("1. Degrees");
        Console.WriteLine("2. Radians\n");

        string unitChoice = ReadChoice("Input unit: ");
        double radians;

        if (unitChoice == "1")
        {
            if (!ReadDouble("Angle (degrees): ", out double deg))
            { Pause(); return; }
            radians = DegreesToRadians(deg);
        }
        else if (unitChoice == "2")
        {
            if (!ReadDouble("Angle (radians): ", out double rad))
            { Pause(); return; }
            radians = rad;
        }
        else
        {
            Console.WriteLine("Invalid unit choice.");
            Pause();
            return;
        }

        double degrees = RadiansToDegrees(radians);

        Console.WriteLine($"\n─── Trig Results for {degrees:F4}° ({radians:F4} rad) ───");
        Console.WriteLine($"sin(θ) = {Math.Sin(radians):F6}");
        Console.WriteLine($"cos(θ) = {Math.Cos(radians):F6}");
        Console.WriteLine($"tan(θ) = {Math.Tan(radians):F6}");

        Pause();
    }

    private void InverseTrig()
    {
        PrintHeader();
        Console.WriteLine("1. arcsin(x)  → angle whose sine is x");
        Console.WriteLine("2. arccos(x)  → angle whose cosine is x");
        Console.WriteLine("3. arctan(x)  → angle whose tangent is x\n");

        switch (ReadChoice("Select function: "))
        {
            case "1":
                if (!ReadDouble("sin value (-1 to 1): ", out double sinVal)) break;
                if (sinVal < -1 || sinVal > 1)
                { Console.WriteLine("Value must be in the range [-1, 1]."); break; }
                PrintAngle(Math.Asin(sinVal), "arcsin");
                break;

            case "2":
                if (!ReadDouble("cos value (-1 to 1): ", out double cosVal)) break;
                if (cosVal < -1 || cosVal > 1)
                { Console.WriteLine("Value must be in the range [-1, 1]."); break; }
                PrintAngle(Math.Acos(cosVal), "arccos");
                break;

            case "3":
                if (!ReadDouble("tan value: ", out double tanVal)) break;
                PrintAngle(Math.Atan(tanVal), "arctan");
                break;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }

        Pause();
    }

    // ── Helpers 

    private static void PrintAngle(double radians, string function)
    {
        Console.WriteLine($"\n─── {function} Result ───");
        Console.WriteLine($"Angle = {RadiansToDegrees(radians):F4}°");
        Console.WriteLine($"     = {radians:F6} radians");
    }

    private static double DegreesToRadians(double deg) => deg * (Math.PI / 180.0);
    private static double RadiansToDegrees(double rad) => rad * (180.0 / Math.PI);
}
