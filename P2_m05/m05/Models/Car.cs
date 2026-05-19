namespace m05.Vehicle;

// Skapa klassen Car som ärver från Vehicle
public class Car : Vehicle
{
    // Anropa Car-klassens konstruktor
    // Skicka parametrar till Vehicle, klassens konstruktor
    public Car(string regNr, string make, string model, int year, bool forSale)
        : base(regNr, make, model, year, forSale)
    {
    }

    // Ange fordonstyp som "bil"
    public override string VehicleType => "Bil";
}