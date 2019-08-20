using AutoMapper;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.DataTransferObjects;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;

namespace EntityFrameworkPlayground.Service.Books
{
    public class UpdateBookStrategy : IUpdateBookStrategy
    {
        private readonly IMapper mapper;
        private readonly IBooksRepository booksRepository;
        private readonly IAuthorRepository authorsRepository;
        private readonly ICreateBookLinksStrategy createLinksStrategy;

        public UpdateBookStrategy(
            IMapper mapper,
            IBooksRepository booksRepository,
            IAuthorRepository authorsRepository,
            ICreateBookLinksStrategy createLinksStrategy)
        {
            this.mapper = mapper;
            this.booksRepository = booksRepository;
            this.authorsRepository = authorsRepository;
            this.createLinksStrategy = createLinksStrategy;
        }

        public async Task<bool> AuthorExists(int authorId)
        {
            return await authorsRepository.Exists(authorId);
        }

        public async Task<bool> BookExists(int bookId)
        {
            return await booksRepository.GetById(bookId) != null;
        }

        public async Task<BookDTO> UpdateBook(int bookId, BookForUpdateDTO value)
        {
            var bookToUpdate = await booksRepository.GetById(bookId);
            mapper.Map(value, bookToUpdate);
            await booksRepository.Update(bookId, bookToUpdate);

            return createLinksStrategy.CreateLinksForBookResource(mapper.Map<BookDTO>(bookToUpdate));
        }

        public async Task<BookForUpdateDTO> ApplyPatch(int bookId, JsonPatchDocument<BookForUpdateDTO> patchDoc)
        {
            var bookFromRepo = await booksRepository.GetById(bookId);
            var bookToPatch = mapper.Map<BookForUpdateDTO>(bookFromRepo);
            patchDoc.ApplyTo(bookToPatch);

            return bookToPatch;
        }
    }
}
