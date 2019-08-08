using AutoMapper;
using EntityFrameworkPlayground.API.ViewModels;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.Entitities;
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

        public BooksController(
            IMapper mapper,
            IBooksRepository booksRepository)
        {
            this.mapper = mapper;
            this.booksRepository = booksRepository;
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
        public async Task<IActionResult> Post([FromBody] BookDTO value)
        {
            try
            {
                var newBook = mapper.Map<Book>(value);
                await booksRepository.Create(newBook);
                return Created("GetBook", mapper.Map<BookDTO>(newBook));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Book value)
        {
            try
            {
                await booksRepository.Update(id, value);
                Ok(value);
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await booksRepository.Delete(id);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest();
                throw;
            }
        }
    }
}
