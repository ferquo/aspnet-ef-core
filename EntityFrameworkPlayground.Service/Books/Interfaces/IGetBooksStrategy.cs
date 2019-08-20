using EntityFrameworkPlayground.Domain.DataTransferObjects;
using EntityFrameworkPlayground.Domain.Models;

namespace EntityFrameworkPlayground.Service.Books
{
    public interface IGetBooksStrategy
    {
        LinkedCollectionResourceWrapperDTO<BookDTO> GetBooks(int authorId, PagingResourceParameters paging);
    }
}