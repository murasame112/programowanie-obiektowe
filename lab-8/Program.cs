using System;
using System.Linq;
using System.Collections.Generic;

namespace lab_8
{
    record Student (int Id, string Name, int Ects);
    class Program
    {
        static void Main(string[] args)
        {
            int[] ints = { 4, 6, 7, 3, 2, 8, 9 };
            IEnumerable<int> evenNumbers =
            from n in ints
            where n % 2 == 0
            select n;

            Console.WriteLine(string.Join(", ", evenNumbers));

            IEnumerable<int> oddNumbersHigherThan5 =
            from n in ints
            where n > 5 && !(n % 2 == 0)
            select n;

            Console.WriteLine(string.Join(", ", oddNumbersHigherThan5));
            Console.WriteLine();

            Predicate<int> intPredicate = n =>
            {
                //Console.WriteLine("Wywołanie predykatu dla " + n);
                return n % 2 == 0;
            };

            evenNumbers = from n in ints
                          where intPredicate.Invoke(n)
                          select n;
            evenNumbers =
                from n in evenNumbers
                where n > 5
                select n;

            Console.WriteLine(string.Join(", ", evenNumbers));
            Console.WriteLine(evenNumbers.Sum());
            Console.WriteLine();

            Console.WriteLine("ilosc parzystych wiekszych od 5: {0}", evenNumbers.Count());
            Console.WriteLine("srednia: {0}", evenNumbers.Average());
            Console.WriteLine("najwieksza: {0}", evenNumbers.Max());
            Console.WriteLine("najmniejsza: {0}", evenNumbers.Min());
            Console.WriteLine();

            Student[] students =
            {
                new Student(1, "Ewa", 67),
                new Student(2, "Karol", 67),
                new Student(4, "Ewa", 63),
                new Student(7, "Ania", 67),
                new Student(5, "Karol", 37),
            };

            IEnumerable<string> enumerable =
                from s in students
                orderby s.Name descending
                orderby s.Ects
                select s.Name + " " + s.Ects;
            Console.WriteLine(string.Join("\n", enumerable));

            Console.WriteLine(string.Join(", ",
                from i in ints
                orderby i descending
                select i
                ));

            Console.WriteLine(string.Join(", ", ints.OrderByDescending(i => i)));
            Console.WriteLine();
            Console.WriteLine(string.Join(", ", students.OrderBy(s => s.Name).ThenBy(s => s.Ects)));
            Console.WriteLine();
            IEnumerable<IGrouping<string, Student>> studentNameGroup =
                from s in students
                group s by s.Name;

            foreach (var item in studentNameGroup)
            {
                Console.WriteLine(item.Key + " " + string.Join(", ", item));
            }

            IEnumerable<(string Key, int)> nameCounters =
                    from s in students
                    group s by s.Name into groupItem
                    select (groupItem.Key, groupItem.Count());

            Console.WriteLine(string.Join(", ", nameCounters));

            string str = "ala ma kota ala lubi kot karol lubi psy";

            IEnumerable<(string Key, int)> strCounters =
                from s in str
                group s by str into groupItem
                select (groupItem.Key, groupItem.Count());

            Console.WriteLine("-----");
            Console.WriteLine(string.Join(", ", strCounters));

            evenNumbers = ints.Where(i => i % 2 == 0).Select(i => i + 2);
            (int Id, string Name) p = students
                .Where(s => s.Ects > 20)
                .OrderBy(s => s.Id)
                .Select(s => (s.Id, s.Name))
                .FirstOrDefault(s => s.Name.StartsWith("E"));

            Console.WriteLine(p);

            int[] powerInts = Enumerable.Range(0, ints.Length)
                .Select(i => ints[i] * ints[i])
                .ToArray();

            Console.WriteLine(string.Join(", ", powerInts));


            Random random = new Random();
            int[] randomNumbersTo100 = Enumerable.Range(0, 100)
                .Select(i => random.Next(10))
                .ToArray();



            int page = 0;
            int size = 3;
            List<Student> pageStudent = students.Skip(page * size).Take(size).ToList();
            Console.WriteLine(string.Join(", ", pageStudent));
            


        }


    }
}
