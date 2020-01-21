using System.Threading.Tasks;

namespace EntityFrameworkPlayground.Service.Books
{
    public interface IDeleteBookStrategy
    {
        Task Delete(int authorId, int bookId);
    }
}