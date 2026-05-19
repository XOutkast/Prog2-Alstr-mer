namespace CarDemo;

internal class Program
{
    // Huvudprogram
    private static void Main(string[] args)
    {
        // Skapa lista
        var carList = new List<Car>();
        // Deklarera val variabel
        char menuSelection;

        // Lägg till test-bilar
        carList = AddCarsAtStart();

        // Huvudloop
        do
        {
            // Visa meny och läs val
            menuSelection = Menu(carList);
            // Hantera val
            switch (menuSelection)
            {
                case '1':
                    // Lägg till bil
                    carList = AddCar(carList);
                    break;
                case '2':
                    // Lista bilar
                    PrintList(carList);
                    break;
                case '3':
                    // Ta bort bil
                    carList = RemoveCar(carList);
                    break;
                case '4':
                    // Töm lista
                    carList = EmptyList(carList);
                    break;
                case '0':
                    // Avsluta
                    Console.WriteLine("\nAvslutar programmet. Tack och hej!");
                    break;
                default:
                    // Ogiltigt val
                    Console.WriteLine("\nOgiltigt val, försök igen.");
                    break;
            }

            // Vänta på tangent
            if (menuSelection != '0')
            {
                Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
            }
        } while (menuSelection != '0'); // Loopa tills avslut
    }

    // Visa meny
    private static char Menu(List<Car> carList)
    {
        // Rensa skärm
        Console.Clear();
        // Skriv meny
        Console.WriteLine("#######################################");
        Console.WriteLine("     BILREGISTER - HUVUDMENY           ");
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine($"  Antal bilar i systemet: {carList.Count}");
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine("  1. Lägg till en bil");
        Console.WriteLine("  2. Lista alla bilar");
        Console.WriteLine("  3. Ta bort en bil");
        Console.WriteLine("  4. Töm listan");
        Console.WriteLine("  0. Avsluta");
        Console.WriteLine("##########################################");
        Console.Write("\nAnge ditt val: ");

        // Läs och returnera val
        var choice = Console.ReadKey().KeyChar;
        Console.WriteLine();
        return choice;
    }

    // Lägg till bil
    private static List<Car> AddCar(List<Car> carList)
    {
        Console.WriteLine("\n--- Lägg till ny bil ---");

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

        // Skapa bil
        var newCar = new Car(regNr, make, model, year, forSale);
        // Lägg till i lista
        carList.Add(newCar);
        Console.WriteLine("\n Bil tillagd!");
        return carList;
    }

    // Lista alla bilar
    private static void PrintList(List<Car> carList)
    {
        // Skriv header
        Console.WriteLine("\n" + "#################################################################################");
        Console.WriteLine("                            LISTA ÖVER BILAR                                ");
        Console.WriteLine("" + "##################################################################################");

        // Kolla om listan är tom
        if (carList.Count == 0)
        {
            Console.WriteLine("\nIngen bil finns registrerad i systemet.");
            return;
        }

        // Skriv rubrik
        Console.WriteLine("\n{0,-5} {1,-10} {2,-15} {3,-15} {4,-20} {5,-10}", "Nr", "RegNr", "Märke", "Modell",
            "Årsmodell", "Till salu");
        Console.WriteLine(new string('-', 80));

        // Skriv varje bil
        for (var i = 0; i < carList.Count; i++) Console.WriteLine("{0,-5} {1}", i + 1, carList[i].ToStringList());

        // Skriv total
        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"Totalt antal bilar: {carList.Count}");
    }

    // Ta bort bil
    private static List<Car> RemoveCar(List<Car> carList)
    {
        Console.WriteLine("\n--- Ta bort bil ---");

        // Kolla om listan är tom
        if (carList.Count == 0)
        {
            Console.WriteLine("\nIngen bil finns att ta bort.");
            return carList;
        }

        // Visa listan
        PrintList(carList);

        // Läs val
        Console.Write("\nAnge nummer på bilen du vill ta bort (0 ångrar): ");
        int choice = Convert.ToInt16(Console.ReadLine());

        // Avbryt
        if (choice == 0)
        {
            Console.WriteLine("Ingen bil togs bort.");
            return carList;
        }

        // Ta bort bil
        if (choice > 0 && choice <= carList.Count)
        {
            carList.RemoveAt(choice - 1);
            Console.WriteLine("\n Bil borttagen!");
        }
        else
        {
            Console.WriteLine("\n Ogiltigt val.");
        }

        return carList;
    }

    // Töm lista
    private static List<Car> EmptyList(List<Car> carList)
    {
        Console.WriteLine("\n--- Töm listan ---");

        // Kolla om listan är tom
        if (carList.Count == 0)
        {
            Console.WriteLine("\nListan är redan tom.");
            return carList;
        }

        // Bekräfta
        Console.Write($"Är du säker på att du vill ta bort alla {carList.Count} bilar? (J/N): ");
        var svar = Convert.ToChar(Console.ReadKey().KeyChar);
        Console.WriteLine();

        // Töm lista
        if (char.ToUpper(svar) == 'J')
        {
            carList.Clear();
            Console.WriteLine("\n Listan är tömd!");
        }
        else
        {
            Console.WriteLine("\nIngen bil togs bort.");
        }

        return carList;
    }

    // Testbilar
    private static List<Car> AddCarsAtStart()
    {
        // Skapa temporär lista
        var tempList = new List<Car>();

        // Lägg till tre bilar
        tempList.Add(new Car("ABC123", "Volvo", "V70", 2012, true));
        tempList.Add(new Car("XYZ789", "Toyota", "Corolla", 2018, false));
        tempList.Add(new Car("DEF456", "BMW", "320i", 2020, true));
        return tempList;
    }
}