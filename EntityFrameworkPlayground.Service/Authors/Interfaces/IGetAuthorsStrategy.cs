using EntityFrameworkPlayground.Domain.DataTransferObjects;
using EntityFrameworkPlayground.Domain.Models;

namespace EntityFrameworkPlayground.Service.Authors
{
    public interface IGetAuthorsStrategy
    {
        LinkedCollectionResourceWrapperDTO<AuthorDTO> GetAuthorsCollection(PagingResourceParameters paging);
    }
}
