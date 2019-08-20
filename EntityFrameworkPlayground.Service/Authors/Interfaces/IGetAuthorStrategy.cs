using System.Threading.Tasks;
using EntityFrameworkPlayground.Domain.DataTransferObjects;

namespace EntityFrameworkPlayground.Service.Authors
{
    public interface IGetAuthorStrategy
    {
        Task<AuthorDTO> GetAuthor(int id);
    }
}