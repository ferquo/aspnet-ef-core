using EntityFrameworkPlayground.Domain.Entitities;
using EntityFrameworkPlayground.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.DataAccess.Repositories.Interfaces
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        IEnumerable<Author> GetAllAuthors();
        PagedList<Author> GetAllAuthors(PagingResourceParameters paging);
        Task<Author> GetAuthorById(int id);
        Task<bool> Exists(int id);
    }
}
