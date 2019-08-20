using System.Threading.Tasks;
using EntityFrameworkPlayground.Domain.DataTransferObjects;

namespace EntityFrameworkPlayground.Service.Authors
{
    public interface ICreateAuthorStrategy
    {
        Task<AuthorDTO> CreateAuthor(AuthorForCreationDTO author);
    }
}