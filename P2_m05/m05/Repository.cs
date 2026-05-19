using System.Data.SQLite;
using m05.Vehicle;

namespace Repository;

// Databasåtkomst för fordon (CRUD & init)
public class Repository
{
    private const string DbFileName = "m05.db";
    private readonly string _connectionString;

    // Sätt connection string & init db
    public Repository()
    {
        // Hitta databasfilen & init db
        var dbPath = ResolveDbPath();
        // Skapa connection string till SQLite-db
        _connectionString = $"Data Source={dbPath};Version=3;";
        InitializeDatabase();
    }

    // Hitta projektmapp & returnera sökväg till databasfilen
    private static string ResolveDbPath()
    {
        var currentDir = new DirectoryInfo(AppContext.BaseDirectory);

        while (currentDir is not null)
        {
            var projectFile = Path.Combine(currentDir.FullName, "m05.csproj");
            // Kontrollera filen
            if (File.Exists(projectFile)) return Path.Combine(currentDir.FullName, DbFileName);
            currentDir = currentDir.Parent;
        }

        // Fallback ifall om projektmapp inte hittas
        // lägg db till där programmet körs
        return Path.Combine(AppContext.BaseDirectory, DbFileName);
    }

    // Skapa tabeller om de inte finns
    private void InitializeDatabase()
    {
        using SQLiteConnection connection = new(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = @"  
            CREATE TABLE IF NOT EXISTS car (
                regNr TEXT NOT NULL PRIMARY KEY UNIQUE,
                make TEXT NOT NULL,
                model TEXT NOT NULL,
                year INTEGER NOT NULL,
                forSale INTEGER NOT NULL
            );

            CREATE TABLE IF NOT EXISTS lorry (
                regNr TEXT NOT NULL PRIMARY KEY UNIQUE,
                make TEXT NOT NULL,
                model TEXT NOT NULL,
                year INTEGER NOT NULL,
                forSale INTEGER NOT NULL,
                load INTEGER NOT NULL
            );
            ";
        command.ExecuteNonQuery();
    }

    // Hämta alla bilar
    public List<Car> GetAllCars()
    {
        var cars = new List<Car>();
        using SQLiteConnection connection = new(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT regNr, make, model, year, forSale FROM car";

        using var reader = command.ExecuteReader();
        while (reader.Read())
            cars.Add(new Car(
                reader.GetString(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetInt32(3),
                reader.GetInt32(4) == 1));
        return cars;
    }

    // Hämta alla lastbilar
    public List<Lorry> GetAllLorries()
    {
        var lorries = new List<Lorry>();
        using SQLiteConnection connection = new(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT regNr, make, model, year, forSale, load FROM lorry";

        using var reader = command.ExecuteReader();
        while (reader.Read())
            // Konvertera db-rad till Lorry-objekt
            lorries.Add(new Lorry(
                reader.GetString(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetInt32(3),
                reader.GetInt32(4) == 1,
                reader.GetInt32(5)));
        return lorries;
    }

    // Lägg till bil i databasen
    public bool AddCar(Car car)
    {
        try
        {
            using SQLiteConnection connection = new(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO car (regNr, make, model, year, forSale)
                VALUES (@regNr, @make, @model, @year, @forSale)";

            command.Parameters.AddWithValue("@regNr", car.RegNr);
            command.Parameters.AddWithValue("@make", car.Make);
            command.Parameters.AddWithValue("@model", car.Model);
            command.Parameters.AddWithValue("@year", car.Year);
            // SQLite har inte bool, så det blir 1/0
            command.Parameters.AddWithValue("@forSale", car.ForSale ? 1 : 0);
            command.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Lägg till lastbil i databasen
    public bool AddLorry(Lorry lorry)
    {
        try
        {
            using SQLiteConnection connection = new(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO lorry (regNr, make, model, year, forSale, load)
                VALUES (@regNr, @make, @model, @year, @forSale, @load)";

            command.Parameters.AddWithValue("@regNr", lorry.RegNr);
            command.Parameters.AddWithValue("@make", lorry.Make);
            command.Parameters.AddWithValue("@model", lorry.Model);
            command.Parameters.AddWithValue("@year", lorry.Year);
            command.Parameters.AddWithValue("@forSale", lorry.ForSale ? 1 : 0);
            command.Parameters.AddWithValue("@load", lorry.Load);
            command.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Ta bort bil via RegNr
    public bool DeleteCar(string regNr)
    {
        try
        {
            using SQLiteConnection connection = new(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM car WHERE regNr = @regNr";
            command.Parameters.AddWithValue("@regNr", regNr);
            command.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Ta bort lastbil via REgNr
    public bool DeleteLorry(string regNr)
    {
        try
        {
            using SQLiteConnection connection = new(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM lorry WHERE regNr = @regNr";
            command.Parameters.AddWithValue("@regNr", regNr);
            command.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
    }
}