using AutoMapper;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.DataTransferObjects;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.Service.Books
{
    public class GetBookStrategy : IGetBookStrategy
    {
        private readonly IBooksRepository booksRepository;
        private readonly ICreateBookLinksStrategy createLinksStrategy;
        private readonly IMapper mapper;

        public GetBookStrategy(
            IBooksRepository booksRepository,
            ICreateBookLinksStrategy createLinksStrategy,
            IMapper mapper)
        {
            this.booksRepository = booksRepository;
            this.createLinksStrategy = createLinksStrategy;
            this.mapper = mapper;
        }

        public async Task<BookDTO> GetBookById(int authorId, int bookId)
        {
            var book = await booksRepository.GetbyIdIncludeAuthor(bookId);
            if (book == null)
            {
                return null;
            }
            return createLinksStrategy.CreateLinksForBookResource(mapper.Map<BookDTO>(book));
        }
    }
}
