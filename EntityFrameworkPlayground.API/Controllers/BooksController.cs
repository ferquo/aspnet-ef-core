using AutoMapper;
using EntityFrameworkPlayground.API.ViewModels;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.Entitities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        public BooksController(
            IMapper mapper,
            IBooksRepository booksRepository,
            IAuthorRepository authorRepository)
        {
            this.mapper = mapper;
            this.booksRepository = booksRepository;
            this.authorRepository = authorRepository;
        }

        // GET: api/Books
        [HttpGet]
        public IActionResult Get(int authorId)
        {
            var books = mapper.Map<IEnumerable<BookDTO>>(booksRepository.GetAllBooksByAuthor(authorId));
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
            return Ok(mapper.Map<BookDTO>(book));
        }

        // POST: api/Books
        [HttpPost]
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
        [HttpPut("{id}")]
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

        [HttpPatch("{id}")]
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
        [HttpDelete("{id}")]
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
    }
}
