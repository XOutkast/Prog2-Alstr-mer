namespace CarDemo;

// Bil model klass
public class Car
{
    // Medlemsvariabler
    private int year;

    // Default-konstruktor
    public Car()
    {
    }

    // Konstruktor med parametrar
    public Car(string regNr, string make, string model, int year, bool forSale)
    {
        RegNr = regNr;
        Make = make;
        Model = model;
        Year = year;
        ForSale = forSale;
    }
    // Properties
    public string RegNr { get; set; } = "";
    public string Make { get; set; } = "";
    public string Model { get; set; } = "";
    


    // Property med validering
    public int Year
    {
        get => year;
        set
        {
            if (value <= DateTime.Now.Year + 1)
                year = value;
            else
                year = -1; // Felaktigt värde
        }
    }

    public bool ForSale { get; set; }

    // Konvertera årtal till text
    public string YearToString()
    {
        if (year == -1) return "felaktigt årtal";
        return $"[{year}]";
    }

    // Konvertera till salu status till text
    public string ForSaleToString()
    {
        if (ForSale) return "Ja";
        return "Nej";
    }
    // ToString metod
    public override string ToString()
    {
        return string.Format("{0, -10} {1, -15} {2, -15} {3, -20} {4, -10}", RegNr, Make, Model,
            YearToString(), ForSaleToString());
    }
}