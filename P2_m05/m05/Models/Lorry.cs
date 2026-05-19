namespace m05.Vehicle;

// Skapa klassen Lorry som ärver från vehicle
public class Lorry : Vehicle
{
    // Anropa konstruktoren och skicka data till Vehicle
    // Load, sätt lastbilens kapacitet
    public Lorry(string regNr, string make, string model, int year, bool forSale, int load)
        : base(regNr, make, model, year, forSale)
    {
        Load = load;
    }

    // Lastbilens last
    public int Load { get; set; }

    // Ange fordonstyp som "Lastbil"
    public override string VehicleType => "Lastbil";
}