using System;

namespace lab_7
{

    delegate double Operation(double a, double b);
    delegate double Power(double a, double b);
    class Program
    {
        static double Addition(double a, double b)
        {
            return a + b;
        }

        static double Mul(double a, double b)
        {
            return a * b;
        }

        static void Main(string[] args)
        {
            Operation add = Addition;
            double result = add.Invoke(3, 5); //<- wywołanie metody addition
            Console.WriteLine(result);

            add = Mul;
            result = add.Invoke(3, 5);
            Console.WriteLine(result);

            Func<double, double, double> Operator = Addition;//<arg1, arg2, return> to jest alternatywny sposób definiowania delegata (w sumie bez definiowania go)
            Func<double, double, double> Div = delegate (double x, double y)
            {
                return x / y;
            };
            Console.WriteLine(Div.Invoke(12, 6));

            Func<int> RandomInt = delegate ()
            {
                return new Random().Next();

            };

            Predicate<int> InRangeFrom0To100 = delegate (int value)//funkcja z jednym argumentem, zawsze zwraca wartosc logiczna
            {
                return value >= 0 && value <= 100;
            };
            Console.WriteLine(InRangeFrom0To100(45));

            Func<int, int, int, bool> InRange = delegate (int value, int min, int max)
            {
                return value >= min && value <= max;
            };

            Action<string> Print = delegate (string s)//przyjmuje dowolna liczbe argumentow, nie ma returna
            {
                Console.WriteLine(s);

            };
            Print.Invoke("abc");

            Operation AddLambda = (a, b) => a + b; //to samo, co Addition, ale bardzo skrocone

            Func<double, double, double> DivLambda = (x, y) =>
            {
                if (y != 0)
                {
                    return x / y;
                }
                else
                {
                    throw new Exception("y is zero");
                }
            };

            Predicate<string> ThreeCharacters = s => s.Length == 3;

            Action<string> PrintUpperLambda = s => Console.WriteLine(s.ToUpper());

        }
    }
}
