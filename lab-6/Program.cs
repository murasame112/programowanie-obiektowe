using System;
using System.Collections.Generic;

namespace lab_6
{
    class Student : IComparable<Student>
    {
        public string Name { get; set; }
        public int Ects { get; set; }

        public Student (string name, int ects)
        {
            this.Name = name;
            this.Ects = ects;
        }

        public override bool Equals(object obj)
        {
            Console.WriteLine("Student Equals");
            return obj is Student student &&
                   Name == student.Name &&
                   Ects == student.Ects;
        }

        public override int GetHashCode()
        {
            Console.WriteLine("Student HashCode");
            return HashCode.Combine(Name, Ects);
        }

        public int CompareTo(Student other)
        {
            if( Name.CompareTo(other.Name) == 0)
            {
                return Ects.CompareTo(other.Ects);
            }

            return Name.CompareTo(other.Name);
        }

        public override string ToString()
        {
            return $"Imie = {Name}, Ects = {Ects}"; 
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            ICollection<string> names = new List<string>();
            names.Add("Ewa");
            names.Add("Karol");
            names.Add("Adam");
            foreach(string name in names)
            {
                Console.WriteLine(name);
            }
            Console.WriteLine();

            Console.WriteLine(names.Contains("Adam"));
            Console.WriteLine();

            ICollection<Student> students = new List<Student>();
            students.Add(new Student("Łukasz", 15));
            students.Add(new Student("Anna", 20));
            students.Add(new Student("Kamil", 45));
            students.Remove(new Student("Anna", 0));

           
            
            
            
            Console.WriteLine(students.Contains(new Student("Kamil", 45)));

            List<Student> list = (List<Student>)students;
            Console.WriteLine(list[0]);

            list.Insert(1, new Student("Robert", 45));

            list.RemoveAt(0);

            Console.WriteLine();
            foreach (Student name in students)
            {
                Console.WriteLine(name.Name + " " + name.Ects);
            }
            int index = list.IndexOf(new Student("Kamil", 45));
            Console.WriteLine("Kamil ma pozycję " + index + '\n');

            Console.WriteLine("==SET==");

            ISet<string> set = new HashSet<string>();
            set.Add("Ewa");
            set.Add("Karol");
            set.Add("Adam");
            set.Add("Adam");
            

            foreach(string name in set)
            {
                Console.WriteLine(name);
            }
            Console.WriteLine();



            ISet<Student> studentGroup = new HashSet<Student>();
            Console.WriteLine("Student group:");
            studentGroup.Add(new Student("Łukasz", 15));
            studentGroup.Add(new Student("Anna", 20));
            studentGroup.Add(new Student("Kamil", 45));
            studentGroup.Add(new Student("Kamil", 45));

            foreach (Student name in studentGroup)
            {
                Console.WriteLine(name.Name + " " + name.Ects);
            }


            Console.WriteLine();
            studentGroup.Contains(new Student("Kamil", 45));
            studentGroup.Remove(new Student("Kamil", 45));
            studentGroup = new SortedSet<Student>(studentGroup);

            studentGroup.Add(new Student("Adam", 20));
            studentGroup.Add(new Student("Adam", 35));

            foreach (Student name in studentGroup)
            {
                Console.WriteLine(name.Name + " " + name.Ects);
            }

            studentGroup.Contains(new Student("Kamil", 45));

            Console.WriteLine("==DICTIONARY=="+'\n');

            Dictionary<Student, string> phoneBook = new Dictionary<Student, string>();

            phoneBook[list[0]] = "123123123";
            phoneBook[list[1]] = "456456546";
            phoneBook[list[2]] = "789789789";
            phoneBook[new Student("Jan", 33)] = "234654793";

            Console.WriteLine(phoneBook.Keys);
            if (phoneBook.ContainsKey(new Student("Anna", 20)))
            {
                Console.WriteLine("Ma telefon");
            }
            else
            {
                Console.WriteLine("Nie ma telefonu");
            }

            Console.WriteLine();

            foreach(var item in phoneBook)
            {
                Console.WriteLine(item.Key + " telefon: " + item.Value);
            }


            Console.WriteLine();

            string[] arr = { "adam", "ewa", "karol", "ewa", "ania", "karol", "adam", "adam", "ewa" };

            Dictionary<string, int> counters = new Dictionary<string, int>();

            foreach (string name in arr) {
                if (counters.ContainsKey(name))
                {
                    counters[name] += 1;
                }
                else
                {
                    counters.Add(name, 1);
                }
                
            }

            foreach (var item in counters)
            {
                Console.WriteLine("name: " + item.Key + " count: " + item.Value);
            }



        }
    }
}
