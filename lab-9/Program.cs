using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Net;

namespace lab_9
{
    class Program
    {
        static void Main(string[] args)
        {
            AppContext context = new AppContext();
            context.Database.EnsureCreated(); // sprawdza czy jest utworzona baza. jak nie ma, to tworzy

            Console.WriteLine(context.Books.Find(1));
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

            Console.WriteLine("-------------------------------------------");


            var listOfCopies = context.Books.Join(
                context.BookCopies,
                b => b.Id,
                c => c.BookId,
                (b, c) => new { UniqueNumber = c.UniqueNumber, Title = b.Title }
                );

            Console.WriteLine(string.Join("\n", listOfCopies));

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
                .Select( x => new { Id = x.Elements("id").First().Value, Title = x.Elements("title").First().Value});

            foreach(var e in booksId)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("-------------------------NBP API-------------------------");

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

            foreach( var item in result)
            {
                Console.WriteLine(item);
            }

        }
    }

    public record BookCopy
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string UniqueNumber { get; set; }
    }

    public record Book {
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
                new BookCopy() { Id = 1, BookId = 1, UniqueNumber = "87654321"},
                new BookCopy() { Id = 2, BookId = 1, UniqueNumber = "12345678" }//,
                //new BookCopy() { Id = 3, BookId = 2, UniqueNumber = "12435678" },
                //new BookCopy() { Id = 4, BookId = 3, UniqueNumber = "21345678" },
                //new BookCopy() { Id = 5, BookId = 3, UniqueNumber = "13245678" }
                );

        }
    }
}
