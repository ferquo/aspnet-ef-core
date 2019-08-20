using System.Threading.Tasks;

namespace EntityFrameworkPlayground.Service.Books
{
    public interface IDeleteBookStrategy
    {
        Task<bool> AuthorExists(int authorId);
        Task<bool> BookExists(int id);
        Task Delete(int id);
    }
}