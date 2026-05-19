using System.Data.SQLite;
using DB.Models;

namespace DB.Data;
public class CarRepository
{
    private readonly string _connectionString;
    private const string DbFileName = "car.db";

    public CarRepository()
    {
        string dbPath = ResolveDbPath();
        _connectionString = $"Data Source={dbPath};Version=3;";
        InitializeDatabase();
    }

    private static string ResolveDbPath()
    {
        var currentDir = new DirectoryInfo(AppContext.BaseDirectory);

        while (currentDir is not null)
        {
            var projectFile = Path.Combine(currentDir.FullName, "DB.csproj");
            if (File.Exists(projectFile))
            {
                return Path.Combine(currentDir.FullName, DbFileName);
            }

            currentDir = currentDir.Parent;
        }

        return Path.Combine(AppContext.BaseDirectory, DbFileName);
    }

    private void InitializeDatabase()
    {
        using (SQLiteConnection connection = new(_connectionString))
        {
            connection.Open();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS car (
                        regNr TEXT NOT NULL PRIMARY KEY UNIQUE,
                        made TEXT NOT NULL,
                        model TEXT NOT NULL,
                        year INTEGER NOT NULL,
                        forSale INTEGER NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS lorry (
                        regNr TEXT NOT NULL PRIMARY KEY UNIQUE,
                        made TEXT NOT NULL,
                        model TEXT NOT NULL,
                        year INTEGER NOT NULL,
                        forSale INTEGER NOT NULL,
                        load INTEGER NOT NULL
                    )";
                command.ExecuteNonQuery();
            }
        }
    }

    public List<Car> GetAllCars()
    {
        var cars = new List<Car>();

        using (SQLiteConnection connection = new(_connectionString))
        {
            connection.Open();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT regNr, made, model, year, forSale FROM car";
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string regNr = reader.GetString(0);
                        string made = reader.GetString(1);
                        string model = reader.GetString(2);
                        int year = reader.GetInt32(3);
                        bool forSale = reader.GetInt32(4) == 1;

                        cars.Add(new Car(regNr, made, model, year, forSale));
                    }
                }
            }
        }

        return cars;
    }

    public List<Lorry> GetAllLorries()
    {
        var lorries = new List<Lorry>();

        using (SQLiteConnection connection = new(_connectionString))
        {
            connection.Open();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT regNr, made, model, year, forSale, load FROM lorry";
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string regNr = reader.GetString(0);
                        string made = reader.GetString(1);
                        string model = reader.GetString(2);
                        int year = reader.GetInt32(3);
                        bool forSale = reader.GetInt32(4) == 1;
                        int load = reader.GetInt32(5);

                        lorries.Add(new Lorry(regNr, made, model, year, forSale, load));
                    }
                }
            }
        }

        return lorries;
    }

    public Car? GetCarByRegNr(string regNr)
    {
        using (SQLiteConnection connection = new(_connectionString))
        {
            connection.Open();
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT regNr, made, model, year, forSale FROM car WHERE regNr = @regNr";
                command.Parameters.AddWithValue("@regNr", regNr);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string readRegNr = reader.GetString(0);
                        string made = reader.GetString(1);
                        string model = reader.GetString(2);
                        int year = reader.GetInt32(3);
                        bool forSale = reader.GetInt32(4) == 1;

                        return new Car(readRegNr, made, model, year, forSale);
                    }
                }
            }
        }

        return null;
    }

    public bool AddCar(Car car)
    {
        try
        {
            using (SQLiteConnection connection = new(_connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        INSERT INTO car (regNr, made, model, year, forSale)
                        VALUES (@regNr, @made, @model, @year, @forSale)";
                    
                    command.Parameters.AddWithValue("@regNr", car.RegNr);
                    command.Parameters.AddWithValue("@made", car.Make);
                    command.Parameters.AddWithValue("@model", car.Model);
                    command.Parameters.AddWithValue("@year", car.Year);
                    command.Parameters.AddWithValue("@forSale", car.ForSale ? 1 : 0);

                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }
    }

    public bool AddLorry(Lorry lorry)
    {
        try
        {
            using (SQLiteConnection connection = new(_connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        INSERT INTO lorry (regNr, made, model, year, forSale, load)
                        VALUES (@regNr, @made, @model, @year, @forSale, @load)";

                    command.Parameters.AddWithValue("@regNr", lorry.RegNr);
                    command.Parameters.AddWithValue("@made", lorry.Make);
                    command.Parameters.AddWithValue("@model", lorry.Model);
                    command.Parameters.AddWithValue("@year", lorry.Year);
                    command.Parameters.AddWithValue("@forSale", lorry.ForSale ? 1 : 0);
                    command.Parameters.AddWithValue("@load", lorry.Load);

                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }
    }

    public bool UpdateCar(Car car)
    {
        try
        {
            using (SQLiteConnection connection = new(_connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        UPDATE car 
                        SET made = @made, model = @model, year = @year, forSale = @forSale
                        WHERE regNr = @regNr";
                    
                    command.Parameters.AddWithValue("@regNr", car.RegNr);
                    command.Parameters.AddWithValue("@made", car.Make);
                    command.Parameters.AddWithValue("@model", car.Model);
                    command.Parameters.AddWithValue("@year", car.Year);
                    command.Parameters.AddWithValue("@forSale", car.ForSale ? 1 : 0);

                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }
    }

    public bool DeleteCar(string regNr)
    {
        try
        {
            using (SQLiteConnection connection = new(_connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM car WHERE regNr = @regNr";
                    command.Parameters.AddWithValue("@regNr", regNr);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }
    }

    public bool DeleteLorry(string regNr)
    {
        try
        {
            using (SQLiteConnection connection = new(_connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM lorry WHERE regNr = @regNr";
                    command.Parameters.AddWithValue("@regNr", regNr);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }
        catch
        {
            return false;
        }
    }
}
