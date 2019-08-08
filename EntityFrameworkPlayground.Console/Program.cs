using EntityFrameworkPlayground.DataAccess;
using EntityFrameworkPlayground.DataAccess.Repositories;
using EntityFrameworkPlayground.Domain.Entitities;
using System;

namespace EntityFrameworkPlayground.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Heklloi
            Console.WriteLine("Hello World!");
            Console.WriteLine("Hello World! Twice");
            //using (var db = new BooksContext())
            //{
            //    var repo = new BooksRepository(db);
            //    //var newBook = new Book() {
            //    //    Title = "Second Random Programming book",
            //    //    Author = new Author() { Name = "Mary Poppkids" }
            //    //};
            //    //repo.Create(newBook).Wait();

            //    var querriedBooks = repo.GetAllBooks();
            //    foreach (var book in querriedBooks)
            //    {
            //        Console.WriteLine($"{book.BookId} {book.Title}");
            //        Console.WriteLine($"{book.Author.AuthorId} {book.Author.Name}");
            //    }
            //}

            Console.ReadKey();
        }
    }
}
