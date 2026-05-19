namespace DB.Models;
public abstract class Vehicle
{
    protected Vehicle(string regNr, string make, string model, int year, bool forSale)
    {
        RegNr = regNr;
        Make = make;
        Model = model;
        Year = year < 1900 ? -1 : year;
        ForSale = forSale;
    }

    public string RegNr { get; set; }

    public string Make { get; set; }

    public string Model { get; set; }

    public int Year { get; set; }

    public bool ForSale { get; set; }

    public string YearText => Year == -1 ? "Felaktigt ar" : Year.ToString();

    public abstract string VehicleType { get; }
}
