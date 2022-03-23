using System;

namespace lab_3_zajecia
{
    class StackInt
    {

        private int[] _arr = new int[10];
        private int _last = -1;
        public void Push(int item)
        {
            _arr[++_last] = item;

        }

        public int Pop()
        {
            return _arr[_last--];
        }
    }
    class Student
    {
        public int Egzam { get; set; }
        public T GetReward<T>(T reward)
        {
            if(Egzam > 50)
            {
                return reward;
            }
            else
            {
                return default;
            }
        }
    }


    class Stack<T> /*where T: Student //wskazuje, ze typem moze byc tylko klasa student oraz klasy pochodne*/
    {

        private T[] _arr = new T[10];
        private int _last = -1;
        public void Push(T item)
        {
            _arr[++_last] = item;

        }

        public T Pop()
        {
            return _arr[_last--];
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Stack<int> stackInt = new Stack<int>();
            stackInt.Push(12);
            stackInt.Push(13);
            stackInt.Push(14);
            stackInt.Push(15);
            Console.WriteLine(stackInt.Pop());
            Console.WriteLine(stackInt.Pop());
            Console.WriteLine(stackInt.Pop());
            Console.WriteLine(stackInt.Pop());

            Stack<string> stackStr = new Stack<string>();

            Student student = new Student() { Egzam = 45 };
            Student student2 = new Student() { Egzam = 55 };
            var stringReward = student.GetReward("Gratulacje");
            var intReward = student2.GetReward<decimal>(100); //do GetReward nie musze podawac typu, ale moge to zrobic jesli potrzebuje
            Console.WriteLine(intReward);
            Console.WriteLine();

            ValueTuple<string, decimal, int> product = ValueTuple.Create("laptop", 2000m, 3);

            (string, decimal, int) laptop = ("laptop", 2000m, 3); //tak mozna zrobic bez wprowadzania klasy
            Console.WriteLine(product == laptop);
            Console.WriteLine();

            product.Item1 = "cell phone";
            Console.WriteLine(product == laptop);
            Console.WriteLine();

            (string name, decimal price, int quantity) anotherProduct = laptop;
            Console.WriteLine($"name of product: {anotherProduct.name}");
            Console.WriteLine();

            anotherProduct = (name: "monitor", price: 1200m, quantity: 2); //mimo podania nazw to pola i tak musza byc w tej samej kolejnosci
        }
    }
}
