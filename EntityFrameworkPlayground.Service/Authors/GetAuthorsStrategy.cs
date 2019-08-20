using AutoMapper;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.DataTransferObjects;
using EntityFrameworkPlayground.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace EntityFrameworkPlayground.Service.Authors
{
    public class GetAuthorsStrategy : IGetAuthorsStrategy
    {
        private readonly IMapper mapper;
        private readonly IAuthorRepository authorRepository;
        private readonly ICreateAuthorLinksStrategy createLinksStrategy;

        public GetAuthorsStrategy(
            IMapper mapper,
            IAuthorRepository authorRepository,
            ICreateAuthorLinksStrategy createLinksStrategy)
        {
            this.mapper = mapper;
            this.authorRepository = authorRepository;
            this.createLinksStrategy = createLinksStrategy;
        }

        public LinkedCollectionResourceWrapperDTO<AuthorDTO> GetAuthorsCollection(PagingResourceParameters paging)
        {
            var authors = authorRepository.GetAllAuthors(paging);
            var authorsToReturn = mapper.Map<IEnumerable<AuthorDTO>>(authors);
            authorsToReturn = authorsToReturn.Select(author =>
            {
                author = createLinksStrategy.CreateLinksForAuthorResource(author);
                return author;
            });
            var authorsWrapper = new LinkedCollectionResourceWrapperDTO<AuthorDTO>(authorsToReturn);

            return createLinksStrategy.CreateLinksForAuthors(authorsWrapper, paging, authors.HasPrevious, authors.HasNext);
        }
    }
}
