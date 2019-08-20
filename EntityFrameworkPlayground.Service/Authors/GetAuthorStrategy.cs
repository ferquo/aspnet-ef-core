using AutoMapper;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.DataTransferObjects;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.Service.Authors
{
    public class GetAuthorStrategy : IGetAuthorStrategy
    {
        private readonly IMapper mapper;
        private readonly IAuthorRepository authorRepository;
        private readonly ICreateLinksStrategy createLinksStrategy;

        public GetAuthorStrategy(
            IMapper mapper,
            IAuthorRepository authorRepository,
            ICreateLinksStrategy createLinksStrategy)
        {
            this.mapper = mapper;
            this.authorRepository = authorRepository;
            this.createLinksStrategy = createLinksStrategy;
        }

        public async Task<AuthorDTO> GetAuthor(int id)
        {
            var author = await authorRepository.GetAuthorById(id);
            if (author == null)
            {
                return null;
            }
            return createLinksStrategy.CreateLinksForAuthorResource(mapper.Map<AuthorDTO>(author));
        }
    }
}
