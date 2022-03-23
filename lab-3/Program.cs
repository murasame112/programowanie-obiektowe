using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace lab_3
{
    class App
    {
        public static void Main(string[] args)
        {
            //UWAGA!!! Nie usuwaj poniższego wiersza!!!
            //Console.WriteLine("Otrzymałeś punktów: " + (Test.Exercises_1() + Test.Excersise_2() + Test.Excersise_3()));
        }
    }

    //Ćwiczenie 1
    //Zmodyfikuj klasę Musician, aby można było tworzyć muzyków dla T  pochodnych po Instrument.
    //Zdefiniuj klasę  Guitar pochodną po Instrument, w metodzie Play zwróć łańcuch "GUITAR";
    //Zdefiniuj klasę Drum pochodną po Instrument, w metodzie Play zwróć łańcuch "DRUM";
    interface IPlay
    {
        string Play();
    }

    class Musician<T> : IPlay where T: Instrument
    {
        public T Instrument { get; init; }

        public string Play()
        {
            return (Instrument as Instrument)?.Play();
        }

        public override string ToString()
        {
            return $"MUSICIAN with {(Instrument as Instrument)?.Play()}";
        }
    }

    abstract class Instrument : IPlay
    {
        public abstract string Play();
    }

    class Keyboard : Instrument
    {
        public override string Play()
        {
            return "KEYBOARD";
        }
    }

    class Guitar : Instrument
    {
        public override string Play()
        {
            return "GUITAR";
        }

    }

    class Drum : Instrument
    {
        public override string Play()
        {
            return "DRUM";
        }
    }

    //Cwiczenie 2
    public class Exercise2<T>
    {
        //Zmień poniższą metodę, aby zwracała krotkę z polami typu string, int i bool oraz wartościami "Karol", 12 i true
        public static object getTuple1()
        {
            (string, int, bool) obj = ("Karol", 12, true);

            return obj;
        }

        //Zdefiniuj poniższą metodę, aby zwracała krotkę o dwóch polach
        //firstAndLast: z tablicą dwuelementową z pierwszym i ostatnim elementem tablicy input
        //isSame: z wartością logiczną określająca równość obu elementów
        //dla pustej tablicy należy zwrócić krotkę z pustą tablica i wartościa false
        //Przykład
        //dla wejścia
        //int[] arr = {2, 3, 4, 6}
        //metoda powinna zwrócić krotkę
        //var tuple = GetTuple2<int>(arr);
        //tuple.firstAndLast    ==> {2, 6}
        //tuple.isSame          ==> false

        
        public static ValueTuple<T[], bool> GetTuple2(T[] arr)
        {
             T[] firstAndLast = new T[2];
            bool isSame;
            firstAndLast[0] = arr[0];
            firstAndLast[1] = arr[arr.Length - 1];
            if (firstAndLast[0].Equals(firstAndLast[1]))
            {
                isSame = true;
            }
            else
            {
                isSame = false;
            }
            

            ValueTuple<T[], bool> result = ValueTuple.Create(firstAndLast, isSame);

            return result;
        }
        
            

    }
       
        
            
   
    


    //Cwiczenie 3
    public class Exercise3
    {
        //Zdefiniuj poniższa metodę, która zlicza liczbę wystąpień elementów tablicy arr
        //Przykład
        //dla danej tablicy
        //string[] arr = {"adam", "ola", "adam" ,"ewa" ,"karol", "ala" ,"adam", "ola"};
        //wywołanie metody
        //countElements(arr, "adam", "ewa", "ola");
        //powinno zwrócić tablicę krotek
        //{("adam", 3), ("ewa", 1), ("ola", 2)}
        //co oznacza, że "adam" występuje 3 raz, "ewa" 1 raz a "ola" 2
        //W obu tablicach moga pojawić się wartości null, które też muszą być zliczane
        public static (T, int)[] countElements<T>(T?[] arr, params T?[] elements)
        {

            (T, int)[] result = new (T, int)[elements.Length];

            int i = 0;
            foreach (T element in elements)
            {
                int counter = 0;

                foreach (T arrElement in arr)
                {
                    if (element.Equals(arrElement))
                    {
                        counter++;
                    }
                }
                (T, int) resultTuple = (element, counter);
                result[i] = resultTuple;
                i++;
            }

            return result;
            
        }
    }

}
