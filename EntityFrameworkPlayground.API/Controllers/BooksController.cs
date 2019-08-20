using AutoMapper;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.DataTransferObjects;
using EntityFrameworkPlayground.Domain.Entitities;
using EntityFrameworkPlayground.Domain.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public IActionResult Get(int authorId, [FromQuery]PagingResourceParameters paging, [FromHeader(Name = "Accept")]string mediaType)
        {
            var books = booksRepository.GetAllBooksByAuthor(authorId, paging);
            var booksToReturn = mapper.Map<IEnumerable<BookDTO>>(books);
            booksToReturn = booksToReturn.Select(book =>
            {
                book = CreateLinksForBookResource(book);
                return book;
            });

            if (mediaType == "application/vnd.hateoas+json")
            {

                var wrapper = new LinkedCollectionResourceWrapperDTO<BookDTO>(booksToReturn);
                return Ok(CreateLinksForBooks(wrapper, paging, books.HasPrevious, books.HasNext));
            }
            else
            {
                var previousPageLink = books.HasPrevious ?
                    CreateResourceUri(paging, ResourceUriType.PreviousPage) : null;
                var nextPageLink = books.HasNext ?
                    CreateResourceUri(paging, ResourceUriType.NextPage) : null;
                var paginationMetaData = new
                {
                    totalCount = books.TotalCount,
                    pageSize = books.PageSize,
                    currentPage = books.CurrentPage,
                    totalPages = books.TotalPages,
                    previousPageLink,
                    nextPageLink
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetaData));
                return Ok(booksToReturn);
            }
        }

        private string CreateResourceUri(
            PagingResourceParameters pagingResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return urlHelper.Link("GetBooks",
                        new
                        {
                            searchQuery = pagingResourceParameters.SearchQuery,
                            pageNumber = pagingResourceParameters.PageNumber - 1,
                            pageSize = pagingResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return urlHelper.Link("GetBooks",
                        new
                        {
                            searchQuery = pagingResourceParameters.SearchQuery,
                            pageNumber = pagingResourceParameters.PageNumber + 1,
                            pageSize = pagingResourceParameters.PageSize
                        });
                default:
                    return urlHelper.Link("GetBooks",
                        new
                        {
                            searchQuery = pagingResourceParameters.SearchQuery,
                            pageNumber = pagingResourceParameters.PageNumber,
                            pageSize = pagingResourceParameters.PageSize
                        });
            }
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
            return Ok(CreateLinksForBookResource(mapper.Map<BookDTO>(book)));
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

        private BookDTO CreateLinksForBookResource(BookDTO book)
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

        private LinkedCollectionResourceWrapperDTO<BookDTO> CreateLinksForBooks(
            LinkedCollectionResourceWrapperDTO<BookDTO> booksWrapper,
            PagingResourceParameters pagingResourceParameters,
            bool hasPrevious,
            bool hasNext)
        {
            booksWrapper.Links = new List<LinkDTO>();

            booksWrapper.Links.Add(new LinkDTO(
                href: urlHelper.Link("GetBooks", new { }),
                rel: "self",
                method: "GET"));

            if (hasPrevious)
            {
                booksWrapper.Links.Add(new LinkDTO(
                    href: urlHelper.Link("GetBooks", new
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
                booksWrapper.Links.Add(new LinkDTO(
                    href: urlHelper.Link("GetBooks", new
                    {
                        searchQuery = pagingResourceParameters.SearchQuery,
                        pageNumber = pagingResourceParameters.PageNumber + 1,
                        pageSize = pagingResourceParameters.PageSize
                    }),
                    rel: "next",
                    method: "GET"));
            }

            return booksWrapper;
        }
    }
}
