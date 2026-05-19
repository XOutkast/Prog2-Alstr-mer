namespace m03u02;

// Klassen Lorrt - ärv från Vehicle
public class Lorry : Vehicle
{
    // Basklass konstruktor
    public Lorry()
    {
    }

    // Konstruktor för 5 värden
    public Lorry(string regNr, string make, string model, int year, bool forSale, int load) : base(regNr, make, model,
        year, forSale)
    {
        Load = load; // max last
    }

    // Properties: för att lösa och ändra maxlast
    public int Load { get; set; }

    // ToStringList()
    // Formaterad sträng för listutskriften, lastbilens info
    public override string ToStringList()
    {
        // till salu?
        string fs;
        if (ForSale)
            fs = "\t\tJA";
        else
            fs = "\t\tNEJ";

        return string.Format("\t{0}\t{1}\t{2}\t[{3}] {4}\tMaxlast: {5}kg.", RegNr, Make, Model,
            YearToString(), fs, Load);
    }

    // ToString()
    // Utskrift för lastbilsobjekt
    public new string ToString()
    {
        var s = base.ToString(); // Hämta basklassens info
        s += string.Format("\nMaxlast: {0}kg.", Load); // lägg till maxlast
        return s;
    }
}