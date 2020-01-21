using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.Entitities;
using EntityFrameworkPlayground.Domain.Models;
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

        public async Task<bool> Exists(int id)
            => await db.Books.AsNoTracking().AnyAsync(x => x.BookId == id);

        public IEnumerable<Book> GetAllBooks()
            => db.Books.AsNoTracking()
            .Include(x => x.Author)
            .ToList();

        public PagedList<Book> GetAllBooksByAuthor(int authorId, PagingResourceParameters paging)
        {
            var query = db.Books
                .AsNoTracking()
                .Where(x => x.AuthorId == authorId)
                .OrderBy(book => book.Title).AsQueryable();

            if (!string.IsNullOrEmpty(paging.SearchQuery))
            {
                query = query.Where(x => x.Title.ToLowerInvariant().Contains(paging.SearchQuery.ToLowerInvariant()));
            }

            return PagedList<Book>.Create(query, paging.PageNumber, paging.PageSize);
        }

        public async Task<Book> GetbyIdIncludeAuthor(int id)
            => await db.Books
            .Include(x => x.Author)
            .SingleAsync(book => book.BookId == id);

        public async Task AddBookToAuthor(int authorId, Book book)
        {
            book.AuthorId = authorId;
            await Create(book);
        }
    }
}
