namespace DB.Models;
public class Lorry : Vehicle
{
    public Lorry(string regNr, string make, string model, int year, bool forSale, int load)
        : base(regNr, make, model, year, forSale)
    {
        Load = load;
    }

    public int Load { get; set; }

    public override string VehicleType => "Lastbil";
}
