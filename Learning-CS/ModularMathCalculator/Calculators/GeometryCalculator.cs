using ModularMath.Core;

namespace ModularMath.Calculators;
/// Handles triangle, square, and Pythagorean theorem calculations.
/// Inherits shared menu/display helpers from Calculator.
public class GeometryCalculator : Calculator
{
    public override string Title => "GEOMETRY CALCULATOR";

    public override void ShowMenu()
    {
        while (true)
        {
            PrintHeader();
            Console.WriteLine("1. Triangle    (Area & Perimeter)");
            Console.WriteLine("2. Square      (Area, Perimeter, Diagonal)");
            Console.WriteLine("3. Pythagorean Theorem  (a² + b² = c²)");
            Console.WriteLine("4. Back to Main Menu\n");

            switch (ReadChoice("Enter your choice (1-4): "))
            {
                case "1": Triangle(); break;
                case "2": Square(); break;
                case "3": Pythagorean(); break;
                case "4": return;
                default:
                    Console.WriteLine("\nInvalid choice.");
                    Pause();
                    break;
            }
        }
    }

    // - Private operations
    private void Triangle()
    {
        PrintHeader();
        Console.WriteLine("1. Right Triangle  (2 legs known)");
        Console.WriteLine("2. General Triangle (3 sides known)\n");

        switch (ReadChoice("Select type: "))
        {
            case "1": RightTriangle(); break;
            case "2": GeneralTriangle(); break;
            default: Console.WriteLine("Invalid choice."); break;
        }

        Pause();
    }

    private void RightTriangle()
    {
        Console.WriteLine();
        if (!ReadDouble("Leg 1: ", out double leg1)) return;
        if (!ReadDouble("Leg 2: ", out double leg2)) return;

        double hyp       = Math.Sqrt(leg1 * leg1 + leg2 * leg2);
        double area      = (leg1 * leg2) / 2.0;
        double perimeter = leg1 + leg2 + hyp;

        Console.WriteLine("\n─── Right Triangle Results ───");
        Console.WriteLine($"Area:        {area:F4} sq units");
        Console.WriteLine($"Hypotenuse:  {hyp:F4} units");
        Console.WriteLine($"Perimeter:   {perimeter:F4} units");
    }

    private void GeneralTriangle()
    {
        Console.WriteLine();
        if (!ReadDouble("Side a: ", out double a)) return;
        if (!ReadDouble("Side b: ", out double b)) return;
        if (!ReadDouble("Side c: ", out double c)) return;

        if (a + b <= c || a + c <= b || b + c <= a)
        {
            Console.WriteLine("\nThese sides cannot form a valid triangle.");
            return;
        }

        double s         = (a + b + c) / 2.0;          // semi-perimeter
        double area      = Math.Sqrt(s * (s-a) * (s-b) * (s-c)); // Heron's formula
        double perimeter = a + b + c;

        Console.WriteLine("\n─── Triangle Results (Heron's Formula) ───");
        Console.WriteLine($"Area:       {area:F4} sq units");
        Console.WriteLine($"Perimeter:  {perimeter:F4} units");
    }

    private void Square()
    {
        PrintHeader();
        if (!ReadDouble("Side length: ", out double side))
        { Pause(); return; }

        double area      = side * side;
        double perimeter = 4 * side;
        double diagonal  = side * Math.Sqrt(2);

        Console.WriteLine("\n─── Square Results ───");
        Console.WriteLine($"Area:       {area:F4} sq units");
        Console.WriteLine($"Perimeter:  {perimeter:F4} units");
        Console.WriteLine($"Diagonal:   {diagonal:F4} units");

        Pause();
    }

    private void Pythagorean()
    {
        PrintHeader();
        Console.WriteLine("1. Find Hypotenuse  (given both legs)");
        Console.WriteLine("2. Find Missing Leg (given hypotenuse + one leg)\n");

        switch (ReadChoice("Select: "))
        {
            case "1":
                if (!ReadDouble("Leg a: ", out double a)) break;
                if (!ReadDouble("Leg b: ", out double b)) break;
                double c = Math.Sqrt(a * a + b * b);
                Console.WriteLine($"\n{a}² + {b}² = {c:F4}²");
                Console.WriteLine($"Hypotenuse: {c:F4}");
                break;

            case "2":
                if (!ReadDouble("Hypotenuse (c): ", out double hyp)) break;
                if (!ReadDouble("Known leg (a):  ", out double knownLeg)) break;
                if (hyp <= knownLeg)
                { Console.WriteLine("\nHypotenuse must be longer than the leg."); break; }
                double missingLeg = Math.Sqrt(hyp * hyp - knownLeg * knownLeg);
                Console.WriteLine($"\n{knownLeg}² + {missingLeg:F4}² = {hyp}²");
                Console.WriteLine($"Missing Leg: {missingLeg:F4}");
                break;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }

        Pause();
    }
}
