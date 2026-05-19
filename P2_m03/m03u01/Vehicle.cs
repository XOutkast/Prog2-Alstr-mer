namespace m03u01;

public abstract class Vehicle
{
    // Medlemsvariabler
    private int year;

    // Defaultkonstruktor
    public Vehicle()
    {
    }

    // Konstruktor med fem inmatade värden
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
    // Metod som gör om year till en String
    // Inparameter: (inga inparametrar)
    // Returvärde: Den formaterade strängen
    public string YearToString()
    {
        if (year == -1) return "felaktigt årtal";

        return Convert.ToString(year);
    }

    // ForSaleToString()
    // Metod som skriver ut om en bil är till salu eller inte
    // Inparameter: (inga inparametrar)
    // Returvärde: Den formaterade strängen
    public string ForSaleToString()
    {
        if (ForSale) return "Bilen är till salu";

        return "Bilen är inte till salu";
    }

    // ToString()
    // Metod som förbereder utskrift av bilinformation
    // Inparameter: (inga inparametrar)
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
    // Abstrakt metod som förbereder utskrift av bilinformation i listform
    // Inparameter: (inga inparametrar)
    // Returvärde: Den formaterade strängen
    public abstract string ToStringList();
}