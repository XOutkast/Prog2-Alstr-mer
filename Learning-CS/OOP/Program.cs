using System;

class Test
{
    static void Main()
    {
        // variable
        int age = 42;
        string name = "Johan H";
        double height =1.54 ;

        // print the variable
        Console.WriteLine($"{age} years old, {name} is {height}");
        bool flag = true;

        // math & combining w text
        int a = 5;
        int b = 3;

        int sum = a + b;
        Console.WriteLine(sum);

        Console.Write("Qor da'daada: ");
        int Qor = Convert.ToInt32(Console.ReadLine());

        if (Qor >= 18)
            Console.WriteLine("Waad codeyn kartaa!");
        else
            Console.WriteLine("Ma codeyn kartid!");

        Console.Write("Qor x");
        int x = Convert.ToInt32(Console.ReadLine());

        Console.Write("Qor y");
        int y = Convert.ToInt32(Console.ReadLine());

        int result = Add(x, y);
        Console.WriteLine($"Naatijada waa {result}");
        Console.WriteLine("2-Naatijada waa " + result);
    }
    static int Add(int x, int y)
    {
        return x + y;
    }


}


