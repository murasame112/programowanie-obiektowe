using System;

namespace lab_4
{

    enum Degree
    {
        A = 50,
        B = 45,
        C = 40,
        D = 35,
        E = 30,
        F = 20
    }

    record Student(string Name, int Ects, bool Egzam);

    class ClassStudent
    {

    }
    class Program
    {
        static void Main(string[] args)
        {
            Degree degree = Degree.F;
            Console.WriteLine((int)degree);
            string[] vs = Enum.GetNames<Degree>();
            Degree[] degrees = Enum.GetValues<Degree>();
            Console.Write("ocena: ");
            string str = Console.ReadLine();
            try
            {
                Degree studentDegree = Enum.Parse<Degree>(str);
                Console.WriteLine("Ocena: " + studentDegree);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Zla ocena");
            }
            double ocena = degree switch
            {
                Degree.A => 5.0,
                Degree.B => 4.5,
                Degree.C or Degree.D => 4.0,
                _ => 3.0
            };

            Student student = new Student("Karol", 12, true);
            Console.WriteLine(student);
            if(student == new Student("Karol", 12, true))
            {
                Console.WriteLine("identyczni");
            }
            else
            {
                Console.WriteLine("rozni");
            }

            Student[] students =
            {
                new Student("Karol", 12, true),
                new Student("Ewa", 17, false),
                new Student("Robert", 18, true),
                new Student("Ania", 15, false)
            };

            foreach(Student st in students)
            {
                Console.WriteLine(
                    st.Name +
                    st switch
                    {
                        { Ects: >= 17, Egzam: true } => " Zaliczone",
                        _ => " Niezaliczone"
                    }
                    );
            }
        }
    }
}
