namespace m03u01;

public class Lorry : Vehicle
{
    // Medlemsvariabler

    public Lorry()
    {
    }

    // Konstruktor för de 5 inmatade värde
    public Lorry(string regNr, string make, string model, int year, bool forSale, int load)
        : base(regNr, make, model, year, forSale)
    {
        Load = load;
    }

    // Properties
    public int Load { get; set; }

    // ToStringList()
    // förbered utskrift för bilinfo i listform
    public override string ToStringList()
    {
        string fs;
        if (ForSale)
            fs = "\t\tJA";
        else
            fs = "\t\tNEJ";

        return string.Format(
            "\t{0}\t{1}\t{2}\t[{3}] {4}\tMaxlast: {5}kg.",
            RegNr,
            Make,
            Model,
            YearToString(),
            fs,
            Load
        );
    }

    // ToString()
    // Förbered utskrift för lastbilsobjekt
    public new string ToString()
    {
        var s = base.ToString();
        s += string.Format("\nMaxlast: {0}kg.", Load);
        return s;
    }
}