using EntityFrameworkPlayground.Domain.DataTransferObjects;
using EntityFrameworkPlayground.Domain.Models;

namespace EntityFrameworkPlayground.Service.Authors
{
    public interface ICreateAuthorLinksStrategy
    {
        AuthorDTO CreateLinksForAuthorResource(AuthorDTO author);
        LinkedCollectionResourceWrapperDTO<AuthorDTO> CreateLinksForAuthors(LinkedCollectionResourceWrapperDTO<AuthorDTO> authorsWrapper, PagingResourceParameters pagingResourceParameters, bool hasPrevious, bool hasNext);
    }
}