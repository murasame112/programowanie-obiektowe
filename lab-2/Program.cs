using System;

namespace lab_2
{

    abstract class AbstractMessage
    {
        public string Content { get; init; }
        abstract public void Send();
    }

    class EmailMessage : AbstractMessage
    {
        public string To { get; init; }
        public string From { get; init; }
        public string Subject { get; init; }

        public override void Send()
        {
            Console.WriteLine($"Sending email from {From} with content {Content}");
        }
    }

    class SmsMessage : AbstractMessage
    {
        public string PhoneNumber { get; init; }
        public override void Send()
        {
            Console.WriteLine($"Sending sms to {PhoneNumber} with content {Content}");
        }
    }

    class MessangerMessage : AbstractMessage
    {
        public override void Send()
        {
            Console.WriteLine($"Sending message: {Content}");
        }
    }

    interface IFly
    {
        void Fly();
    }

    interface ISwim
    {
        void Swim();
    }

    interface IFlyAndSwim: IFly, ISwim
    {
        //zawiera zarówno Fly() i Swim()
    }

    class Duck : IFlyAndSwim
    {
        public void Fly()
        {

        }
        public void Swim()
        {

        }

    }

    class Wasp : IFly
    {
        public void Fly()
        {

        }
    }

    interface IEngine
    {

    }

    class Hydroplane : IFlyAndSwim, IEngine
    {
        public void Fly()
        {

        }
        public void Swim()
        {

        }
    }

    interface IAggregate
    {
        IIterator createIterator();

    }

    interface IIterator
    {
        bool HasNext();
        string GetNext();

    }

    class StringAggregate : IAggregate
    {
        internal string[] names;

        public StringAggregate(string[] names)
        {
            this.names = names;
        }
        public IIterator createIterator()
        {
            return new StringIterator(this);
        }
    }

    class StringIterator : IIterator
    {
        private StringAggregate aggregate;
        private int index = 0;
        public StringIterator(StringAggregate aggregate)
        {
            this.aggregate = aggregate;
        }
        public string GetNext()
        {
            return aggregate.names[index++];
        }

        public bool HasNext()
        {
            return aggregate.names.Length > index;
        }
    }

    class ArrayIntAggregate : IAggregate
    {
        internal int[] array = {1, 2, 3, 4, 5};



        
        public IIterator createIterator()
        {
            return new ArrayIntIterator(this);
        }
        
    }

    class ArrayIntIterator: IIterator
    {

        private int _index = 0;
        private int _indexReverse = 0;
       
        private ArrayIntAggregate _aggregate;
        
        public ArrayIntIterator(ArrayIntAggregate aggregate)
        {
            this._aggregate = aggregate;
            _indexReverse = _aggregate.array.Length - 1;
        }
        
        public string GetNext()
        {
            throw new Exception();
        }

        public int GetNextInt()
        {
            return _aggregate.array[_index++];
        }

        public int ReverseIterator()
        {
            
            _index++;
            return _aggregate.array[_indexReverse--];
        }

        public bool HasNext()
        {
            return _aggregate.array.Length > _index;
        }
    }


    class SimpleAggregate : IAggregate
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public IIterator createIterator()
        {
            return new SimpleIterator(this);
        }
    }

    class SimpleIterator : IIterator
    {

        private SimpleAggregate aggregate;
        private int count = 0;

        public SimpleIterator(SimpleAggregate aggregate)
        {
            this.aggregate = aggregate;
        }

        public string GetNext()
        {
            switch (++count)
            {
                case 1:
                    return aggregate.FirstName;
                    
                case 2:
                    return aggregate.LastName;
                    
                default:
                    throw new Exception();
                    
            }
        }

        public bool HasNext()
        {
            return count < 2;
        }
    }

    public abstract class Vehicle
    {
        public double Weight { get; init; }
        public int MaxSpeed { get; init; }
        protected int _mileage;
        public int Mealeage
        {
            get { return _mileage; }
        }
        public abstract decimal Drive(int distance);
        public override string ToString()
        {
            return $"Vehicle{{ Weight: {Weight}, MaxSpeed: {MaxSpeed}, Mileage: {_mileage} }}";
        }
    }

    public abstract class Scooter : Vehicle
    {

    }

    public class ElectricScooter : Scooter
    {
        public int BatteriesLevel { get; set; }
        public int MaxRange = 100;

        public void ChargeBatteries()
        {
            BatteriesLevel= 100;
        }
        public override decimal Drive(int distance)
        {
            if(distance <= BatteriesLevel)
            { 
            _mileage+=distance;
            BatteriesLevel-=distance;
            return (decimal) (distance / (double) MaxSpeed);
            }

            return -1;
        }
    }

    public class KickScooter : Scooter
    {
        public override decimal Drive(int distance)
        {
            return -1;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            AbstractMessage[] messages = new AbstractMessage[4];
            messages[0] = new EmailMessage() { Content = "hello", From = "asdf@bcd.com", To = "ff@dd.com", Subject = "hello!" };
            messages[1] = new SmsMessage() { Content = "hello", PhoneNumber = "333555444" };
            messages[2] = new EmailMessage() { Content = "hi", From = "aasddde@eef.com", To = "sadff@fd.com", Subject = "hi!" };
            messages[3] = new MessangerMessage() { Content = "messanger"};
            int mailCounter = 0;
            foreach(var message in messages)
            {
                message.Send();
                // sposob 1
                /*if(message is EmailMessage)
                {
                    mailCounter ++;
                    EmailMessage email = (EmailMessage) message;
                }*/

                //sposob 2
                EmailMessage email = message as EmailMessage;
                mailCounter += email == null ? 0 : 1;

            }
            Console.WriteLine($"Liczba wysłanych emaili: {mailCounter}");

            IFly[] flyingObjects = new IFly[3];
            flyingObjects[0] = new Duck();
            flyingObjects[1] = new Hydroplane();
            flyingObjects[2] = new Wasp();

            ISwim swimming = flyingObjects[0] as ISwim;
            int swimCounter = 0;
            foreach(IFly s in flyingObjects)
            {
                ISwim amISwimming = s as ISwim;
                swimCounter += amISwimming == null ? 0 : 1;
            }
            Console.WriteLine();
            Console.WriteLine($"Swimming objects in array: {swimCounter}");

            string[] names = { "Adam", "Ewa", "Karol" };

            //Działamy na agregatorach i iteratorach, mimo że nie są jeszcze skonkretyzowane
            IAggregate aggregate;
            //Na obu tych aggregatach kod niżej działa poprawnie, mimo że są różne
            aggregate = new StringAggregate(names);
            aggregate = new SimpleAggregate() { FirstName = "Asdf", LastName = "Ghjk" };


            IIterator iterator = aggregate.createIterator();
            while (iterator.HasNext())
            {
                Console.WriteLine(iterator.GetNext());
            }
            Console.WriteLine();

            
            aggregate = new ArrayIntAggregate();
            ArrayIntIterator intIterator = (ArrayIntIterator)aggregate.createIterator();
            

            Console.WriteLine();
            Console.WriteLine("Cw 3:");

            while (intIterator.HasNext())
            {
                Console.WriteLine(intIterator.ReverseIterator());
            }
            


        }
    }
}


//======================================== notatki 

//-------------------------------------------------------------------------------------------------LAB-1-------------------------------------------------------------------------------------------------

public class PersonProperties
{
    public string FirstName { get; set; }
}

// to to samo co to niżej

public class PersonField
{
    private string FirstName;
    public string getFirstName()
    {
        return FirstName;
    }

    public void setFirstName(string value)
    {
        FirstName = value;
    }
}

// mozna te metody get i set wtedy edytowac, np.
set
{
    if (value.Length >= 2)
    {
        firstName = value;
    }
}

// enum i jego uzycie

public enum Currency
{
    PLN = 1,
    USD = 2,
    EUR = 3
}

public class Money : IEquatable<Money>
{
    private readonly decimal _value;
    private readonly Currency _currency;

    private Money(decimal value, Currency currency)
    {
        _value = value;
        _currency = currency;
    }

}

// statyczna metoda wytwórcza (troche jak konstruktor, ale nazwa dowolna, może zwracać null oraz przed opublikowaniem obiektu można wykonać dodatkowe operacje (konstruktor nie zawsze moze))

public static Money? Of(decimal value, Currency currency)
{
    return value < 0 ? null : new Money(value, currency);
}

// pobieranie samej kwoty

public decimal Value
{
    get { return _value;)}
}

// operatory arytmetyczne - przykład pozwala na pomnożenie obiektu money przez liczbę, co będzie równe z pomnożeniem wartości tego obiektu money przez liczbę

public static Money operator *(Money money, decimal factor)
{
    return new Money(money.Value * factor, money.Currency);
}

// operatory konwersji. implicit to konwersja niejawna, bo rzutujemy na ten sam typ (decimal). explicit to jawna, rzutuje na inny typ (double) i może przez to np. utracić część wartości kwoty

public static implicit operator decimal(Money money)
{
    return money.Value;
}
public static explicit operator double(Money money)
{
    return (double)money.Value;
}

// klasa ze stanem. zasadniczo chodzi o to, że Capacity jest readonly, by jego wartość zmieniać dało się TYLKO I WYŁĄCZNIE za pomocą metody Tank

public class Tank
{
    public readonly int Capacity;
    private int _level;

    public Tank(int capacity)
    {
        Capacity = capacity;
    }
    public int Level
    {
        get
        {
            return _level;
        }

        private set
        {
            if (value < 0 || value > Capacity)
            {
                throw new ArgumentOutOfRangeException();
            }
            _level = value;
        }
    }
}

// własny wyjątek

throw new ArgumentException("Argument cant be non positive!");

// nadpisanie odziedziczonej po klasie Object metody ToString() za pomocą override

public override string ToString()
{
    return $"{_value} {_currency}";
}

// IEquatable. testuje równość danego obiektu z innym obiektem tego typu

public class PersonProperties : IEquatable<PersonProperties>

public bool Equals(PersonProperties other)
{
    return other._firstName.Equals(_firstName) && other.Ects == Ects;
}

public override bool Equals(object obj)
{
    return obj is PersonProperties properties &&
           Ects == properties.Ects &&
           FirstName == properties.FirstName;
}

// IComparable. porównuje bieżący obiekt z argumentem, w celu stwierdzenia który z nich powinien wystąpić wcześniej. służy do sortowania w kolekcjach i tablicach. zwraca liczbę całkowitą - 0 oznacza że obiekty są równe, 1 oznacza że obiekt bieżący powinien wystąpić za argumentem, a -1 że poprzedza argumeent

public class Money : IComparable<Money>


public int CompareTo(Money? other)
{
    int currencyCon = Currency.CompareTo(other.Currency);
    if (currencyCon == 0)
    {
        return Value.CompareTo(other.Value);

    }
    else
    {
        return currencyCon;
    }
}


// metody rozszerzające. służą do wzbogacania istniejących już klas bez przedefiniowania ich. są to metody definiowane jako statyczne, pierwszy argument musi być typu klasy którą rozszerza, do tego wyróżniony słowem this.

public static class MoneyExtension
{
    public static Money ToCurrency(this Money money, Currency currency, decimal course)
    {
        if (money.Currency == currency)
        {
            return Money.Of(money.Value, currency);
        }
        else
        {
            return Money.Of(money.Value / course, currency);
        }
    }
}


//-------------------------------------------------------------------------------------------------LAB-2-------------------------------------------------------------------------------------------------

// klasy abstrakcyjne. nie da sie stworzyć instancji, może posiadać metody abstrakcyjne. służy do tworzenia typu bazowego, na którego podstawie są tworzone typy pochodne

public abstract class Vehicle
{
    public double Weight { get; init; }
    public int MaxSpeed { get; init; }
    protected int _mileage;
    public int Mealeage
    {
        get { return _mileage; }
    }
    public abstract decimal Drive(int distance);
    public override string ToString()
    {
        return $"Vehicle{{ Weight: {Weight}, MaxSpeed: {MaxSpeed}, Mileage: {_mileage} }}";
    }
}

public class Car : Vehicle
{
    public bool isFuel { get; set; }
    public bool isEngineWorking { get; set; }
}

public class Bicycle : Vehicle
{
    public bool isDriver { get; set; }
}

// tablica pojazdów - są w niej i samochody i rowery

Vehicle[] vehicles =
{
    new Bicycle(){Weight = 15, MaxSpeed = 30, isDriver = true},
    new Car(){Weight = 900, MaxSpeed = 120, isFuel = true, isEngineWorking = true},
    new Bicycle(){Weight = 21, MaxSpeed = 40, isDriver = true},
    new Bicycle(){Weight = 19, MaxSpeed = 35, isDriver = true},
    new Car(){Weight = 1200, MaxSpeed = 130, isFuel = true, isEngineWorking = true}
};

int bicycleCounter = 0;
int carCounter = 0;
foreach (var vehicle in vehicles)
{
    if (vehicle is Bicycle)
    {
        bicycleCounter++;
        Bicycle bicycle = (Bicycle)vehicle;
        Console.WriteLine("Czy rower ma kierowcę: " + bicycle.isDriver);
    }
}

// poniżej jakaś zabawa dziedziczeniem czy coś

interface IAggregate
{
    IIterator createIterator();

}

interface IIterator
{
    bool HasNext();
    string GetNext();

}

class StringAggregate : IAggregate
{
    internal string[] names;

    public StringAggregate(string[] names)
    {
        this.names = names;
    }
    public IIterator createIterator()
    {
        return new StringIterator(this);
    }
}

class StringIterator : IIterator
{
    private StringAggregate aggregate;
    private int index = 0;
    public StringIterator(StringAggregate aggregate)
    {
        this.aggregate = aggregate;
    }
    public string GetNext()
    {
        return aggregate.names[index++];
    }

    public bool HasNext()
    {
        return aggregate.names.Length > index;
    }
}

//-------------------------------------------------------------------------------------------------LAB-3-------------------------------------------------------------------------------------------------

// klasy generyczne. pozwalają one "opóźnić" decyzję, jakiego typu będzie dana zmienna. na podstwaie poprzednich klas:

public abstract class Iterator<T>
{
    public abstract T GetNext();
    public abstract bool HasNext();
}

// tu podajemy typ zmiennej - <int>

public sealed class ArrayIntIterator : Iterator<int>
{
    private int _index = 0;
    private ArrayIntAggregate _aggregate;
    public ArrayIntIterator(ArrayIntAggregate aggregate)
    {
        _aggregate = aggregate;
    }
    public override int GetNext()
    {
        return _aggregate.array[_index++];
    }
    public override bool HasNext()
    {
        return _index < _aggregate.array.Length;
    }
}

public abstract class Aggregate<T>
{
    public abstract Iterator<T> CreateIterator();
}

public class ArrayAggregate<T> : Aggregate<T>
{
    internal T[] _array;
    public ArrayAggregate(T[] array)
    {
        _array = array;
    }
    public override Iterator<T> CreateIterator()
    {
        return new AggregatetIterator<T>(this);
    }
}

public sealed class AggregatetIterator<T> : Iterator<T>
{
    private int _index = 0;
    private ArrayAggregate<T> _aggregate;
    public AggregatetIterator(ArrayAggregate<T> aggregate)
    {
        _aggregate = aggregate;
    }
    public override T GetNext()
    {
        return _aggregate._array[_index++];
    }
    public override bool HasNext()
    {
        return _index < _aggregate._array.Length;
    }
}

// przyklad kodu korzystjaacego z tych klas

Aggregate<int> intAggregate = new ArrayAggregate<int>(new[] { 1, 2, 3, 4, 5 });
Aggregate<string> stringAggregate = new ArrayAggregate<string>(new[]{"jeden", "dwa", "trzy",
"cztery", "pięć"});
var aggregate = stringAggregate;
//var aggregate = intAggregate;
for (var iterator = aggregate.CreateIterator(); iterator.HasNext();)
{
    Console.WriteLine(iterator.GetNext());
}

// metody tez moga byc generyczne w klasach, np w postaci metod statycznych

public class Mapper
{
    public static V to<T, V>(ICollection<T> values, V empty, V any)
    {
        return values.Count == 0 ? empty : any;
    }
}


IList<int> numbers = new List<int>() { 1, 2, 3 };
Console.WriteLine(Mapper.to(numbers, "empty", "numbers"));

// krotka - kontener, przechowujący kilka (max 8) typów

var tuple = ("Adam", 5, 'A');
Console.WriteLine($"imię: {tuple.Item1}");
Console.WriteLine($"ects: {tuple.Item2}");
Console.WriteLine($"grupa: {tuple.Item3}");

// inny zapis, znaczy to samo

ValueTuple<string, int, char> studentValueTuple = ValueTuple.Create("Adam", 5, 'A');

// mozna podac nazwy pól, zeby potem wygodniej sie odwolywac

(string name, int ects, char group) student = ("Karol", 6, 'C');
Console.WriteLine($"imię: {student.name}");
Console.WriteLine($"ects: {student.ects}");
Console.WriteLine($"grupa: {student.group}");


//-------------------------------------------------------------------------------------------------LAB-4-------------------------------------------------------------------------------------------------

// typ wyliczeniowy
// jak nie zainicjuję liczb, to będą po kolei liczby calkowite od 0
public enum GamerLevel
{
    NOOB = 1,
    MEDIUM = 20,
    PRO = 40,
    MASTER = 60
}

GamerLevel gamerLevel = GamerLevel.NOOB;

int gamerLevelValue = (int)GamerLevel;

// pobranie nazw wszystkich stałych wyliczenia
foreach (string name in Enum.GetNames<GamerLevel>())
{
    Console.WriteLine(name);
}

// pobranie wszystkich stałych wyliczenia
foreach (string value in Enum.GetNames<GamerLevel>())
{
    Console.WriteLine(name);
}

// wyrażenie switch 

double ocena = degree switch
{
    Degree.A => 5.0,
    Degree.B => 4.5,
    Degree.C or Degree.D => 4.0,
    _ => 3.0 // default
};

// przyklad wyrazenia switch z krotką
public static (int, int) NextPoint(Direction4 direction, (int, int) point, (int, int) screenSize)
{
    (int, int) resultPoint = point;
    resultPoint = direction switch
    {
        Direction4.UP => (resultPoint.Item1, resultPoint.Item2 - 1),
        Direction4.RIGHT => (resultPoint.Item1 + 1, resultPoint.Item2),
        Direction4.DOWN => (resultPoint.Item1, resultPoint.Item2 + 1),
        Direction4.LEFT => (resultPoint.Item1 - 1, resultPoint.Item2),
        _ => (resultPoint.Item1, resultPoint.Item2)
    };
}

// kolejny przykład

string romanNumber = a switch
{
    > 1 and < 4 => string.Concat(Enumerable.Repeat("I", a)),
    4 => "IV",
    5 => "V",
    > 5 and < 9 => "V" + string.Concat(Enumerable.Repeat("I", a - 5)),
    9 => "IX",
    10 => "X",
    _ => ""
};


// rekord. struktura, domyślnie ma niemodyfikowalne pola. można w nich definiować metody, ale nie trzeba nadpisywać Equals, GetHashCode i ToString. służą głównie do tworzenia obiektow zorientowanych na dane


record Student(string Name, int Ects, bool Egzam); // to poza metoda main

//w metodzie main
Student student = new Student("Karol", 12, true);
Console.WriteLine(student);

// Porównanie rekordów polega na testowaniu każdego pola			
Console.WriteLine(student.Equals(new Student("Adam", 45, 'F')));

// mozna je definiowac na wzor klasy
public record Student
{
    public string Name { get; set; }
    public int Points { get; set; }
    public char Egzam { get; set; }
}

// init. init sprawia, ze wartosc jest niemodyfikowalna, ale mozna ja definiowac
// czyli chyba ze mozna dowolnie nadac wartosc danej zmiennej, ale tylko raz. czyyyyyyli w sumie to po prostu const

public class ImmutableStudent
{
    public string Name { get; init; }
    public int Ects { get; init; }
    public char Group { get; set; }
}

//-------------------------------------------------------------------------------------------------LAB-5-------------------------------------------------------------------------------------------------

// W języku C# klasa staje się kolekcją jeśli implementuje interfejs IEnumerator<> lub IEnumerator. Jest to minimalne wymaganie, które daje możliwość przeglądania elementów za pomocą instrukcji foreach.	

// kod enumeratorów z Book i Library, którego w sumie do końca nie rozumiem	
public record Book(string Title, string Author, String Isbn);

class Library : IEnumerable<Book>
{
    internal Book[] _books = {
            new Book("C#", "Freeman", "123"),
            new Book("Asp.NET", "Freeman", "456"),
            null,
            null,
            new Book("Java", "Bloch", "789"),
            null
            };

    public Book this[string isbn] // zwraca książkę po isbn
    {
        get
        {
            foreach (Book book in _books)
            {
                if (book.Isbn.Equals(isbn))
                {
                    return book;
                }
            }
            return null;
        }
    }

    public Book this[int index] // zwraca książkę po indexie
    {
        get
        {
            return _books[index - 1];
        }
        set
        {
            _books[index - 1] = value;
        }
    }

    public IEnumerator<Book> GetEnumerator()
    {
        //return new BookEnumerator(this);
        foreach (Book book in _books)
        {
            if (book != null)
            {
                yield return book;
                // yield sprawia, ze dopiero MoveNext powoduje kolejny obrot pętli, co w sumie zastpeuje cala klase BookEnumerator
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}




class BookEnumerator : IEnumerator<Book>
{
    private Library _library;
    int index = -1;

    public BookEnumerator(Library library)
    {
        _library = library;
    }

    public Book Current // zwraca książkę
    {
        get
        {

            while (_library._books[index] == null && index < _library._books.Length - 1)
            {
                index++;
            }
            return _library._books[index];

        }
    }

    object IEnumerator.Current => Current;

    public void Dispose()
    {

    }

    public bool MoveNext() // sprawdza czy kolejna książka istnieje, czy to koniec kolekcji
    {
        return ++index < _library._books.Length;
    }

    public void Reset()
    {
        index = 0;
    }
}


// w main
Library books = new Library();
IEnumerator<Book> enumerator = books.GetEnumerator();
foreach (Book book in books)
{
    Console.WriteLine(book);
}
Console.WriteLine(books["561"]);
books[3] = new Book("Nowa", "Nowy", "111");
Console.WriteLine(string.Join(", ", books));

// kod Teamu enumeratora, który jest bardziej zrozumiały

class Team : IEnumerable<string>
{
    public string Leader;
    public string ScrumMaster;
    public string Developer;

    public IEnumerator<string> GetEnumerator()
    {
        yield return Leader;        //yield powoduje, ze nie jest zwracana wartosc funkcji, tylko wartosc zwrocona przy pierszym wywolaniu MoveNext().
        yield return ScrumMaster;       // z pomoca yield po prostu po kolei mowimy co ma sie wywolac
        yield return Developer;
        for (int i = 0; i < 5; i++)
        {
            yield return "Vacat";
        }

    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();

    }
}

// w main
Team team = new Team() { Leader = "AA", ScrumMaster = "BB", Developer = "CC" };
foreach (string member in team)
{
    Console.WriteLine(member);
}

// yield. Słowo kluczowe yield sygnalizuje, że metoda, operator lub akcesor get jest iteratorem

IEnumerable<string> names()
{
    yield return "ala";
    yield return "ola";
    yield return "adam";
}


//-------------------------------------------------------------------------------------------------LAB-6-------------------------------------------------------------------------------------------------

// ICollection<T> to interfejs, ktory rozszerza IEnumerable o metody:
// void Add(t item) dodanie elementu do kolekcji
// bool Remove(T item) usunięcie elementu z kolekcji
// bool Contains(T item) sprawdzenie, czy element znajduje się w kolekcji
// void Clear() usunięcie elementów z kolekcji
// void CopyTo(T[] array, int arrayIndex) skopiowanie elementów kolekcji do tablicy

// przykład listy. elementy listy są uporządkowane pod względem kolejności
IList<string> names = new List<string>() { "adam", "ola", "karol" };
Console.WriteLine($"Liczba elementów: {names.Count}");
Console.WriteLine($"Element pod indeksem 2: {names[2]}");
Console.WriteLine($"Pozycja imienia 'ola': {names.IndexOf("ola")}");
names.RemoveAt(1);
Console.WriteLine($"Lista po usunięciu elementu o indeksie 1: {String.Join(", ", names)}");
names.Insert(1, "ewa");
Console.WriteLine($"Lista po wstawieniu elementu na pozycji 1: {String.Join(", ", names)}");


// przykład zbioru. elementy zbioru są unikalne, ale nie są uporządkowane

ISet<string> setA = new HashSet<string>();
setA.Add("ala");
setA.Add("ola");
setA.Add("ewa");
Console.WriteLine($"Zbiór A: {String.Join(", ", setA)}");
string[] names = { "karol", "adam", "ola" };
ISet<String> setB = new SortedSet<string>(names);
Console.WriteLine($"Zbiór B: {String.Join(", ", setB)}");
Console.WriteLine($"Czy zbiór A jest podzbiorem B: {setA.IsSubsetOf(setB)}");
ISet<string> result = new HashSet<string>(setA);
result.IntersectWith(setB);
Console.WriteLine($"Cześć wspólna zbiorów A i B: {String.Join(", ", result)}");
result = new HashSet<string>(setA);
result.UnionWith(setB);
Console.WriteLine($"Połączenie zbiorów A i B: {String.Join(", ", result)}");
result = new HashSet<string>(setA);
result.ExceptWith(setB);
Console.WriteLine($"Zbiór A po usunięciu części wspólnej ze zbiorem B: {String.Join(", ", result)}");

// przykład słownika. elementy są identyfikowane za pomocą kluczy (dowolnego typu). elementy w słowniku to wartość

Dictionary<string, int> numbers = new Dictionary<string, int>();
numbers.Add("one", 1);
numbers.Add("two", 2);
numbers.Add("three", 3);
Console.WriteLine("Zawartość słownika – iterowanie foreach:");
foreach (var item in numbers)
{
    Console.WriteLine(item.Key + " " + item.Value);
}
Console.WriteLine($"Zbiór kluczy: {String.Join(", ", numbers.Keys)}");
Console.WriteLine($"Kolekcja wartości: {String.Join(", ", numbers.Values)}");
Console.WriteLine($"Wartość pod kluczem 'two': {numbers["two"]}");
numbers.Remove("three");
Console.WriteLine($"Słownik po usunięciu wartości o kluczu 'three': {String.Join(", ", numbers)}");

// aby poinformowac kolekcje w jaki sposob sortowac elementy, mozna:
// 1 zdefiniowac w klasie elementu kolekcji metode Comparable
// 2 przekazac do metody sortowania obiekt klasy IComparer, ktory ma zdefiniowana metode porownujaca dwa elementy kolekcji
// przkyład:

class LengthComparer : IComparer<string>
{
    public int Compare(string? x, string? y)
    {
        if (x.Length == y.Length)
        {
            return String.Compare(x, y);
        }
        else
        {
            return x.Length.CompareTo(y.Length);
        }
    }
}

// dla kolekcji wykorzystujacych hashowanie bardzo ważne jest poprawne zdefiniowanie metody Equals i GetHashCode w klasie elementu

class User
{
    public string Name { get; set; }
    public int Points { get; set; }
    public override bool Equals(object? obj)
    {
        Console.WriteLine("Calling Equals");
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((User)obj);
    }
    public override int GetHashCode()
    {
        var hash = HashCode.Combine(Name, Points);
        Console.WriteLine($"Calling GetHashCode, hashCode = {hash}");
        return hash;
    }
    public override string ToString()
    {
        return $"{Name: {Name}, Points: {Points}}";
    }
}


HashSet<User> users = new HashSet<User>();
users.Add(new User { Name = "adam", Points = 10 });
users.Add(new User { Name = "adam", Points = 10 });
users.Add(new User { Name = "adam", Points = 10 });
Console.WriteLine(String.Join(", ", users));

// pierwsze dodanie uzytkownika wywoluje GetHashCode i dodaje go
// drugie dodanie uzytkownika po wywolaniu GetHashCode stwierdzi, ze jego hashCode jest identyczne z poprzenim elementem, wiec wywola Equals. Equals daje wynik pozytywny, wiec element nie jest dodawnay do zbioru. 
// w trzecim przypadku tak samo
// finalnie w naszym zbiorze jest 1 element

//-------------------------------------------------------------------------------------------------LAB-7-------------------------------------------------------------------------------------------------

// delegat. delegaty to zmienne przechowujące funkcje.


// delegat typu bool
delegate bool StringPredicate(String name);

// funkcja wywołująca nasz delegat
List<string> FilterNames(List<string> names, StringPredicate predicate)
{
    List<String> result = new List<string>();
    foreach (var name in names)
    {
        if (predicate.Invoke(name))  // Invoke wywoluje delegat
        {
            result.Add(name);
        }
    }
    return result;
}

// poniżej dwie funkcje, które bedzie mogl zastepowac nasz delegat
bool Only3Letters(String name)
{
    return name.Length == 3;
}

bool StartWithA(string name)
{
    return name.StartsWith("a");
}

List<string> names = new List<string> { "ala", "ola", "karol" };
Console.WriteLine(String.Join(", ", FilterNames(names, Only3Letters))); // ala, ola
Console.WriteLine(String.Join(", ", FilterNames(names, StartWithA))); // ala


// drugi przykład użycia delegatu

delegate double Operation(double a, double b);

static double Addition(double a, double b)
{
    return a + b;
}

static double Mul(double a, double b)
{
    return a * b;
}



Operation add = Addition;
double result = add.Invoke(3, 5); //<- wywołanie metody addition
Console.WriteLine(result); // 8

add = Mul;
result = add.Invoke(3, 5);
Console.WriteLine(result); // 15


// istneją delegaty generyczne
// Func - przyjmuje parametry i zwraca wartość
// Action - przyjmuje parametry i nie zwraca wartości
// Predicate - przyjmuje 1 parametr i zwraca typ logiczny


// Func - przyjmuje do 16 parametrów, a ostatni parametr to zawsze zwracany typ

// przykład 1
Func<double, double, double> Operator = Addition;

// przykład 2
Func<double, double, double> Div = delegate (double x, double y)
{
    return x / y;
};

Console.WriteLine(Div.Invoke(12, 6));

// przykład 3
Func<int> RandomInt = delegate ()
{
    return new Random().Next();

};


// Action - to tak jak Func, ale bez zwracania wartości (czyli jak void)

Action<string> Print = delegate (string s)
{
    Console.WriteLine(s);

};
Print.Invoke("abc");

// Predicate - przyjmuje tylko jeden parametr, a sam delegat zawsze zwraca typ bool

Predicate<int> InRangeFrom0To100 = delegate (int value)
{
    return value >= 0 && value <= 100;
};

Console.WriteLine(InRangeFrom0To100(45));


// lambda. wyrazenie lambda to bardziej zwiezly sposob definiowania delegatow

Operation AddLambda = (a, b) => a + b; //to samo, co Addition, ale bardzo skrocone

Predicate<string> ThreeCharacters = s => s.Length == 3; // lambda predykat, sprawdza czy string ma 3 znaki

Action<string> PrintUpperLambda = s => Console.WriteLine(s.ToUpper());


//-------------------------------------------------------------------------------------------------LAB-8-------------------------------------------------------------------------------------------------

// LINQ - składnia zapytań, jak do SQL, ale w kodzie c#. można używać np na kolekcjach

// zaczynamy od from, potem wszystkie te rzeczy z sql typu where, na koniec select

// przykład linq wybierającego liczby parzyste
int[] ints = { 4, 6, 7, 3, 2, 8, 9 };
IEnumerable<int> evenNumbers =
from n in ints
where n % 2 == 0
select n;

Console.WriteLine(string.Join(", ", evenNumbers));

// przykład z użyciem predykatu
Predicate<int> intPredicate = n =>
{
    return n % 2 == 0;
};

evenNumbers = from n in ints
              where intPredicate.Invoke(n)
              select n;
Console.WriteLine(string.Join(", ", evenNumbers));


// linq obsulugje sporo metod ktore juz stworzono

Console.WriteLine("srednia: {0}", evenNumbers.Average());
Console.WriteLine("najwieksza: {0}", evenNumbers.Max());
Console.WriteLine("najmniejsza: {0}", evenNumbers.Min());


// uzycie orderby

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

// uzycie group by - zlicza ile jest wystapien danego imienia


IEnumerable<(string Key, int)> nameCounters =
                    from s in students
                    group s by s.Name into groupItem
                    select (groupItem.Key, groupItem.Count());

Console.WriteLine(string.Join(", ", nameCounters));

// inny sposob zapisywania zapytan. chyyyyyba nazywa sie fluelent api


(int Id, string Name) p = students
.Where(s => s.Ects > 20)
.OrderBy(s => s.Id)
.Select(s => (s.Id, s.Name))
.FirstOrDefault(s => s.Name.StartsWith("E"));
Console.WriteLine(p);

// kolejny przyklad

int[] powerInts = Enumerable.Range(0, ints.Length)
                .Select(i => ints[i] * ints[i])
                .ToArray();

Console.WriteLine(string.Join(", ", powerInts));

// kolejny przyklad

Random random = new Random();
int[] randomNumbersTo100 = Enumerable.Range(0, 100)
    .Select(i => random.Next(10))
    .ToArray();


//-------------------------------------------------------------------------------------------------LAB-9-------------------------------------------------------------------------------------------------

// do enitty framework z nugeta pobieramy: glowny entity framework core, .tools oraz .sqlite

// tworzę model (lub modele). tu w formie rekordów

public record BookCopy
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string UniqueNumber { get; set; }
}

public record Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int EditionYear { get; set; }
    public int AuthorId { get; set; }

}

public record Author
{
    public int Id { get; set; }
    public string Name { get; set; }


}

// tworzę appDbContext (lub jak sobie ten context nazwę). dziedziczy on po DbContext. są w nim DbSety modeli, oraz mogę zrobic model builder (by od razu na start były rekordy w tabelach). w OnConfiguring piszę gdzie jest baza danych. jeśli chcę zmienić modele, to muszę usunąć bazę (w sensie, usunąć plik) i odpalić program ponownie.

class AppContext : DbContext
{
    public DbSet<BookCopy> BookCopies { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //   d:\\database\\base.db
        optionsBuilder.UseSqlite("DATASOURCE=D:/Users/tomasz.wiesek/database/base.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .ToTable("books")
            .HasData(
            new Book() { Id = 1, AuthorId = 1, EditionYear = 2020, Title = "C#" },
            new Book() { Id = 2, AuthorId = 1, EditionYear = 2021, Title = "Asp.Net" },
            new Book() { Id = 3, AuthorId = 2, EditionYear = 2019, Title = "Data structures" },
            new Book() { Id = 4, AuthorId = 2, EditionYear = 2018, Title = "Web applications" }
            );

        modelBuilder.Entity<Author>()
            .ToTable("authors")
            .HasData(
            new Author() { Id = 1, Name = "Freeman" },
            new Author() { Id = 2, Name = "Bloch" }
            );

        modelBuilder.Entity<BookCopy>()
            .ToTable("book_copies")
            .HasData(
            new BookCopy() { Id = 1, BookId = 1, UniqueNumber = "87654321" },
            new BookCopy() { Id = 2, BookId = 1, UniqueNumber = "12345678" }//,
                                                                            //new BookCopy() { Id = 3, BookId = 2, UniqueNumber = "12435678" },
                                                                            //new BookCopy() { Id = 4, BookId = 3, UniqueNumber = "21345678" },
                                                                            //new BookCopy() { Id = 5, BookId = 3, UniqueNumber = "13245678" }
            );

    }
}

// w main tworzę context

AppContext context = new AppContext();
context.Database.EnsureCreated(); // sprawdza czy jest utworzona baza. jak nie ma, to tworzy


// szukanie po id (chyba)
Console.WriteLine(context.Books.Find(1));

// mozna w tym uzywac linq. tu np szukanie ksiazek ktore maja edition year powyzej 2019
IQueryable<Book> books = from book in context.Books
                         where book.EditionYear > 2019
                         select book;
//Console.WriteLine(string.Join("\n", books));
Console.WriteLine(string.Join("\n", books));

// klasyczny join
var list = from book in context.Books
           join author in context.Authors
           on book.AuthorId equals author.Id
           where book.EditionYear > 2018
           select new { BookAuthor = author.Name, Title = book.Title }; // metoda klasy anonimowej

Console.WriteLine(string.Join("\n", list));

foreach (var item in list)
{
    Console.WriteLine(item.BookAuthor);
}

// fluent api join

list = context.Authors.Join(
            context.Books,
            a => a.Id,
            b => b.AuthorId,
            (a, b) => new { BookAuthor = a.Name, Title = b.Title }
            );


Console.WriteLine(string.Join("\n", list));

// drugi przyklad joina

var listOfCopies = context.Books.Join(
            context.BookCopies,
            b => b.Id,
            c => c.BookId,
            (b, c) => new { UniqueNumber = c.UniqueNumber, Title = b.Title }
            );

// obsluga XML

string xml = "<books>" +
            "<book>" +
                "<id>1</id>" +
                "<title>C#</title>" +
            "</book>" +
            "<book>" +
                "<id>2</id>" +
                "<title>Asp.Net</title>" +
            "</book>" +
            "</books>";

XDocument doc = XDocument.Parse(xml);

var booksId = doc
    .Elements("books")
    .Elements("book")
    .Select(x => new { Id = x.Elements("id").First().Value, Title = x.Elements("title").First().Value });

foreach (var e in booksId)
{
    Console.WriteLine(e);
}

// korzystanie z NBP api - pobieranie danych webclientem i odczytywanie xml'a

WebClient client = new WebClient();
client.Headers.Add("Accept", "application/xml");
xml = client.DownloadString("https://api.nbp.pl/api/exchangerates/tables/c");

XDocument docNbp = XDocument.Parse(xml);

var result = docNbp
    .Elements("ArrayOfExchangeRatesTable")
    .Elements("ExchangeRatesTable")
    .Elements("Rates")
    .Elements("Rate")
    .Select(x => new { Code = x.Element("Code").Value, Bid = x.Element("Bid").Value, Ask = x.Element("Ask").Value });

foreach (var item in result)
{
    Console.WriteLine(item);
}


biblioteki w lab 9

using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Net;