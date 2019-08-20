using AutoMapper;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.DataTransferObjects;
using EntityFrameworkPlayground.Domain.Entitities;
using EntityFrameworkPlayground.Domain.Models;
using EntityFrameworkPlayground.Service.Authors;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IGetAuthorsStrategy getAuthorsStrategy;
        private readonly IUrlHelper urlHelper;

        public AuthorsController(
            IMapper mapper,
            IAuthorRepository authorRepository,
            IGetAuthorsStrategy getAuthorsStrategy,
            IUrlHelper urlHelper)
        {
            this.mapper = mapper;
            this.authorRepository = authorRepository;
            this.getAuthorsStrategy = getAuthorsStrategy;
            this.urlHelper = urlHelper;
        }

        // GET: api/authors
        [HttpGet(Name = "GetAuthors")]
        public IActionResult Get([FromQuery]PagingResourceParameters paging)
        {
            var authorsToReturn = getAuthorsStrategy.GetAuthorsCollection(paging);
            return Ok(authorsToReturn);
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

    }
}
