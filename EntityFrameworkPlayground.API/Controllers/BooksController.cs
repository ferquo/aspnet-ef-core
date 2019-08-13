using AutoMapper;
using EntityFrameworkPlayground.API.ViewModels;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.Entitities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.API.Controllers
{
    [Route("api/authors/{authorId}/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IBooksRepository booksRepository;
        private readonly IAuthorRepository authorRepository;
        private readonly IUrlHelper urlHelper;

        public BooksController(
            IMapper mapper,
            IBooksRepository booksRepository,
            IAuthorRepository authorRepository,
            IUrlHelper urlHelper)
        {
            this.mapper = mapper;
            this.booksRepository = booksRepository;
            this.authorRepository = authorRepository;
            this.urlHelper = urlHelper;
        }

        // GET: api/Books
        [HttpGet(Name = "GetBooks")]
        public IActionResult Get(int authorId)
        {
            var books = mapper.Map<IEnumerable<BookDTO>>(booksRepository.GetAllBooksByAuthor(authorId));
            books = books.Select(book =>
            {
                book = CreateLinksForAuthorResource(book);
                return book;
            });
            return Ok(books);
        }

        // GET: api/Books/5
        [HttpGet("{id}", Name = "GetBook")]
        public async Task<IActionResult> Get(int authorId, int id)
        {
            var book = await booksRepository.GetbyIdIncludeAuthor(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(CreateLinksForAuthorResource(mapper.Map<BookDTO>(book)));
        }

        // POST: api/Books
        [HttpPost(Name = "CreateBook")]
        public async Task<IActionResult> Post(int authorId, [FromBody] BookForCreationDTO value)
        {
            if (value == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var authorExists = await authorRepository.Exists(authorId);
            if (!authorExists)
            {
                return NotFound();
            }

            var bookEntity = mapper.Map<Book>(value);
            await booksRepository.AddBookToAuthor(authorId, bookEntity);
            var bookToReturn = mapper.Map<BookDTO>(bookEntity);

            return Created("/", bookToReturn);
        }

        // PUT: api/Books/5
        [HttpPut("{id}", Name = "UpdateBook")]
        public async Task<IActionResult> Put(int authorId, int id, [FromBody] BookForUpdateDTO value)
        {
            var authorExists = await authorRepository.Exists(authorId);
            if (!authorExists)
            {
                return NotFound();
            }

            var bookToUpdate = await booksRepository.GetById(id);
            if (bookToUpdate == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            mapper.Map(value, bookToUpdate);
            await booksRepository.Update(id, bookToUpdate);

            return Ok(mapper.Map<BookDTO>(bookToUpdate));
        }

        [HttpPatch("{id}", Name = "UpdatePartialBook")]
        public async Task<IActionResult> Patch(int authorId, int id, [FromBody] JsonPatchDocument<BookForUpdateDTO> patchDoc)
        {
            var authorExists = await authorRepository.Exists(authorId);
            if (!authorExists)
            {
                return NotFound();
            }

            var bookFromRepo = await booksRepository.GetById(id);
            if (bookFromRepo == null)
            {
                return NotFound();
            }

            var bookToPatch = mapper.Map<BookForUpdateDTO>(bookFromRepo);
            patchDoc.ApplyTo(bookToPatch, ModelState);

            TryValidateModel(bookToPatch);
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            mapper.Map(bookToPatch, bookFromRepo);
            await booksRepository.Update(id, bookFromRepo);

            return Ok(mapper.Map<BookDTO>(bookFromRepo));
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}", Name = "DeleteBook")]
        public async Task<IActionResult> Delete(int authorId, int id)
        {
            var authorExists = await authorRepository.Exists(authorId);
            if (!authorExists)
            {
                return NotFound();
            }

            var bookFromRepo = await booksRepository.GetById(id);
            if (bookFromRepo == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            await booksRepository.Delete(id);
            return NoContent();
        }

        private BookDTO CreateLinksForAuthorResource(BookDTO book)
        {
            book.Links = new List<LinkDTO>();

            book.Links.Add(new LinkDTO(
                href: urlHelper.Link("GetBook", new { authorId = book.AuthorId, id = book.Id }),
                rel: "self",
                method: "GET"));

            book.Links.Add(new LinkDTO(
                href: urlHelper.Link("GetAuthor", new { id = book.AuthorId }),
                rel: "parrent",
                method: "GET"));

            book.Links.Add(new LinkDTO(
                href: urlHelper.Link("CreateBook", new { authorId = book.AuthorId }),
                rel: "create-book",
                method: "POST"));

            book.Links.Add(new LinkDTO(
                href: urlHelper.Link("UpdateBook", new { authorId = book.AuthorId, id = book.Id }),
                rel: "update-book",
                method: "PUT"));

            book.Links.Add(new LinkDTO(
                href: urlHelper.Link("UpdateBook", new { authorId = book.AuthorId, id = book.Id }),
                rel: "update-partial-book",
                method: "PATCH"));

            book.Links.Add(new LinkDTO(
                href: urlHelper.Link("DeleteBook", new { authorId = book.AuthorId, id = book.Id }),
                rel: "delete-book",
                method: "DELETE"));

            return book;
        }
    }
}
