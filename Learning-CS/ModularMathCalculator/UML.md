# Modular Math Calculator - UML

## Project Overview

A console-based modular mathematics calculator system with three main calculator modules:

1. **Calculus Calculator** - Derivatives & Integrals
2. **Geometry Calculator** - Triangles, Squares, & Pythagorean Theorem
3. **Trigonometry Calculator** - Sin, Cos, Tan & Inverse Functions

---

## Class Diagram

```mermaid
classDiagram
    class Calculator {
        <<abstract>>
        + Title : string
        + ShowMenu() void
        # PrintHeader() void
        # Pause()$ void
        # ReadDouble(prompt, value)$ bool
        # ReadInt(prompt, value)$ bool
        # ReadChoice(prompt)$ string
    }

    class CalculusCalculator {
        + Title : string
        + ShowMenu() void
        - DerivativePowerRule() void
        - PolynomialDerivative() void
        - IntegralPowerRule() void
    }

    class GeometryCalculator {
        + Title : string
        + ShowMenu() void
        - Triangle() void
        - Square() void
        - Pythagorean() void
        - RightTriangle() void
        - GeneralTriangle() void
    }

    class TrigonometryCalculator {
        + Title : string
        + ShowMenu() void
        - ForwardTrig() void
        - InverseTrig() void
        - PrintAngle(radians, function)$ void
        - DegreesToRadians(deg)$ double
        - RadiansToDegrees(rad)$ double
    }

    class Program {
        <<entry point>>
        + Main()
        - calculators : List~Calculator~
    }

    Calculator <|-- CalculusCalculator : inherits
    Calculator <|-- GeometryCalculator : inherits
    Calculator <|-- TrigonometryCalculator : inherits
    Program o-- Calculator : registry
```

---

## System Architecture Diagram

```mermaid
graph TD
    A[Application Start] --> B[Program.cs<br/>Calculator Registry Loop]

    B --> C{User Select<br/>Calculator}

    C -->|calculators 0 .ShowMenu| D[CalculusCalculator]
    C -->|calculators 1 .ShowMenu| E[GeometryCalculator]
    C -->|calculators 2 .ShowMenu| F[TrigonometryCalculator]
    C -->|Exit| G[Exit]

    D --> D1{Sub-Menu}
    D1 -->|Power Rule| D2[DerivativePowerRule]
    D1 -->|Polynomial| D3[PolynomialDerivative]
    D1 -->|Integral| D4[IntegralPowerRule]

    E --> E1{Sub-Menu}
    E1 -->|Triangle| E2[Right / General Triangle]
    E1 -->|Square| E3[Square Properties]
    E1 -->|Pythagorean| E4[Find Hypotenuse / Leg]

    F --> F1{Sub-Menu}
    F1 -->|Forward| F2[ForwardTrig]
    F1 -->|Inverse| F3[InverseTrig]

    D2 --> B
    D3 --> B
    D4 --> B
    E2 --> B
    E3 --> B
    E4 --> B
    F2 --> B
    F3 --> B
    G --> H[Application End]
```

---

## Sequence Diagram - User Interaction Flow

```mermaid
sequenceDiagram
    User->>ModularMathCalculator: Start Application
    ModularMathCalculator->>User: Display Main Menu
    User->>ModularMathCalculator: Select Calculator (1-4)

    alt Calculus Selected
        ModularMathCalculator->>CalculusCalculator: ShowMenu()
        CalculusCalculator->>User: Display Calculus Options
        User->>CalculusCalculator: Select Calculation Type
        CalculusCalculator->>CalculusCalculator: Perform Calculation
        CalculusCalculator->>User: Display Result
    else Geometry Selected
        ModularMathCalculator->>GeometryCalculator: ShowMenu()
        GeometryCalculator->>User: Display Geometry Options
        User->>GeometryCalculator: Select Shape & Operation
        GeometryCalculator->>GeometryCalculator: Perform Calculation
        GeometryCalculator->>User: Display Result
    else Trigonometry Selected
        ModularMathCalculator->>TrigonometryCalculator: ShowMenu()
        TrigonometryCalculator->>User: Display Trig Options
        User->>TrigonometryCalculator: Select Function Type
        TrigonometryCalculator->>TrigonometryCalculator: Perform Calculation
        TrigonometryCalculator->>User: Display Result
    end

    User->>ModularMathCalculator: Return to Main Menu or Exit
    ModularMathCalculator->>User: Application Closes
```

---

## Component Diagram

```mermaid
graph LR
    subgraph Entry["Program.cs — Entry Point"]
        D["<b>Calculator Registry</b><br/>List&lt;Calculator&gt;<br/>Menu loop + polymorphic dispatch"]
    end

    subgraph Core["Core/ — Shared Contract"]
        Base["<b>Calculator</b> (abstract)<br/>Title, ShowMenu()<br/>PrintHeader, Pause<br/>ReadDouble, ReadInt, ReadChoice"]
    end

    subgraph Calculators["Calculators/ — Concrete Implementations"]
        A["<b>CalculusCalculator</b><br/>Derivatives · Integrals"]
        B["<b>GeometryCalculator</b><br/>Triangles · Squares · Pythagorean"]
        C["<b>TrigonometryCalculator</b><br/>Sin/Cos/Tan · Arcsin/Arccos/Arctan"]
    end

    D -->|polymorphic call| Base
    Base -->|inherited by| A
    Base -->|inherited by| B
    Base -->|inherited by| C
```

---

## Calculus Calculator - Method Details

| Method                   | Purpose                    | Input                    | Output                            |
| ------------------------ | -------------------------- | ------------------------ | --------------------------------- |
| `ShowMenu()`             | Displays calculus sub-menu | User choice              | Routes to appropriate calculation |
| `DerivativePowerRule()`  | Calculates d/dx(x^n)       | Exponent n               | Derivative formula                |
| `PolynomialDerivative()` | Handles multiple terms     | Coefficients & exponents | Sum of derivatives                |
| `IntegralPowerRule()`    | Calculates ∫x^n dx         | Exponent n               | Integral formula with constant    |

### Supported Operations

| Operation             | Formula                               | Example   | Result   |
| --------------------- | ------------------------------------- | --------- | -------- |
| Power Rule Derivative | d/dx(x^n) = n·x^(n-1)                 | d/dx(x³)  | 3x²      |
| Polynomial Derivative | Σ(coefficient × exponent × x^(exp-1)) | d/dx(3x²) | 6x       |
| Power Rule Integral   | ∫x^n dx = x^(n+1)/(n+1) + C           | ∫x² dx    | x³/3 + C |

---

## Geometry Calculator - Method Details

| Method                 | Purpose                        | Inputs               | Outputs                    |
| ---------------------- | ------------------------------ | -------------------- | -------------------------- |
| `ShowMenu()`           | Displays geometry sub-menu     | User choice          | Routes to shape calculator |
| `TriangleCalculator()` | Calculates triangle properties | Side lengths or legs | Area, perimeter, angles    |
| `SquareCalculator()`   | Calculates square properties   | Side length          | Area, perimeter, diagonal  |
| `PythagoreanTheorem()` | Applies a² + b² = c²           | Two of three sides   | Missing side               |

### Triangle Calculations

#### Right Triangle (when 2 legs known)

| Property   | Formula                  | Validation                 |
| ---------- | ------------------------ | -------------------------- |
| Area       | (leg₁ × leg₂) / 2        | Both legs must be positive |
| Hypotenuse | √(leg₁² + leg₂²)         | Computed from legs         |
| Perimeter  | leg₁ + leg₂ + hypotenuse | Sum of all sides           |

#### General Triangle (when 3 sides known)

| Property           | Formula             | Validation                       |
| ------------------ | ------------------- | -------------------------------- |
| Semi-perimeter (s) | (a + b + c) / 2     | Must satisfy triangle inequality |
| Area (Heron's)     | √[s(s-a)(s-b)(s-c)] | Each factor must be positive     |
| Perimeter          | a + b + c           | Sum of all sides                 |

### Square & Pythagorean Operations

| Shape          | Property         | Formula        |
| -------------- | ---------------- | -------------- |
| Square         | Area             | a²             |
| Square         | Perimeter        | 4a             |
| Square         | Diagonal         | a√2            |
| Right Triangle | Find Hypotenuse  | c = √(a² + b²) |
| Right Triangle | Find Missing Leg | b = √(c² - a²) |

---

## Trigonometry Calculator - Method Details

| Method                     | Purpose                        | Input         | Output                  |
| -------------------------- | ------------------------------ | ------------- | ----------------------- |
| `ShowMenu()`               | Displays trigonometry sub-menu | User choice   | Routes to trig function |
| `CalculateTrigFunctions()` | Computes sin, cos, tan         | Angle (°/rad) | All three ratios        |
| `CalculateInverseTrig()`   | Finds angle from ratio         | Ratio value   | Angle in ° and radians  |

### Forward Trigonometric Functions

| Function | Definition            | Domain                    | Range    |
| -------- | --------------------- | ------------------------- | -------- |
| sin(θ)   | opposite / hypotenuse | All real numbers          | [-1, 1]  |
| cos(θ)   | adjacent / hypotenuse | All real numbers          | [-1, 1]  |
| tan(θ)   | opposite / adjacent   | All real except ±π/2 + nπ | All real |

### Inverse Trigonometric Functions

| Function  | Input Range | Output Range | Use Case                |
| --------- | ----------- | ------------ | ----------------------- |
| arcsin(x) | [-1, 1]     | [-π/2, π/2]  | Find angle from sine    |
| arccos(x) | [-1, 1]     | [0, π]       | Find angle from cosine  |
| arctan(x) | All real    | (-π/2, π/2)  | Find angle from tangent |

### Unit Conversion

- **Degrees to Radians**: radians = degrees × (π/180)
- **Radians to Degrees**: degrees = radians × (180/π)

---

## State Diagram - Main Menu States

```mermaid
stateDiagram-v2
    [*] --> MainMenu

    MainMenu --> CalculusMenu : Select 1
    MainMenu --> GeometryMenu : Select 2
    MainMenu --> TrigonometryMenu : Select 3
    MainMenu --> Exit : Select 4

    CalculusMenu --> Calculation : Choose Operation
    Calculation --> MainMenu : Back

    GeometryMenu --> Calculation : Choose Shape

    TrigonometryMenu --> Calculation : Choose Function

    Exit --> [*]
```

---

## Activity Diagram - Calculation Flow

```mermaid
flowchart TD
    A["User Starts App"] --> B["Display Main Menu"]
    B --> C{"Select Calculator<br/>(1-4)"}

    C -->|1| D["Calculus Module"]
    C -->|2| E["Geometry Module"]
    C -->|3| F["Trigonometry Module"]
    C -->|4| G["Exit"]

    D --> D1{"Choose Operation"}
    D1 -->|Derivative| D2["Input Exponent"]
    D1 -->|Integral| D3["Input Exponent"]
    D2 --> D4["Calculate & Display"]
    D3 --> D4

    E --> E1{"Choose Shape"}
    E1 -->|Triangle| E2["Input Dimensions"]
    E1 -->|Square| E3["Input Side"]
    E1 -->|Pythagorean| E4["Input Sides"]
    E2 --> E5["Calculate & Display"]
    E3 --> E5
    E4 --> E5

    F --> F1{"Choose Function"}
    F1 -->|Forward Trig| F2["Input Angle"]
    F1 -->|Inverse Trig| F3["Input Ratio"]
    F2 --> F4["Calculate & Display"]
    F3 --> F4

    D4 --> H{"Continue?"}
    E5 --> H
    F4 --> H

    H -->|Yes| B
    H -->|No| G
    G --> I["End"]
```

---

## Mathematical References

### Calculus Formulas

**Power Rule for Derivatives**
$$\frac{d}{dx}(x^n) = n \cdot x^{n-1}$$

**Power Rule for Integrals**
$$\int x^n \, dx = \frac{x^{n+1}}{n+1} + C$$

### Geometry Formulas

**Heron's Formula (Triangle Area)**
$$A = \sqrt{s(s-a)(s-b)(s-c)} \quad \text{where} \quad s = \frac{a+b+c}{2}$$

**Pythagorean Theorem**
$$a^2 + b^2 = c^2$$

**Square Diagonal**
$$d = a\sqrt{2}$$

### Trigonometry Formulas

**Basic Ratios**
$$\sin(\theta) = \frac{\text{opposite}}{\text{hypotenuse}}, \quad \cos(\theta) = \frac{\text{adjacent}}{\text{hypotenuse}}, \quad \tan(\theta) = \frac{\text{opposite}}{\text{adjacent}}$$

**Angle Conversion**
$$\text{radians} = \text{degrees} \times \frac{\pi}{180}$$

---

## Use Case Diagram

```mermaid
graph TB
    subgraph User["User"]
        Actor["👤 Calculator User"]
    end

    subgraph System["Modular Calc System"]
        UC1["Calculate Derivative"]
        UC2["Calculate Integral"]
        UC3["Solve Triangle"]
        UC4["Square Properties"]
        UC5["Pythagorean Solve"]
        UC6["Trig Functions"]
        UC7["Inverse Trig"]
        Menu["Navigate Menu"]
    end

    Actor -->|uses| Menu
    Menu -->|leads to| UC1
    Menu -->|leads to| UC2
    Menu -->|leads to| UC3
    Menu -->|leads to| UC4
    Menu -->|leads to| UC5
    Menu -->|leads to| UC6
    Menu -->|leads to| UC7
```

---

## Data Flow Diagram - Level 0

```mermaid
graph LR
    User["👤 User"] -->|Input: Number/Angle| Process["📊 Calculator<br/>Processor"]
    Process -->|Output: Result| User
    Process -->|Validate| Validator["✓ Input<br/>Validator"]
    Validator -->|OK/Error| Process
```

---

## Module Dependencies

```mermaid
graph TB
    Main["Program.cs<br/>ModularMath (top-level)"]

    Base["Core/Calculator.cs<br/>ModularMath.Core"]

    Calc["Calculators/CalculusCalculator.cs<br/>ModularMath.Calculators"]
    Geom["Calculators/GeometryCalculator.cs<br/>ModularMath.Calculators"]
    Trig["Calculators/TrigonometryCalculator.cs<br/>ModularMath.Calculators"]

    Math["System.Math<br/>(.NET Framework)"]

    Main -->|List of Calculator| Base
    Calc -->|extends| Base
    Geom -->|extends| Base
    Trig -->|extends| Base

    Calc -->|uses| Math
    Geom -->|uses| Math
    Trig -->|uses| Math
```

---

## Implementation Technology Stack

| Layer        | Technology     | Details                           |
| ------------ | -------------- | --------------------------------- |
| Language     | C# 10+         | .NET 10.0                         |
| Runtime      | .NET CLR       | Console application               |
| Input/Output | Console I/O    | `Console.Read*`, `Console.Write*` |
| Libraries    | System         | Math, DateTime                    |
| Platform     | Cross-platform | Windows, Linux, macOS via .NET    |

---

## Project Structure

```
ModularMathCalculator/
│
├── Program.cs                              # Entry point — calculator registry & menu loop
│
├── Core/
│   └── Calculator.cs                       # Abstract base class (ModularMath.Core)
│       ├── abstract Title : string         # Each subclass provides its display name
│       ├── abstract ShowMenu()             # Each subclass provides its own menu
│       ├── PrintHeader()                   # Shared — renders title box on screen
│       ├── Pause()                         # Shared — "press any key" prompt
│       ├── ReadDouble(prompt, out value)   # Shared — validated numeric input
│       ├── ReadInt(prompt, out value)      # Shared — validated integer input
│       └── ReadChoice(prompt)             # Shared — raw menu string input
│
├── Calculators/
│   ├── CalculusCalculator.cs              # Extends Calculator (ModularMath.Calculators)
│   ├── GeometryCalculator.cs              # Extends Calculator (ModularMath.Calculators)
│   └── TrigonometryCalculator.cs          # Extends Calculator (ModularMath.Calculators)
│
├── ModularMathCalculator.csproj           # Project configuration
└── UML.md                                 # This documentation
```

---

## Features Matrix

| Feature             | Calculus | Geometry | Trigonometry |
| ------------------- | -------- | -------- | ------------ |
| Menu Navigation     | ✅       | ✅       | ✅           |
| Input Validation    | ✅       | ✅       | ✅           |
| Error Handling      | ✅       | ✅       | ✅           |
| Multiple Operations | ✅       | ✅       | ✅           |
| Formatted Output    | ✅       | ✅       | ✅           |
| Unit Conversion     | −        | −        | ✅           |
| Advanced Formulas   | −        | ✅       | ✅           |

---

## Quality Attributes

| Attribute           | Implementation                                                                           |
| ------------------- | ---------------------------------------------------------------------------------------- |
| **Inheritance**     | All calculators extend `Calculator`; shared behaviour written once in the base class     |
| **Abstraction**     | `Calculator` defines the contract; callers only depend on `Title` and `ShowMenu()`       |
| **Code Reuse**      | `PrintHeader`, `Pause`, `ReadDouble`, `ReadInt`, `ReadChoice` inherited — never repeated |
| **Open/Closed**     | New calculators added by creating a new file + one registry line; nothing else changes   |
| **Separation**      | Entry point, contract, and implementations each live in their own file and namespace     |
| **Maintainability** | Bug in shared input handling? Fix it once in `Calculator.cs`, all modules benefit        |
| **Scalability**     | The registry loop is dynamic — the menu grows automatically when a new class is added    |

---

## Future Enhancement?

- **Phase 1**: Statistics module (mean, median, standard deviation, variance)
- **Phase 2**: Matrix operations (addition, multiplication, determinant)
- **Phase 3**: Complex number calculations
- **Phase 4**: Calculation history & save to file
- **Phase 5**: Graphing / visualization capabilities
- **Phase 6**: Unit conversion system (length, weight, temperature)
- **Phase 7**: Advanced calculus (Taylor series, numerical integration)
