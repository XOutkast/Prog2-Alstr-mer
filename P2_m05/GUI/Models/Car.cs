namespace GUI.Models;
public class Car : Vehicle
{
    public Car(string regNr, string make, string model, int year, bool forSale)
        : base(regNr, make, model, year, forSale)
    {
    }

    public override string VehicleType => "Bil";
}