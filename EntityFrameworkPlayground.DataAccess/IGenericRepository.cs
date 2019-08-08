using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.DataAccess
{
    public interface IGenericRepository<TEntity>
 where TEntity : class
    {
        IEnumerable<TEntity> GetAll();

        Task<TEntity> GetById(int id);

        Task Create(TEntity entity);

        Task Update(int id, TEntity entity);

        Task Delete(int id);
    }
}
