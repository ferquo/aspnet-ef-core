using AutoMapper;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.DataTransferObjects;
using EntityFrameworkPlayground.Domain.Entitities;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.Service.Books
{
    public class CreateBookStrategy : ICreateBookStrategy
    {
        private readonly IMapper mapper;
        private readonly IBooksRepository booksRepository;
        private readonly IAuthorRepository authorsRepository;
        private readonly ICreateBookLinksStrategy createLinksStrategy;

        public CreateBookStrategy(
            IMapper mapper,
            IBooksRepository booksRepository,
            IAuthorRepository authorsRepository,
            ICreateBookLinksStrategy createLinksStrategy)
        {
            this.mapper = mapper;
            this.booksRepository = booksRepository;
            this.authorsRepository = authorsRepository;
            this.createLinksStrategy = createLinksStrategy;
        }

        public async Task<bool> AuthorExists(int authorId)
        {
            return await authorsRepository.Exists(authorId);
        }

        public async Task<BookDTO> CreateBook(int authorId, BookForCreationDTO value)
        {
            var bookEntity = mapper.Map<Book>(value);
            await booksRepository.AddBookToAuthor(authorId, bookEntity);
            return createLinksStrategy.CreateLinksForBookResource(mapper.Map<BookDTO>(bookEntity));
        }
    }
}
