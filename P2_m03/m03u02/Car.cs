namespace m03u02;

// Car: ärv från Vehicle
public class Car : Vehicle
{
    // Basklass konstruktor
    public Car()
    {
    }

    // Konstruktor: tar emot inmatade värder som modell, år osv
    public Car(string regNr, string make, string model, int year, bool forSale) : base(regNr, make, model, year,
        forSale)
    {
    }

    // ToStringList()
    //  Förbered utskrift for bil-info
    public override string ToStringList()
    {
        // Info om bilen
        var s = string.Format("\t{0}\t{1}\t{2}\t[{3}]", RegNr, Make, Model, YearToString());
        // Till salu (JA/NEJ)
        if (ForSale)
            s += "\t\tJA";
        else
            s += "\t\tNEJ";

        return s;
    }
}