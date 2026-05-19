# UML Diagram: Vehicle Arv (m03u01)

## Mermaid Diagram

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

## Notering

- `Vehicle.ToStringList()` är abstrakt
