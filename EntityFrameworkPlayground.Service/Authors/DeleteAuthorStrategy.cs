using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.Service.Authors
{
    public class DeleteAuthorStrategy : IDeleteAuthorStrategy
    {
        private readonly IAuthorRepository authorRepository;

        public DeleteAuthorStrategy(
            IAuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository;
        }

        public async Task<bool> Exists(int id)
        {
            return await authorRepository.Exists(id);
        }

        public async Task Delete(int id)
        {
            await authorRepository.Delete(id);
        }
    }
}
