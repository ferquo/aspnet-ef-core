using AutoMapper;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.DataTransferObjects;
using EntityFrameworkPlayground.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace EntityFrameworkPlayground.Service.Books
{
    public class GetBooksStrategy : IGetBooksStrategy
    {
        private readonly IBooksRepository booksRepository;
        private readonly ICreateBookLinksStrategy createLinksStrategy;
        private readonly IMapper mapper;

        public GetBooksStrategy(
            IBooksRepository booksRepository,
            ICreateBookLinksStrategy createLinksStrategy,
            IMapper mapper)
        {
            this.booksRepository = booksRepository;
            this.createLinksStrategy = createLinksStrategy;
            this.mapper = mapper;
        }

        public LinkedCollectionResourceWrapperDTO<BookDTO> GetBooks(int authorId, PagingResourceParameters paging)
        {
            var books = booksRepository.GetAllBooksByAuthor(authorId, paging);
            var booksToReturn = mapper.Map<IEnumerable<BookDTO>>(books);
            booksToReturn = booksToReturn.Select(book =>
            {
                book = createLinksStrategy.CreateLinksForBookResource(book);
                return book;
            });

            var wrapper = new LinkedCollectionResourceWrapperDTO<BookDTO>(booksToReturn);
            return createLinksStrategy.CreateLinksForBooks(wrapper, paging, books.HasPrevious, books.HasNext);

        }
    }
}
