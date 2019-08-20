using System.Threading.Tasks;
using EntityFrameworkPlayground.Domain.DataTransferObjects;

namespace EntityFrameworkPlayground.Service.Books
{
    public interface IGetBookStrategy
    {
        Task<BookDTO> GetBookById(int authorId, int bookId);
    }
}