using AutoMapper;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Domain.DataTransferObjects;
using EntityFrameworkPlayground.Domain.Exceptions;
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

        public async Task<BookDTO> UpdateBook(int authorId, int bookId, BookForUpdateDTO value)
        {
            if (await authorsRepository.Exists(authorId))
            {
                throw new NotFoundException("Author", authorId);
            }

            var bookToUpdate = await booksRepository.GetById(bookId);
            if (bookToUpdate == null)
            {
                throw new NotFoundException("Book", bookId);
            }
            mapper.Map(value, bookToUpdate);
            await booksRepository.Update(bookId, bookToUpdate);

            return createLinksStrategy.CreateLinksForBookResource(mapper.Map<BookDTO>(bookToUpdate));
        }

        public async Task<BookForUpdateDTO> ApplyPatch(int authorId, int bookId, JsonPatchDocument<BookForUpdateDTO> patchDoc)
        {
            if (await authorsRepository.Exists(authorId))
            {
                throw new NotFoundException("Author", authorId);
            }

            var bookFromRepo = await booksRepository.GetById(bookId);
            if (bookFromRepo == null)
            {
                throw new NotFoundException("Book", bookId);
            }
            var bookToPatch = mapper.Map<BookForUpdateDTO>(bookFromRepo);
            patchDoc.ApplyTo(bookToPatch);

            return bookToPatch;
        }
    }
}
