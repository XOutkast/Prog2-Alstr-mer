namespace m03u01;

internal class Program
{
    // (Vehicle - Car/Lorry)
    private static void Main(string[] args)
    {
        Console.WriteLine("= m03u01 - OOP Arv =\n");

        // Lista som kan innehålla både Car och Lorry (polymorfism)
        var vehicles = new List<Vehicle>();

        // Testobjekt
        vehicles.Add(new Car("ABC123", "Volvo", "V70", 2012, true));
        vehicles.Add(new Car("XYZ789", "Toyota", "Corolla", -800, false));
        vehicles.Add(new Lorry("LOR123", "Scania", "R500", 2019, true, 18000));

        Console.WriteLine("\n- Skapa din egen bil -");

        Console.Write("Ange registreringsnummer: ");
        var regNr = Console.ReadLine()!;

        Console.Write("Ange märke: ");
        var make = Console.ReadLine()!;

        Console.Write("Ange modell: ");
        var model = Console.ReadLine()!;

        Console.Write("Ange årsmodell: ");
        int year = Convert.ToInt16(Console.ReadLine());

        Console.Write("Är bilen till salu? (J/N): ");
        var svar = Convert.ToChar(Console.Read());
        Console.ReadLine();
        var forSale = char.ToUpper(svar) == 'J';

        vehicles.Add(new Car(regNr, make, model, year, forSale));

        Console.WriteLine("\n- Alla fordon (Car + Lorry) -");
        Console.WriteLine("Nr\tRegNr\tMärke\tModell\tÅr\t\tTill salu\tÖvrigt");
        Console.WriteLine(new string('-', 90));

        for (var i = 0; i < vehicles.Count; i++)
            Console.WriteLine($"{i + 1}{vehicles[i].ToStringList()}");

        Console.WriteLine("\nTryck på en tangent för att avsluta...");
        Console.ReadKey();
    }
}