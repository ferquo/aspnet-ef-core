using EntityFrameworkPlayground.Domain.DataTransferObjects;
using EntityFrameworkPlayground.Domain.Models;
using EntityFrameworkPlayground.Service.Authors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.API.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IGetAuthorsStrategy getAuthorsStrategy;
        private readonly IGetAuthorStrategy getAuthorStrategy;
        private readonly ICreateAuthorStrategy createAuthorStrategy;
        private readonly IDeleteAuthorStrategy deleteAuthorStrategy;

        public AuthorsController(
            IGetAuthorsStrategy getAuthorsStrategy,
            IGetAuthorStrategy getAuthorStrategy,
            ICreateAuthorStrategy createAuthorStrategy,
            IDeleteAuthorStrategy deleteAuthorStrategy)
        {
            this.getAuthorsStrategy = getAuthorsStrategy;
            this.getAuthorStrategy = getAuthorStrategy;
            this.createAuthorStrategy = createAuthorStrategy;
            this.deleteAuthorStrategy = deleteAuthorStrategy;
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
            var author = await getAuthorStrategy.GetAuthor(id);
            return Ok(author);
        }

        // POST: api/Authors
        [HttpPost(Name = "CreateAuthor")]
        public async Task<IActionResult> Post([FromBody] AuthorForCreationDTO author)
        {
            if (author == null)
            {
                return BadRequest();
            }
            var authorToReturn = await createAuthorStrategy.CreateAuthor(author);
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
            await deleteAuthorStrategy.Delete(id);
            return NoContent();
        }
    }
}
