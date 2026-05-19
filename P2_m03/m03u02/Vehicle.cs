namespace m03u02;

// Abstrakt basklass för alla fordon
public abstract class Vehicle
{
    // Privata medlem-variabler
    private int year;

    // konstruktor
    public Vehicle()
    {
    }

    // Konstruktor: för alla värden
    public Vehicle(string regNr, string make, string model, int year, bool forSale)
    {
        RegNr = regNr;
        Make = make;
        Model = model;
        Year = year;
        ForSale = forSale;
    }

    // Properties: läsa & ändra fordonsdata
    public string RegNr { get; set; } = "";

    public string Make { get; set; } = "";

    public string Model { get; set; } = "";

    // Validera årtal (år före 1900 ska markeras som felaktigt)
    public int Year
    {
        get => year;
        set
        {
            if (value < 1900)
                year = -1;
            else
                year = value;
        }
    }

    public bool ForSale { get; set; }

    // YearToString()
    public string YearToString()
    {
        if (year == -1) return "felaktigt årtal";

        return Convert.ToString(year);
    }

    // ForSaleToString()
    // Skriva ut om bilen är till salu eller inte
    public string ForSaleToString()
    {
        if (ForSale) return "Bilen är till salu";

        return "Bilen är inte till salu";
    }

    // ToString()
    // Förbered utskrift för bilens info
    public override string ToString()
    {
        return string.Format("\nBilinformation\nReg: {0}, {1} {2} [{3}]\n{4}", RegNr, Make, Model,
            YearToString(), ForSaleToString());
    }

    // ToStringList()
    // Abstrakt metod som förbereder utskrift fär bilens info
    public abstract string ToStringList();
}