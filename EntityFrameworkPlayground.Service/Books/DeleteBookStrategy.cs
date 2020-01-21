using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.Exceptions;
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

        public async Task Delete(int authorId, int bookId)
        {
            if (await authorsRepository.Exists(authorId))
            {
                throw new NotFoundException("Author", authorId);
            }

            var bookToUpdate = await booksRepository.GetById(bookId);
            if (await booksRepository.Exists(bookId))
            {
                throw new NotFoundException("Book", bookId);
            }

            await booksRepository.Delete(bookId);
        }
    }
}
