namespace m03u01;

public abstract class Vehicle
{
    // Medlemsvariabler
    private int year;

    // Konstruktor
    public Vehicle()
    {
    }

    // Konstruktor för 5 värden
    public Vehicle(string regNr, string make, string model, int year, bool forSale)
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
    // Returvärde: Den formaterade strängen
    public string YearToString()
    {
        if (year == -1) return "felaktigt årtal";

        return Convert.ToString(year);
    }

    // ForSaleToString()
    // Returvärde: Den formaterade strängen
    public string ForSaleToString()
    {
        if (ForSale) return "Bilen är till salu";

        return "Bilen är inte till salu";
    }

    // ToString()
    // Returvärde: Den formaterade strängen
    public override string ToString()
    {
        return string.Format(
            "\nBilinformation\nReg: {0}, {1} {2} [{3}]\n{4}",
            RegNr,
            Make,
            Model,
            YearToString(),
            ForSaleToString()
        );
    }

    // ToStringList()
    // Abstrakt: förbered utskrift av bilinformation i listform
    // Returvärde: Den formaterade strängen
    public abstract string ToStringList();
}