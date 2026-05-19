namespace CarDemo;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("= CarDemo - OOP Tut =\n");

        // Skapa bil objekt
        var c1 = new Car("ABC123", "Volvo", "V70", 2012, true);

        // Skriv rubrik
        Console.WriteLine("{0, -10} {1, -15} {2, -15} {3, -20} {4, -10}", "RegNr", "Märke", "Modell", "Årsmodell",
            "Till salu");
        Console.WriteLine(new string('-', 75));

        // Skriva ut bilen
        Console.WriteLine(c1.ToString());
        Console.WriteLine("\n- Test för validering -");
        // Test felaktigt årtal
        var c2 = new Car("XYZ789", "Toyota", "Corolla", -800, false);
        Console.WriteLine(c2.ToString());

        // User-inout
        Console.WriteLine("\n- Skapa din egen bil -");

        // read regnr
        Console.Write("Ange registreringsnummer: ");
        var regNr = Console.ReadLine()!;

        // read brand
        Console.Write("Ange märke: ");
        var make = Console.ReadLine()!;

        // read modell
        Console.Write("Ange modell: ");
        var model = Console.ReadLine()!;

        // Läs årtal
        Console.Write("Ange årsmodell: ");
        int year = Convert.ToInt16(Console.ReadLine());

        // Läs till salu status
        Console.Write("Är bilen till salu? (J/N): ");
        var svar = Convert.ToChar(Console.Read());
        Console.ReadLine(); // Rensa buffer
        var forSale = char.ToUpper(svar) == 'J';

        // Skapa ny bil
        var c3 = new Car(regNr, make, model, year, forSale);

        // Skriv ut alla bilar
        Console.WriteLine("\n- Alla bilar -");
        // för list formatering
        Console.WriteLine("{0, -10} {1, -15} {2, -15} {3, -20} {4, -10}", "RegNr", "Märke", "Modell", "Årsmodell",
            "Till salu");
        Console.WriteLine(new string('-', 75));
        Console.WriteLine(c1.ToString());
        Console.WriteLine(c2.ToString());
        Console.WriteLine(c3.ToString());
        Console.WriteLine("\nTryck på en tangent för att avsluta...");
        Console.ReadKey();
    }
}