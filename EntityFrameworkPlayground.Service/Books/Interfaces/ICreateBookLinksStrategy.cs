using EntityFrameworkPlayground.Domain.DataTransferObjects;
using EntityFrameworkPlayground.Domain.Models;

namespace EntityFrameworkPlayground.Service.Books
{
    public interface ICreateBookLinksStrategy
    {
        BookDTO CreateLinksForBookResource(BookDTO book);
        LinkedCollectionResourceWrapperDTO<BookDTO> CreateLinksForBooks(LinkedCollectionResourceWrapperDTO<BookDTO> booksWrapper, PagingResourceParameters pagingResourceParameters, bool hasPrevious, bool hasNext);
    }
}