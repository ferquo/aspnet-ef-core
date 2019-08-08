using EntityFrameworkPlayground.Domain.Entitities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.DataAccess.Repositories.Interfaces
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        IEnumerable<Author> GetAllAuthors();
        Task<Author> GetAuthorById(int id);
    }
}
