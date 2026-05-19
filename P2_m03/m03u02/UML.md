# UML Diagram - Vehicle Arv + Samling (m03u02)

## Mermaid Diagram

```mermaid
classDiagram
    class Vehicle {
        <<abstract>>
        - string regNr
        - string make
        - string model
        - int year
        - bool forSale
        + Vehicle()
        + Vehicle(string regNr, string make, string model, int year, bool forSale)
        + string RegNr
        + string Make
        + string Model
        + int Year
        + bool ForSale
        + string YearToString()
        + string ForSaleToString()
        + string ToString()
        + string ToStringList()
    }

    class Car {
        + Car()
        + Car(string regNr, string make, string model, int year, bool forSale)
        + string ToStringList()
    }

    class Lorry {
        - int load
        + Lorry()
        + Lorry(string regNr, string make, string model, int year, bool forSale, int load)
        + int Load
        + string ToStringList()
        + string ToString()
    }

    Vehicle <|-- Car
    Vehicle <|-- Lorry
```

## Program.cs Funktionalitet

### Samling

- `List<Vehicle> vehicleList` - Lagra både bilar och lastbilar (polymorfism)

### Metoder

- `Menu()` - Visar meny och returnerar val
- `AddVehicle()` - Lägg till bil eller lastbil
- `PrintList()` - Lista alla fordon
- `RemoveVehicle()` - Ta bort ett fordon från listan
- `EmptyList()` - Töm hela listan
- `AddVehiclesAtStart()` - Lägg till testfordon vid start

