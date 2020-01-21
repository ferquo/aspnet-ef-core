using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.Exceptions;
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

        public async Task Delete(int id)
        {
            if (!await authorRepository.Exists(id))
            {
                throw new NotFoundException("Author", id);
            }
            await authorRepository.Delete(id);
        }
    }
}
