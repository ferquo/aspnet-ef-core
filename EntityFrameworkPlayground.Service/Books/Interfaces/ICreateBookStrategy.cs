using System.Threading.Tasks;
using EntityFrameworkPlayground.Domain.DataTransferObjects;

namespace EntityFrameworkPlayground.Service.Books
{
    public interface ICreateBookStrategy
    {
        Task<bool> AuthorExists(int authorId);
        Task<BookDTO> CreateBook(int authorId, BookForCreationDTO value);
    }
}