using AutoMapper;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.DataTransferObjects;
using EntityFrameworkPlayground.Domain.Entitities;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.Service.Authors
{
    public class CreateAuthorStrategy : ICreateAuthorStrategy
    {
        private readonly IMapper mapper;
        private readonly IAuthorRepository authorRepository;
        private readonly ICreateAuthorLinksStrategy createLinksStrategy;

        public CreateAuthorStrategy(
            IMapper mapper,
            IAuthorRepository authorRepository,
            ICreateAuthorLinksStrategy createLinksStrategy)
        {
            this.mapper = mapper;
            this.authorRepository = authorRepository;
            this.createLinksStrategy = createLinksStrategy;
        }

        public async Task<AuthorDTO> CreateAuthor(AuthorForCreationDTO author)
        {
            var authorEntity = mapper.Map<Author>(author);
            await authorRepository.Create(authorEntity);
            return createLinksStrategy.CreateLinksForAuthorResource(mapper.Map<AuthorDTO>(authorEntity));
        }
    }
}
