using System.Threading.Tasks;

namespace EntityFrameworkPlayground.Service.Authors
{
    public interface IDeleteAuthorStrategy
    {
        Task Delete(int id);
        Task<bool> Exists(int id);
    }
}