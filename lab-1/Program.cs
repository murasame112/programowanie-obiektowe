using System;

namespace lab_1
{
    public class PersonProperties
    {
        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if(value != null && value.Length >= 2)
                {
                    _firstName = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Name's too short");
                }
            }
        }

        private PersonProperties(string name)
        {
            _firstName = name;

        }

        public static PersonProperties OfName(string name)
        {
            if(name != null && name.Length >= 2)
            {
                return new PersonProperties(name);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Name is too short");
            }
        }



    }

    public enum Currency
    {

        
        PLN = 1,
        USD = 2,
        EUR = 3
    }


    
    

    public class Money
    {
        private readonly decimal _value;

        private readonly Currency _currency;
        public Currency Currency
        {
            get { return _currency; }
        }

        public decimal Value
        {
            get { return _value; }
        }

        private Money(decimal value, Currency currency)
        {
            _value = value;
            _currency = currency;   
        }

        public static Money? Of(decimal value, Currency currency)
        {
            return value < 0 ? null : new Money(value, currency);
        }

        public static Money? OfWithException(decimal value, Currency currency)
        {
            if (value < 0 || value == null)
            {
                throw new NullReferenceException("is null");
            }
            else
            {
                return new Money(value, currency);
                
            }
        }

        // money * 4 -> *(money, 4)
        public static Money operator *(Money money, decimal factor)
        {
            return Money.Of(money.Value * factor, money.Currency);
        }

        public static Money operator *(decimal factor, Money money)
        {
            return Money.Of(money.Value * factor, money.Currency);
        }



    }


    class Program
    {
        static void Main(string[] args)
        {
            

            PersonProperties person = PersonProperties.OfName("Artgygy");
            Console.WriteLine(person.FirstName == null ? "NULL":"PERSON");
            Money? money = Money.Of(13, Currency.USD) ?? Money.Of(0, Currency.USD);
            Money result = money * 4;

            Console.WriteLine(money.Value);
            Console.WriteLine(result.Value);
        }
    }
}
