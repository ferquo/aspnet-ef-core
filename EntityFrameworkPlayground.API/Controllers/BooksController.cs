using EntityFrameworkPlayground.Domain.DataTransferObjects;
using EntityFrameworkPlayground.Domain.Models;
using EntityFrameworkPlayground.Service.Books;
using EntityFrameworkPlayground.Service.Core;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.API.Controllers
{
    [Route("api/authors/{authorId}/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IGetBooksStrategy getBooksStrategy;
        private readonly IGetBookStrategy getBookStrategy;
        private readonly ICreateBookStrategy createBookStrategy;
        private readonly IUpdateBookStrategy updateBookStrategy;
        private readonly IDeleteBookStrategy deleteBookStrategy;
        private readonly IValidationStrategy bookValidationStrategy;

        public BooksController(
            IGetBooksStrategy getBooksStrategy,
            IGetBookStrategy getBookStrategy,
            ICreateBookStrategy createBookStrategy,
            IUpdateBookStrategy updateBookStrategy,
            IDeleteBookStrategy deleteBookStrategy,
            IValidationStrategy bookValidationStrategy)
        {
            this.getBooksStrategy = getBooksStrategy;
            this.getBookStrategy = getBookStrategy;
            this.createBookStrategy = createBookStrategy;
            this.updateBookStrategy = updateBookStrategy;
            this.deleteBookStrategy = deleteBookStrategy;
            this.bookValidationStrategy = bookValidationStrategy;
        }

        // GET: api/Books
        [HttpGet(Name = "GetBooks")]
        public IActionResult Get(int authorId, [FromQuery]PagingResourceParameters paging)
        {
            var books = getBooksStrategy.GetBooks(authorId, paging);
            return Ok(books);
        }

        // GET: api/Books/5
        [HttpGet("{id}", Name = "GetBook")]
        public async Task<IActionResult> Get(int authorId, int id)
        {
            var book = await getBookStrategy.GetBookById(authorId, id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        // POST: api/Books
        [HttpPost(Name = "CreateBook")]
        public async Task<IActionResult> Post(int authorId, [FromBody] BookForCreationDTO value)
        {
            if (value == null)
            {
                return BadRequest();
            }

            if (!bookValidationStrategy.IsValid(value))
            {
                return new UnprocessableEntityObjectResult(bookValidationStrategy.GetValidationResults(value));
            }

            var authorExists = await createBookStrategy.AuthorExists(authorId);
            if (!authorExists)
            {
                return NotFound();
            }

            var bookToReturn = await createBookStrategy.CreateBook(authorId, value);

            return Created("/", bookToReturn);
        }

        // PUT: api/Books/5
        [HttpPut("{id}", Name = "UpdateBook")]
        public async Task<IActionResult> Put(int authorId, int id, [FromBody] BookForUpdateDTO value)
        {
            if (!bookValidationStrategy.IsValid(value))
            {
                return new UnprocessableEntityObjectResult(bookValidationStrategy.GetValidationResults(value));
            }

            var updatedBook = await updateBookStrategy.UpdateBook(authorId, id, value);

            return Ok(updatedBook);
        }

        [HttpPatch("{id}", Name = "UpdatePartialBook")]
        public async Task<IActionResult> Patch(int authorId, int id, [FromBody] JsonPatchDocument<BookForUpdateDTO> patchDoc)
        {
            var bookToPatch = await updateBookStrategy.ApplyPatch(authorId, id, patchDoc);

            if (!bookValidationStrategy.IsValid(bookToPatch))
            {
                return new UnprocessableEntityObjectResult(bookValidationStrategy.GetValidationResults(bookToPatch));
            }

            var updatedBook = await updateBookStrategy.UpdateBook(authorId, id, bookToPatch);

            return Ok(updatedBook);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}", Name = "DeleteBook")]
        public async Task<IActionResult> Delete(int authorId, int id)
        {
            await deleteBookStrategy.Delete(authorId, id);
            return NoContent();
        }
    }
}
