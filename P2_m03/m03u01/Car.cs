namespace m03u01;

public class Car : Vehicle
{
    public Car()
    {
    }

    // Konstruktor för de 5 inmatade värde
    public Car(string regNr, string make, string model, int year, bool forSale)
        : base(regNr, make, model, year, forSale)
    {
    }

    // ToStringList()
    // Förbered utskrift av bilinformation i listform
    public override string ToStringList()
    {
        var s = string.Format(
            "\t{0}\t{1}\t{2}\t[{3}]",
            RegNr,
            Make,
            Model,
            YearToString()
        );

        if (ForSale)
            s += "\t\tJA";
        else
            s += "\t\tNEJ";

        return s;
    }
}