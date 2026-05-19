namespace m03u02;

internal class Program
{
    // Programstart
    private static void Main(string[] args)
    {
        // Skapa lista - som lagrar alla fordon
        var vehicleList = new List<Vehicle>();
        // variable för menyval
        char menuSelection;

        // Lägg till testfordon
        vehicleList = AddVehiclesAtStart();

        // Loop - som ska avslutas bara när användaren väljer det
        do
        {
            // Visa meny och läsa val
            menuSelection = Menu(vehicleList);
            // Hantera val (switch cases)
            switch (menuSelection)
            {
                case '1': // Lägg till fordon
                    vehicleList = AddVehicle(vehicleList);
                    break;
                case '2': // Lista fordon
                    PrintList(vehicleList);
                    break;
                case '3': // Ta bort fordon
                    vehicleList = RemoveVehicle(vehicleList);
                    break;
                case '4': // Töm lista
                    vehicleList = EmptyList(vehicleList);
                    break;
                case '0': // Avsluta
                    Console.WriteLine("\nAvslutar programmet. Tack och hej!");
                    break;
                default: // Ogiltigt val
                    Console.WriteLine("\nOgiltigt val, försök igen.");
                    break;
            }

            // Vänta på tangent
            if (menuSelection != '0')
            {
                Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
            }
        } while (menuSelection != '0'); // Loopa tills användaren avslutar
    }

    // Visa meny
    private static char Menu(List<Vehicle> vehicleList)
    {
        // Rensa skärm
        Console.Clear();
        // Skriv meny
        Console.WriteLine("#######################################");
        Console.WriteLine("     BILREGISTER - HUVUDMENY           ");
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine($"  Antal fordon i systemet: {vehicleList.Count}");
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine("  1. Lägg till ett fordon (bil/lastbil)");
        Console.WriteLine("  2. Lista alla fordon");
        Console.WriteLine("  3. Ta bort ett fordon");
        Console.WriteLine("  4. Töm listan");
        Console.WriteLine("  0. Avsluta");
        Console.WriteLine("##########################################");
        Console.Write("\nAnge ditt val: ");

        // Läs och returnera val
        var choice = Console.ReadKey().KeyChar;
        Console.WriteLine();
        return choice;
    }

    // Lögg till ett nytt fordon
    private static List<Vehicle> AddVehicle(List<Vehicle> vehicleList)
    {
        Console.WriteLine("\n--- Lägg till ett nytt fordon ---");

        // Välj typ av fordon
        Console.Write("Typ (B)il / (L)astbil: ");
        var typeChoice = char.ToUpper(Console.ReadKey().KeyChar);
        Console.WriteLine();

        // Läs regnr
        Console.Write("Ange registreringsnummer: ");
        var regNr = Console.ReadLine()!;

        // Läs märke
        Console.Write("Ange märke: ");
        var make = Console.ReadLine()!;

        // Läs modell
        Console.Write("Ange modell: ");
        var model = Console.ReadLine()!;

        // Läs årtal
        Console.Write("Ange årsmodell: ");
        int year = Convert.ToInt16(Console.ReadLine());

        // Läs till salu status
        Console.Write("Är bilen till salu? (J/N): ");
        var svar = Convert.ToChar(Console.ReadKey().KeyChar);
        Console.WriteLine();
        var forSale = char.ToUpper(svar) == 'J';

        // Skapar rött objekt - beroende på val
        if (typeChoice == 'L')
        {
            Console.Write("Ange maxlast (kg): ");
            var load = Convert.ToInt32(Console.ReadLine());
            vehicleList.Add(new Lorry(regNr, make, model, year, forSale, load));
            Console.WriteLine("\n Lastbil tillagd!");
        }
        else
        {
            vehicleList.Add(new Car(regNr, make, model, year, forSale));
            Console.WriteLine("\n Bil tillagd!");
        }

        return vehicleList;
    }

    // PrintList()
    // Lista alla fordon i systemet
    private static void PrintList(List<Vehicle> vehicleList)
    {
        // Skriv header
        Console.WriteLine("\n" + "#################################################################################");
        Console.WriteLine("                            LISTA ÖVER FORDON                                ");
        Console.WriteLine("" + "##################################################################################");

        // Kolla om listan är tom
        if (vehicleList.Count == 0)
        {
            Console.WriteLine("\nInget fordon finns registrerat i systemet.");
            return;
        }

        Console.WriteLine("\nNr\tRegNr\tMärke\tModell\tÅr\t\tTill salu\tÖvrigt");
        Console.WriteLine(new string('-', 90));

        // loop igenom alla fordon
        for (var i = 0; i < vehicleList.Count; i++) Console.WriteLine($"{i + 1}{vehicleList[i].ToStringList()}");

        // Skriv total
        Console.WriteLine(new string('-', 90));
        Console.WriteLine($"Totalt antal fordon: {vehicleList.Count}");
    }

    // RemoveVehicle()
    private static List<Vehicle> RemoveVehicle(List<Vehicle> vehicleList)
    {
        Console.WriteLine("\n--- Ta bort fordon ---");

        // Kolla om listan är tom
        if (vehicleList.Count == 0)
        {
            Console.WriteLine("\nInget fordon finns att ta bort.");
            return vehicleList;
        }

        // Visa listan
        PrintList(vehicleList);

        // Läs val
        Console.Write("\nAnge nummer på fordonet du vill ta bort (0 ångrar): ");
        int choice = Convert.ToInt16(Console.ReadLine());

        // Avbryt
        if (choice == 0)
        {
            Console.WriteLine("Inget fordon togs bort.");
            return vehicleList;
        }

        // Ta bort bil
        if (choice > 0 && choice <= vehicleList.Count)
        {
            vehicleList.RemoveAt(choice - 1);
            Console.WriteLine("\n Fordon borttaget!");
        }
        else
        {
            Console.WriteLine("\n Ogiltigt val.");
        }

        return vehicleList;
    }

    // EmptyListan
    private static List<Vehicle> EmptyList(List<Vehicle> vehicleList)
    {
        Console.WriteLine("\n--- Töm listan ---");

        // Kolla om listan är tom
        if (vehicleList.Count == 0)
        {
            Console.WriteLine("\nListan är redan tom.");
            return vehicleList;
        }

        // Bekräfta
        Console.Write($"Är du säker på att du vill ta bort alla {vehicleList.Count} fordon? (J/N): ");
        var svar = Convert.ToChar(Console.ReadKey().KeyChar);
        Console.WriteLine();

        // Töm lista
        if (char.ToUpper(svar) == 'J')
        {
            vehicleList.Clear();
            Console.WriteLine("\n Listan är tömd!");
        }
        else
        {
            Console.WriteLine("\nInget fordon togs bort.");
        }

        return vehicleList;
    }

    // AddVehiclesAtStart
    private static List<Vehicle> AddVehiclesAtStart()
    {
        // Temporär lista
        var tempList = new List<Vehicle>();

        // Lägg till testfordon
        tempList.Add(new Car("ABC123", "Volvo", "V70", 2012, true));
        tempList.Add(new Lorry("LOR123", "Scania", "R500", 2019, true, 18000));
        tempList.Add(new Car("XYZ123", "JEEP", "Cherokee", 2026, false));
        tempList.Add(new Lorry("LOR321", "Volvo", "XC60", 2026, false, 25000));
        return tempList;
    }
}