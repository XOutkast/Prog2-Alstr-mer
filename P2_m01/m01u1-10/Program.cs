// Moment-01. (m01u01 till m01u10)
using System;
using System.Collections.Generic;
using System.Text;

namespace m01u1_10;

internal class Program
{
    private static void Main(string[] args)
    {
        // för svenska tecken (äöå)
        Console.OutputEncoding = Encoding.UTF8;

        char val; // Variabel för menyval
        do // Loop, den ska köras minst en gång
        {
            // Meny val
            Console.Clear();
            Console.WriteLine("Moment 01 - GRUNDLÄGGANDE C# UPPGIFTER");
            Console.WriteLine("\n");
            Console.WriteLine("Välj vilken uppgift du vill köra:\n");
            Console.WriteLine("  1 - m01u01: Utskrifter och escapetecken");
            Console.WriteLine("  2 - m01u02: Variabler och beräkningar");
            Console.WriteLine("  3 - m01u03: Inmatning (Shopping)");
            Console.WriteLine("  4 - m01u04: Selektioner (Skoldag)");
            Console.WriteLine("  5 - m01u05: Switch-meny");
            Console.WriteLine("  6 - m01u06: For-loop");
            Console.WriteLine("  7 - m01u07: While-loop (Råttor)");
            Console.WriteLine("  8 - m01u08: Do-while meny");
            Console.WriteLine("  9 - m01u09: Listor (Tärning)");
            Console.WriteLine("  0 - m01u10: Metoder (Cirkel)");
            Console.WriteLine("\n  Q - Avsluta programmet\n");
            Console.Write("Ditt val: ");

            // Läs och konvertera valet
            val = Console.ReadKey().KeyChar;
            val = char.ToUpper(val); // Gör om till versal
            Console.WriteLine("\n");

            // switches: val för olika uppgifter
            switch (val)
            {
                case '1':
                    Uppgift01();
                    break;
                case '2':
                    Uppgift02();
                    break;
                case '3':
                    Uppgift03();
                    break;
                case '4':
                    Uppgift04();
                    break;
                case '5':
                    Uppgift05();
                    break;
                case '6':
                    Uppgift06();
                    break;
                case '7':
                    Uppgift07();
                    break;
                case '8':
                    Uppgift08();
                    break;
                case '9':
                    Uppgift09();
                    break;
                case '0':
                    Uppgift10();
                    break;
                case 'Q':
                    Console.WriteLine("Programmet avslutas...");
                    break;
                default:
                    Console.WriteLine("Felaktigt val! Tryck på valfri tangent för att försöka igen...");
                    Console.ReadKey();
                    break;
            }

            // Vänta på knapp-tryck innan man går tillbaka till menyn
            if (val != 'Q' && val >= '0' && val <= '9')
            {
                Console.WriteLine("\n\nTryck på valfri tangent för att återgå till menyn...");
                Console.ReadKey();
            }
        } while (val != 'Q'); // Fortsätt tills man väljer Q

        Console.WriteLine("Tack för att du använde programmet!");
    }

    // m01u01
    private static void Uppgift01()
    {
        Console.Clear();
        Console.WriteLine("=== m01u01 - Utskrifter ===\n");

        Console.WriteLine("Jag heter [X] och jag läser programmering på Alströmergymnasiet.");
        Console.WriteLine("Jag tycker att det verkar \"kul\" med programmering."); // \" för dubbelfnutt
        Console.WriteLine("Dessa tre rader\nhar jag skrivit med\nen enda instruktion."); // \n för ny rad
        Console.WriteLine("Följande saker har jag kommenterat i min kod:");
        Console.WriteLine("\t* Hur jag skrev ut dubbelfnuttarna på andra raden"); // \t för tab
        Console.WriteLine("\t* Hur jag kunde skriva ut tre rader med en enda instruktion.");
        Console.WriteLine("\t* Hur jag använde mig av tab, för att göra denna listan.");
    }

    // m01u02
    private static void Uppgift02()
    {
        Console.Clear();
        Console.WriteLine("=== m01u02 - Variabler ===\n");

        // Deklarera och initiera variabler
        var antalPennor = 3;
        var prisPenna = 4.0;
        var viktApple = 0.2; // 200g = 0.2kg
        var prisApplePerKg = 19.0;
        var antalApple = 1;

        // Beräkna summor
        var delsummaPennor = antalPennor * prisPenna;
        var delsummaApple = viktApple * prisApplePerKg;
        var totalt = delsummaPennor + delsummaApple;

        // Metod 1: Konkatenering med +
        Console.WriteLine("1. Med konkatenering:");
        Console.WriteLine("Jag handlade " + antalPennor + " pennor för " + delsummaPennor + "kr och " +
                          antalApple + " äpple för " + delsummaApple.ToString("0.00") + "kr vilket totalt blev " +
                          totalt.ToString("0.00") + "kr.");

        // Metod 2: Bygg sträng först, skriv ut sen
        Console.WriteLine("\n2. Med sträng som byggs upp:");
        var utskrift = "Jag handlade " + antalPennor + " pennor för " + delsummaPennor + "kr och " +
                       antalApple + " äpple för " + delsummaApple.ToString("0.00") + "kr vilket totalt blev " +
                       totalt.ToString("0.00") + "kr.";
        Console.WriteLine(utskrift);

        // Metod 3: Flera Write() utan radbrytning
        Console.WriteLine("\n3. Med Write() utan radbrytning:");
        Console.Write("Jag handlade ");
        Console.Write(antalPennor);
        Console.Write(" pennor för ");
        Console.Write(delsummaPennor);
        Console.Write("kr och ");
        Console.Write(antalApple);
        Console.Write(" äpple för ");
        Console.Write(delsummaApple.ToString("0.00"));
        Console.Write("kr vilket totalt blev ");
        Console.Write(totalt.ToString("0.00"));
        Console.WriteLine("kr.");

        // Metod 4: Placeholder {0}, {1}, etc.
        Console.WriteLine("\n4. Med placeholder {{0}}, {{1}}:");
        Console.WriteLine(
            "Jag handlade {0} pennor för {1}kr och {2} äpple för {3:0.00}kr vilket totalt blev {4:0.00}kr.",
            antalPennor, delsummaPennor, antalApple, delsummaApple, totalt);

        // Metod 5: String interpolation med $
        Console.WriteLine("\n5. Med string interpolation ($):");
        Console.WriteLine(
            $"Jag handlade {antalPennor} pennor för {delsummaPennor}kr och {antalApple} äpple för {delsummaApple:0.00}kr vilket totalt blev {totalt:0.00}kr.");
    }

    // m01u03
    private static void Uppgift03()
    {
        Console.Clear();
        Console.WriteLine("=== m01u03 - Inmatning ===\n");

        // Priserna
        var prisPenna = 4.0;
        var prisApplePerKg = 19.0;
        var viktPerApple = 0.2;

        // Läs in antal
        Console.Write("Hur många pennor vill du köpa? ");
        var antalPennor = Convert.ToInt32(Console.ReadLine()); // string till int

        Console.Write("Hur många äpplen vill du köpa? ");
        var antalApple = Convert.ToInt32(Console.ReadLine());

        // Beräkna totalt
        var delsummaPennor = antalPennor * prisPenna;
        var delsummaApple = antalApple * viktPerApple * prisApplePerKg;
        var totalt = delsummaPennor + delsummaApple;

        Console.WriteLine(
            $"\nJag handlade {antalPennor} pennor för {delsummaPennor:0.00}kr och {antalApple} äpple för {delsummaApple:0.00}kr vilket totalt blev {totalt:0.00}kr.");
    }

    // m01u04
    private static void Uppgift04()
    {
        Console.Clear();
        Console.WriteLine("=== m01u04 - Selektioner ===\n");

        // aktuell tid
        var dt = DateTime.Now;
        var hour = dt.Hour;

        Console.WriteLine($"Klockan är nu: {dt:HH:mm}");
        Console.WriteLine($"Timme: {hour}\n");

        // Kolla om skoldagen pågår, inte börjat eller är slut
        if (hour >= 8 && hour <= 16)
            Console.WriteLine("Skoldagen pågår!");
        else if (hour >= 0 && hour <= 7)
            Console.WriteLine("Skoldagen har inte börjat än.");
        else
            Console.WriteLine("Skoldagen är slut!");
    }

    // m01u05
    private static void Uppgift05()
    {
        Console.Clear();
        Console.WriteLine("=== m01u05 - Switch-meny ===\n");
        Console.WriteLine("Välj ett alternativ:");
        Console.WriteLine("A - Visa din profil");
        Console.WriteLine("B - Räkna ut cirkelarea");
        Console.WriteLine("C - Visa dagens datum");
        Console.WriteLine("D - Visa slumptal");
        Console.WriteLine("E - Om programmet");
        Console.Write("\nVälj (A-E): ");

        var val = Console.ReadKey().KeyChar;
        Console.WriteLine("\n");
        val = char.ToUpper(val); // Gör om till versal

        // olika val för switch
        switch (val)
        {
            case 'A':
                Console.WriteLine("Du valde: Visa din profil");
                Console.WriteLine("Namn: Ingemar XXXXXX");
                Console.WriteLine("Klass: TeXXx");
                break; // Avsluta case
            case 'B':
                Console.WriteLine("Du valde: Räkna ut cirkelarea");
                Console.Write("Ange radie: ");
                var radie = Convert.ToDouble(Console.ReadLine());
                var area = Math.PI * radie * radie; // Area = π × r²
                Console.WriteLine($"Arean är: {area:0.00}");
                break;
            case 'C':
                Console.WriteLine("Du valde: Visa dagens datum");
                Console.WriteLine($"Idag är det: {DateTime.Now:yyyy-MM-dd HH:mm}");
                break;
            case 'D':
                Console.WriteLine("Du valde: Visa slumptal");
                var rand = new Random();
                Console.WriteLine($"Ditt slumptal (1-100): {rand.Next(1, 101)}");
                break;
            case 'E':
                Console.WriteLine("Du valde: Om programmet");
                Console.WriteLine("Detta är ett övningsprogram för C# switch-satser.");
                Console.WriteLine("Version 1.0, skapad 2026-01-26");
                break;
            default: // Om inget case matchar
                Console.WriteLine("Felaktigt val! Du måste välja mellan A-E.");
                break;
        }
    }

    // m01u06
    private static void Uppgift06()
    {
        Console.Clear();
        Console.WriteLine("=== m01u06 - For-loop ===\n");

        var produkt = 1; // Startvärde för multiplikation
        Console.WriteLine("Tal från 1 till 10 i steg om 2:");

        // For-loop: start; villkor; steg
        for (var i = 1; i <= 10; i += 2)
        {
            Console.WriteLine(i);
            produkt *= i; // Multiplicera med alla tal
        }

        Console.WriteLine($"\nProdukten av alla dessa tal är: {produkt}");
        Console.WriteLine($"(1 × 3 × 5 × 7 × 9 = {produkt})");
    }

    // m01u07
    private static void Uppgift07()
    {
        Console.Clear();
        Console.WriteLine("=== m01u07 - While-loop ===\n");

        // Startvärden
        var population = 100;
        var manader = 0;
        var malgrans = 1000000;

        Console.WriteLine($"Startpopulation: {population} råttor");
        Console.WriteLine($"Målgräns: {malgrans} råttor");
        Console.WriteLine("Populationen dubblas varje månad.\n");

        // Körs while-loop så länge villkoret är sant
        while (population < malgrans)
        {
            manader++; // Öka antal månader
            population *= 2; // Dubbla populationen
            Console.WriteLine($"Månad {manader}: {population} råttor");
        }

        Console.WriteLine($"\nDet tar {manader} månader innan populationen når {malgrans} råttor.");
        Console.WriteLine($"Efter {manader} månader finns det {population} råttor.");
    }

    // m01u08
    private static void Uppgift08()
    {
        char val; // Måste deklareras utanför för att användas i villkoret
        do // Körs minst en gång
        {
            Console.Clear();
            Console.WriteLine("=== m01u08 - Do-while meny ===\n");
            Console.WriteLine("Välj ett alternativ:");
            Console.WriteLine("A - Visa din profil");
            Console.WriteLine("B - Räkna ut cirkelarea");
            Console.WriteLine("C - Visa dagens datum");
            Console.WriteLine("D - Visa slumptal");
            Console.WriteLine("E - Om programmet");
            Console.WriteLine("Q - Tillbaka till huvudmenyn");
            Console.Write("\nVälj (A-E eller Q): ");

            val = Console.ReadKey().KeyChar;
            Console.WriteLine("\n");
            val = char.ToUpper(val);

            switch (val)
            {
                case 'A':
                    Console.WriteLine("Du valde: Visa din profil");
                    Console.WriteLine("Namn: Elev Elevsson");
                    Console.WriteLine("Klass: TE22A");
                    break;
                case 'B':
                    Console.WriteLine("Du valde: Räkna ut cirkelarea");
                    Console.Write("Ange radie: ");
                    var radie = Convert.ToDouble(Console.ReadLine());
                    var area = Math.PI * radie * radie;
                    Console.WriteLine($"Arean är: {area:0.00}");
                    break;
                case 'C':
                    Console.WriteLine("Du valde: Visa dagens datum");
                    Console.WriteLine($"Idag är det: {DateTime.Now:yyyy-MM-dd HH:mm}");
                    break;
                case 'D':
                    Console.WriteLine("Du valde: Visa slumptal");
                    var rand = new Random();
                    Console.WriteLine($"Ditt slumptal (1-100): {rand.Next(1, 101)}");
                    break;
                case 'E':
                    Console.WriteLine("Du valde: Om programmet");
                    Console.WriteLine("Detta är ett övningsprogram för C# do-while och switch-satser.");
                    Console.WriteLine("Version 2.0, skapad 2026-01-26");
                    break;
                case 'Q':
                    Console.WriteLine("Återgår till huvudmenyn...");
                    break;
                default:
                    Console.WriteLine("Felaktigt val! Du måste välja mellan A-E eller Q.");
                    break;
            }

            if (val != 'Q')
            {
                Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
            }
        } while (val != 'Q'); // Villkoret kollas sist
    }

    // m01u09
    private static void Uppgift09()
    {
        Console.Clear();
        Console.WriteLine("=== m01u09 - Listor ===\n");

        Console.Write("Hur många tärningsslag vill du göra? ");
        var antalKast = Convert.ToInt32(Console.ReadLine());

        // Skapa en dynamisk lista
        var slag = new List<int>();
        var rand = new Random();

        Console.WriteLine($"\nKastar tärningen {antalKast} gånger...\n");
        // Simulera en tärningskast
        for (var i = 0; i < antalKast; i++)
        {
            var kast = rand.Next(1, 7); // Slumptal 1-6
            slag.Add(kast); // Lägg till i listan
        }

        // Array för att räkna förekomster (index 0 används inte)
        var antal = new int[7];

        // Räkna hur många av varje värde
        foreach (var s in slag) antal[s]++; // Öka räknaren för detta värde

        // Skriva ut statistik
        Console.WriteLine("=== Resultat ===\n");
        for (var i = 1; i <= 6; i++)
        {
            var procent = (double)antal[i] / antalKast * 100;
            Console.WriteLine($"Antal {i}:or: {antal[i],5} st ({procent:0.0}%)");

            // Rita stapel
            Console.Write("  ");
            for (var j = 0; j < antal[i]; j++)
                if (j % 10 == 0 && j > 0)
                    Console.Write("|");
                else
                    Console.Write("█");

            Console.WriteLine();
        }

        Console.WriteLine($"\nTotalt antal kast: {antalKast}");

        // Beräkna medelvärde
        var summa = 0;
        foreach (var s in slag) summa += s;

        var medel = (double)summa / antalKast;
        Console.WriteLine($"Medelvärde: {medel:0.00}");
    }

    // m01u10
    private static void Uppgift10()
    {
        Console.Clear();
        Console.WriteLine("=== m01u10 - Metoder ===\n");

        Console.Write("Ange cirkelns radie: ");
        var radie = Convert.ToDouble(Console.ReadLine());

        // Anropa metod & spara returvärden
        var area = BeraknaArea(radie);
        var omkrets = BeraknaOmkrets(radie);

        // Skriva ut output
        Console.WriteLine($"\nResultat för cirkel med radie {radie}:");
        Console.WriteLine($"Area: {area:0.00} enheter²");
        Console.WriteLine($"Omkrets: {omkrets:0.00} enheter");
    }

    // Metod: Beräknar area (A = π × r²)
    // double = returtyp, radie = parameter
    public static double BeraknaArea(double radie)
    {
        return Math.PI * radie * radie;
    }

    // Metod: Beräknar omkrets (O = 2 × π × r)
    public static double BeraknaOmkrets(double radie)
    {
        return 2 * Math.PI * radie;
    }
}