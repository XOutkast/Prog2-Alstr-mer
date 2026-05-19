namespace m05.Vehicle;

// Vehicle - en abstrakt klass för alla fordon
public abstract class Vehicle
{
    // Värde för fordonerna
    protected Vehicle(string regNr, string make, string model, int year, bool forSale)
    {
        RegNr = regNr;
        Make = make;
        Model = model;
        Year = year < 1900 ? -1 : year;
        ForSale = forSale;
    }

    public string RegNr { get; set; } // Spara RegNr
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }

    public bool ForSale { get; set; } // Ange om det är till salu eller inte?

    // Returnera år som text eller visa felmeddelande när det är ogiltigt år
    public string YearText => Year == -1 ? "Invalid year" : Year.ToString();

    // Kräv att underklasser ange fordonstyp (Alltså fordonstyp ska hämtas från car or lorry)
    public abstract string VehicleType { get; }
}