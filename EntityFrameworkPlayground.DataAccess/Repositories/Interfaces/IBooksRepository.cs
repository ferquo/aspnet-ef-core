using EntityFrameworkPlayground.Domain.Entitities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.DataAccess.Repositories.Interfaces
{
    public interface IBooksRepository : IGenericRepository<Book>
    {
        IEnumerable<Book> GetAllBooks();
        IEnumerable<Book> GetAllBooksByAuthor(int authorId);
        Task<Book> GetbyIdIncludeAuthor(int id);
        Task AddBookToAuthor(int authorId, Book book);
    }
}
