using EntityFrameworkPlayground.Domain.Entitities;
using EntityFrameworkPlayground.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.DataAccess.Repositories.Interfaces
{
    public interface IBooksRepository : IGenericRepository<Book>
    {
        IEnumerable<Book> GetAllBooks();
        PagedList<Book> GetAllBooksByAuthor(int authorId, PagingResourceParameters paging);
        Task<Book> GetbyIdIncludeAuthor(int id);
        Task AddBookToAuthor(int authorId, Book book);
    }
}
