using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.Entitities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.DataAccess.Repositories
{
    public class BooksRepository : GenericRepository<Book>, IBooksRepository
    {
        public BooksRepository(BooksContext db)
            : base(db)
        { }

        public IEnumerable<Book> GetAllBooks()
            => db.Books.AsNoTracking()
            .Include(x => x.Author)
            //.Select(book => new {
            //    BookId = book.BookId,
            //    Title = book.Title,
            //    Author = book.Author.Name
            //})
            .ToList();

        public IEnumerable<Book> GetAllBooksByAuthor(int authorId)
            => db.Books.AsNoTracking().Where(x => x.AuthorId == authorId).ToList();

        public async Task<Book> GetbyIdIncludeAuthor(int id)
            => await db.Books
            .Include(x => x.Author)
            //.Select(book => new {
            //    BookId = book.BookId,
            //    Title = book.Title,
            //    Author = book.Author.Name
            //})
            .SingleAsync(book => book.BookId == id);

        public async Task AddBookToAuthor(int authorId, Book book)
        {
            book.AuthorId = authorId;
            await Create(book);
        }
    }
}
