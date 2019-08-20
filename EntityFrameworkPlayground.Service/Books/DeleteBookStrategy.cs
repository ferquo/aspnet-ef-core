using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.Service.Books
{
    public class DeleteBookStrategy : IDeleteBookStrategy
    {
        private readonly IBooksRepository booksRepository;
        private readonly IAuthorRepository authorsRepository;

        public DeleteBookStrategy(
            IBooksRepository booksRepository,
            IAuthorRepository authorsRepository)
        {
            this.booksRepository = booksRepository;
            this.authorsRepository = authorsRepository;
        }

        public async Task<bool> BookExists(int id)
        {
            return await booksRepository.GetById(id) != null;
        }
        public async Task<bool> AuthorExists(int authorId)
        {
            return await authorsRepository.Exists(authorId);
        }

        public async Task Delete(int id)
        {
            await booksRepository.Delete(id);
        }
    }
}
