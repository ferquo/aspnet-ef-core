using AutoMapper;
using EntityFrameworkPlayground.API.ViewModels;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.Entitities;
using EntityFrameworkPlayground.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
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
        public IActionResult Get([FromQuery]PagingResourceParameters paging)
        {
            var authors = authorRepository.GetAllAuthors(paging);

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

            return Ok(mapper.Map<IEnumerable<AuthorDTO>>(authors));
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
                            pageNumber = pagingResourceParameters.PageNumber - 1,
                            pageSize = pagingResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("GetAuthors",
                        new
                        {
                            pageNumber = pagingResourceParameters.PageNumber + 1,
                            pageSize = pagingResourceParameters.PageSize
                        });
                default:
                    return urlHelper.Link("GetAuthors",
                        new
                        {
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
            return Ok(mapper.Map<AuthorDTO>(author));
        }

        // POST: api/Authors
        [HttpPost]
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
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
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
    }
}
