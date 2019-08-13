using AutoMapper;
using EntityFrameworkPlayground.API.ViewModels;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.Entitities;
using EntityFrameworkPlayground.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.API.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAuthorRepository authorRepository;
        private readonly IUrlHelper urlHelper;

        public AuthorsController(
            IMapper mapper,
            IAuthorRepository authorRepository,
            IUrlHelper urlHelper)
        {
            this.mapper = mapper;
            this.authorRepository = authorRepository;
            this.urlHelper = urlHelper;
        }

        // GET: api/authors
        [HttpGet(Name = "GetAuthors")]
        public IActionResult Get([FromQuery]PagingResourceParameters paging, [FromHeader(Name = "Accept")]string mediaType)
        {
            var authors = authorRepository.GetAllAuthors(paging);
            var authorsToReturn = mapper.Map<IEnumerable<AuthorDTO>>(authors);
            authorsToReturn = authorsToReturn.Select(author =>
            {
                author = CreateLinksForAuthorResource(author);
                return author;
            });

            if (mediaType == "application/vnd.hateoas+json")
            {
                var authorsWrapper = new LinkedCollectionResourceWrapperDTO<AuthorDTO>(authorsToReturn);
                return Ok(CreateLinksForAuthors(authorsWrapper, paging, authors.HasPrevious, authors.HasNext));
            }
            else
            {
                var previousPageLink = authors.HasPrevious ?
                    CreateResourceUri(paging, ResourceUriType.PreviousPage) : null;
                var nextPageLink = authors.HasNext ?
                    CreateResourceUri(paging, ResourceUriType.NextPage) : null;
                var paginationMetaData = new
                {
                    totalCount = authors.TotalCount,
                    pageSize = authors.PageSize,
                    currentPage = authors.CurrentPage,
                    totalPages = authors.TotalPages,
                    previousPageLink,
                    nextPageLink
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetaData));

                return Ok(authorsToReturn);
            }
        }

        private string CreateResourceUri(
            PagingResourceParameters pagingResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("GetAuthors",
                        new
                        {
                            searchQuery = pagingResourceParameters.SearchQuery,
                            pageNumber = pagingResourceParameters.PageNumber - 1,
                            pageSize = pagingResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("GetAuthors",
                        new
                        {
                            searchQuery = pagingResourceParameters.SearchQuery,
                            pageNumber = pagingResourceParameters.PageNumber + 1,
                            pageSize = pagingResourceParameters.PageSize
                        });
                default:
                    return urlHelper.Link("GetAuthors",
                        new
                        {
                            searchQuery = pagingResourceParameters.SearchQuery,
                            pageNumber = pagingResourceParameters.PageNumber,
                            pageSize = pagingResourceParameters.PageSize
                        });
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}", Name = "GetAuthor")]
        public async Task<IActionResult> Get(int id)
        {
            var author = await authorRepository.GetAuthorById(id);
            if (author == null)
            {
                NotFound();
            }
            return Ok(CreateLinksForAuthorResource(mapper.Map<AuthorDTO>(author)));
        }

        // POST: api/Authors
        [HttpPost(Name = "CreateAuthor")]
        public async Task<IActionResult> Post([FromBody] AuthorForCreationDTO author)
        {
            if (author == null)
            {
                return BadRequest();
            }
            var authorEntity = mapper.Map<Author>(author);
            await authorRepository.Create(authorEntity);
            var authorToReturn = mapper.Map<AuthorDTO>(authorEntity);
            return Created("GetAuthor", authorToReturn);
        }

        // PUT: api/Authors/5
        [HttpPut("{id}", Name = "UpdateAuthor")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}", Name = "DeleteAuthor")]
        public async Task<IActionResult> Delete(int id)
        {
            var authorExists = await authorRepository.Exists(id);
            if (!authorExists)
            {
                return NotFound();
            }

            await authorRepository.Delete(id);
            return NoContent();
        }

        private AuthorDTO CreateLinksForAuthorResource(AuthorDTO author)
        {
            author.Links = new List<LinkDTO>();

            author.Links.Add(new LinkDTO(
                href: urlHelper.Link("GetAuthor", new { id = author.Id }),
                rel: "self",
                method: "GET"));

            author.Links.Add(new LinkDTO(
                href: urlHelper.Link("GetBooks", new { authorId = author.Id }),
                rel: "children",
                method: "GET"));

            author.Links.Add(new LinkDTO(
                href: urlHelper.Link("CreateAuthor", null),
                rel: "create-author",
                method: "POST"));

            author.Links.Add(new LinkDTO(
                href: urlHelper.Link("UpdateAuthor", new { id = author.Id }),
                rel: "update-author",
                method: "PUT"));

            author.Links.Add(new LinkDTO(
                href: urlHelper.Link("DeleteAuthor", new { id = author.Id }),
                rel: "delete-author",
                method: "DELETE"));

            return author;
        }

        private LinkedCollectionResourceWrapperDTO<AuthorDTO> CreateLinksForAuthors(
            LinkedCollectionResourceWrapperDTO<AuthorDTO> authorsWrapper,
            PagingResourceParameters pagingResourceParameters,
            bool hasPrevious,
            bool hasNext)
        {
            authorsWrapper.Links = new List<LinkDTO>();

            authorsWrapper.Links.Add(new LinkDTO(
                href: urlHelper.Link("GetAuthors", new { }),
                rel: "self",
                method: "GET"));

            if (hasPrevious)
            {
                authorsWrapper.Links.Add(new LinkDTO(
                    href: urlHelper.Link("GetAuthors", new
                    {
                        searchQuery = pagingResourceParameters.SearchQuery,
                        pageNumber = pagingResourceParameters.PageNumber - 1,
                        pageSize = pagingResourceParameters.PageSize
                    }),
                    rel: "previous",
                    method: "GET"));
            }

            if (hasNext)
            {
                authorsWrapper.Links.Add(new LinkDTO(
                    href: urlHelper.Link("GetAuthors", new
                    {
                        searchQuery = pagingResourceParameters.SearchQuery,
                        pageNumber = pagingResourceParameters.PageNumber + 1,
                        pageSize = pagingResourceParameters.PageSize
                    }),
                    rel: "next",
                    method: "GET"));
            }

            return authorsWrapper;
        }
    }
}
